using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace Lab3.Core
{
    class Neuron
    {
        public double Output { get; set; }
        public double Delta { get; set; }
        public int Inputs { get; set; }
        public List<double> Weights { get; set; }
        public List<double> DeltaWeights { get; set; }

        public Neuron(int inputs)
        {
            Inputs = inputs;
            Weights = new List<double>(inputs);
            DeltaWeights = new List<double>(inputs);
            var rand = new Random();

            for (int i = 0; i < inputs; i++)
            {
                Weights.Add((rand.NextDouble() % 0.6 - 0.3));
                DeltaWeights.Add(0);
            }
        }
    }
}
