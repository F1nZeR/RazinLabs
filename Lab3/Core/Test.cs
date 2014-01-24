using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Core
{
    class Test
    {
        public List<double> Input { get; set; }
        public List<double> ResValues { get; set; }
        public int Value { get; set; }

        public Test(List<double> inputs, int value)
        {
            Value = value;
            Input = inputs;
            
            var output = new double[10];
            output[value] = 1;
            ResValues = output.ToList();
        }
    }
}
