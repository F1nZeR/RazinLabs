using Lab2_Correct.Core;

namespace Lab2_Correct.Helper
{
    public class Logger
    {
        private static DecisionTree _prevId3;
        public static string LogText { get; set; }

        public static void Init(DecisionTree id3)
        {
            LogText = string.Empty;
            _prevId3 = id3;
        }

        public static void AddLogEntry(DecisionTree curId3, string entryText)
        {
            if (curId3 != _prevId3)
            {
                LogText += "---------------\n";
                _prevId3 = curId3;
            }
            LogText += entryText + "\n";
        }
    }
}
