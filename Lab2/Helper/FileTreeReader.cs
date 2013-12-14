using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Attribute = Lab2_Correct.Core.Attribute;

namespace Lab2_Correct.Helper
{
    class FileTreeReader
    {
        public string GoalAttrName { get; set; }

        public List<Attribute> ReadTable(string fileName)
        {
            var attrs = ReadHeaders(fileName);
            FillAttrsWithData(attrs, fileName);
            return attrs;
        }

        private List<Attribute> ReadHeaders(string fileName)
        {
            var attrs = new List<Attribute>();
            var inputTextLines = File.ReadAllLines(fileName);
            var headers = inputTextLines[0].Split('\t');
            foreach (var header in headers)
            {
                var matches = Regex.Matches(header, @"(\w|\s)+");
                var type = matches[1].Value;
                var isQuantity = type == "q";
                attrs.Add(new Attribute(matches[0].Value, isQuantity));

                if (type == "g") GoalAttrName = matches[0].Value;
            }
            return attrs;
        }

        private void FillAttrsWithData(List<Attribute> attrs, string fileName)
        {
            var inputTextLines = File.ReadAllLines(fileName);
            for (int row = 1; row < inputTextLines.Count(); row++)
            {
                var strValues = inputTextLines[row].Split('\t');
                for (int valNum = 0; valNum < strValues.Count(); valNum++)
                {
                    attrs[valNum].Values.Add(strValues[valNum]);
                }
            }
        }
    }
}
