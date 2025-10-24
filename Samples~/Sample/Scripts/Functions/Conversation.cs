using System;
using System.Collections.Generic;
using System.Linq;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Aftertime.HappinessBlossom
{
    public delegate void ConversationStartAction(string conversation, string[] characterNames);
    public delegate void InlineFunctionAction(StorylineEngine.Function function,InlineFunction inlineFunction);

    public class Conversation : StorylineEngine.Function
    {
        [Presenter] private ConversationPresenter _presenter;
        public event ConversationStartAction onConversationStart = delegate { };

        public event InlineFunctionAction onInlineFunction = delegate { };

        private const string FunctionPrefix = "{{";
        private const string FunctionSuffix = "}}";

        private bool isEventRegister;
        private bool isTextShowed;

        public override void Init()
        {
            base.Init();

            if (!isEventRegister)
            {
                RegisterEvent();
            }
        }

        private void RegisterEvent()
        {
            isEventRegister = true;
            _presenter.onTextShowed += () => isTextShowed = true;
            _presenter.onTextCompletedShowed += ExecuteFunctionInConversationImmediate;
        }


        public override async UniTask StartFunction()
        {
            base.StartFunction();
            
            onConversationStart?.Invoke(TextElement.Conversation, TextElement.Characters);

            isTextShowed = false;
            string conversation = TextElement.Conversation;
            string characterName = GetCharacterName(TextElement.Characters);
            string userName = GetUserName();

            string userNameKey = StorylineEngineSettings.Instance.CharacterSettings.UserNameKey;
            string userNameParsedText = conversation.Replace(userNameKey, userName);

            _presenter.ShowText(userNameParsedText);
            _presenter.UpdateName(characterName);
            PlayVoice();

            InlineFunction[] functionDataArray = GetFunctionInConversation(conversation);
            ExecuteFunctionInConversation(functionDataArray);
        }

        protected virtual string GetCharacterName(string[] originCharacterNames)
        {
            List<string> characterNames = new List<string>();
            foreach (string originCharacterName in originCharacterNames)
            {
                CharacterSettings settings = StorylineEngineSettings.Instance.CharacterSettings;
                string userNameKey = settings.UserNameKey;
                string anonymousNameKey = settings.AnonymousNameKey;
                
                bool isAnonymous = originCharacterName.Contains(anonymousNameKey);
                bool hasUserName = originCharacterName.Contains(userNameKey);
                string characterName;
                if (isAnonymous)
                {
                    characterName = settings.AnonymousName;
                }
                else if (hasUserName)
                {
                    return GetUserName();
                }
                else
                {
                    characterName = originCharacterName;
                }
                
                characterNames.Add(characterName);
            }

            return string.Join(",",characterNames);
        }

        protected virtual string GetUserName()
        {
            return "미구현";
        }

        public virtual void PlayVoice()
        {
            string[] voicePath = TextElement.VoicePath;
            bool hasVoice = TextElement.VoicePath.Length > 0;
            if (hasVoice)
            {
                SoundManager.Instance.PlayVoice(voicePath);
            }
        }

        private InlineFunction[] GetFunctionInConversation(string rawConversation)
        {
            List<string> functionTexts = GetFunctionTexts(rawConversation);
            if (functionTexts == null || functionTexts.Count == 0)
                return null;

            List<InlineFunction> functionInConversations = new List<InlineFunction>();
            foreach (string functionText in functionTexts)
            {
                string functionName = GetFunctionName(functionText);
                string[] functionValues = GetFunctionValues(functionText);
                int functionStartIndex = GetFunctionStartIndex(functionText);
                rawConversation = rawConversation.Replace(functionText, "");

                InlineFunction inlineFunction =
                    new InlineFunction(functionText, functionName, functionValues, functionStartIndex);
                functionInConversations.Add(inlineFunction);
            }

            return functionInConversations.ToArray();

            List<string> GetFunctionTexts(string rawConversation)
            {
                string[] functionTexts = rawConversation.Split(FunctionPrefix);
                if (functionTexts.Length == 0)
                    return null;

                List<string> functionTextList = functionTexts.ToList();
                int nonFunctionIndex = 0;
                functionTextList.RemoveAt(nonFunctionIndex);
                for (int i = 0; i < functionTextList.Count; i++)
                {
                    int functionEndIndex = functionTextList[i].IndexOf(FunctionSuffix);
                    string nonFunctionText = functionTextList[i].Substring(functionEndIndex);
                    functionTextList[i] = functionTextList[i].Replace(nonFunctionText, "");
                }

                return functionTextList;
            }

            string GetFunctionName(string functionText)
            {
                string functionName = functionText.Split("=")[0];
                functionName = functionName.Replace("#", "");
                return functionName;
            }

            string[] GetFunctionValues(string functionText)
            {
                if (functionText.Contains("=") == false)
                    return null;

                string splitText = functionText.Split("=")[1];
                string[] functionValues = splitText.Split(",");

                for (int i = 0; i < functionValues.Length; i++)
                {
                    functionValues[i] = functionValues[i].Replace(",", "");
                }

                return functionValues;
            }

            int GetFunctionStartIndex(string functionText)
            {
                if (rawConversation.Contains(functionText) == false)
                    return -1;

                int index = rawConversation.IndexOf(functionText);

                return index;
            }
        }

        private async void ExecuteFunctionInConversation(InlineFunction[] functionDataArray)
        {
            if (functionDataArray == default || functionDataArray.Length == 0)
                return;

            while (isTextShowed == false)
            {
                int curConversationIndex = _presenter.GetShowingIndex();
                foreach (InlineFunction functionData in functionDataArray)
                {
                    int startIndex = functionData.StartIndex;
                    if (startIndex == curConversationIndex)
                    {
                        onInlineFunction(this,functionData);
                    }
                }
                
                await UniTask.Yield();
            }
        }

        private void ExecuteFunctionInConversationImmediate()
        {
            string rawConversation = TextElement.Conversation;
            InlineFunction[] functionDataArray = GetFunctionInConversation(rawConversation);
            if (functionDataArray == null || functionDataArray.Length == 0)
                return;

            foreach (InlineFunction functionData in functionDataArray)
            {
                onInlineFunction(this,functionData);
            }
        }
    }
}