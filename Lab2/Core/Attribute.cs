using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_Correct.Core
{
    [Serializable]
    public class Attribute
    {
        public Attribute(string name, bool isQuantity)
        {
            Values = new List<string>();
            Name = name;
            IsQuantity = isQuantity;
        }

        public bool IsQuantity { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }
        public double InfoGain { get; set; }
    }
}
