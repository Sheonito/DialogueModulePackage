using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public class EpisodeData : ScriptableObject
    {
        public IReadOnlyList<TextElement> TextElements => _textElements;
        [SerializeField] private List<TextElement> _textElements = new List<TextElement>();

        public TextElement GetNextTextElement(int curIndex)
        {
            return _textElements[curIndex + 1];
        }

#if UNITY_EDITOR
        public void UpdateData(List<TextElement> updateTextElements)
        {
            bool isChanged = CheckChanges(updateTextElements);
            if (isChanged)
            {
                UIDMergeWindow uidMergeWindow = CreateInstance<UIDMergeWindow>();    
                uidMergeWindow.ShowWindow(_textElements.ToList(), updateTextElements,name);
            }
            
            _textElements.Clear();
            _textElements.AddRange(updateTextElements);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        
        
        public bool CheckChanges(List<TextElement> curTextElements)
        {
            bool isAnyElementsChanged = false;
            for (int i = 0; i < _textElements.Count; i++)
            {
                TextElement prevTextElement = _textElements[i];
                bool hasNewElement = curTextElements.Count > i;
                if (hasNewElement)
                {
                    TextElement newTextElement = curTextElements[i];
                    string oldText = prevTextElement.Text;
                    string newText = newTextElement.Text;
                    bool isChanged = oldText != newText;
                    prevTextElement.SetTextChanged(isChanged);
                    if (isChanged)
                    {
                        isAnyElementsChanged = true;
                    }
                    else
                    {
                        newTextElement.SetUid(prevTextElement.Uid);
                    }
                }
                else
                {
                    prevTextElement.SetTextChanged(true);
                    isAnyElementsChanged = true;
                }
            }

            return isAnyElementsChanged;
        }
#endif
    }
}
