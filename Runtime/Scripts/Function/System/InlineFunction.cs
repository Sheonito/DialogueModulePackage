namespace Aftertime.StorylineEngine
{
    public struct InlineFunction
    {
        public InlineFunction(string rawText, string functionName, string[] functionValues, int startIndex)
        {
            RawText = rawText;
            FunctionName = functionName;
            FunctionValues = functionValues;
            StartIndex = startIndex;
        }

        public string RawText { get; private set; }
        public string FunctionName { get; private set; }
        public string[] FunctionValues { get; private set; }
        public int StartIndex { get; private set; }
    }
}