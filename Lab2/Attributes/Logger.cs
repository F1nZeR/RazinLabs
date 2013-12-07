using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Attributes
{
    public class Logger
    {
        private static DecisionTreeId3 _prevId3;
        public static string LogText { get; set; }

        public static void Init(DecisionTreeId3 id3)
        {
            LogText = string.Empty;
            _prevId3 = id3;
        }

        public static void AddLogEntry(DecisionTreeId3 curId3, string entryText)
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
