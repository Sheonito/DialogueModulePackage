using System;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    [Serializable]
    public class TextElement
    {
        public string Text { get => text; private set => text = value; }
        public int Index { get => index; private set => index = value; }
        public string EpisodeName { get => episodeName; private set => episodeName = value; }
        public string Uid { get => uid; private set => uid = value; }
        public string[] Characters { get => characters; private set => characters = value; }
        public string FunctionName { get => functionName; private set => functionName = value; }
        public FunctionType FunctionType { get => functionType; private set => functionType = value; }
        internal string[] FunctionValuesRaw { get => functionValues; set => functionValues = value; }
        public FunctionValue FunctionValue => new FunctionValue(functionValues);
        public string[] SelectElements { get => selectElements; private set => selectElements = value; }
        public string Conversation { get => conversation; private set => conversation = value; }
        public string[] VoicePath { get => voicePath; private set => voicePath = value; }
        public bool IsTextChanged { get; private set; }

        [SerializeField,ReadOnly] protected string text;
        [SerializeField,ReadOnly] private int index;
        [SerializeField,ReadOnly] private string episodeName;
        [SerializeField,ReadOnly] private string uid;
        [SerializeField,ReadOnly] private string[] characters;
        [SerializeField,ReadOnly] private string functionName;
        [SerializeField,ReadOnly] private FunctionType functionType;
        [SerializeField,ReadOnly] private string[] functionValues;
        [SerializeField,ReadOnly] private string[] selectElements;
        [SerializeField,ReadOnly] private string conversation;
        [SerializeField,ReadOnly] private string[] voicePath;

        public TextElement()
        {
        }
        
        public TextElement(string inlineFunctionText)
        {
            text = inlineFunctionText;
        }

        public void SetTextChanged(bool isTextChanged)
        {
            IsTextChanged = isTextChanged;
        }

        public void SetUid(string newUid)
        {
            uid = newUid;
        }

        public void CreateUid()
        {
            uid = Guid.NewGuid().ToString();
        }
        
    }
}
