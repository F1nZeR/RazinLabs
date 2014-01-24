using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Label = System.Windows.Forms.Label;

namespace Lab3.Core
{
    public class NeuralWorker
    {
        private readonly NeuralNetwork _neuralNetwork;
        private List<Test> _learningTuples;
        private static readonly Random Rnd = new Random();
        private readonly Label _label;

        public NeuralWorker(string path, Label label)
        {
            _label = label;
            _neuralNetwork = new NeuralNetwork(600, 3, 60, 10, 0.5, 0.5);
            CreateLearningTuples(path);
        }

        private void CreateLearningTuples(string path)
        {
            _learningTuples = new List<Test>();
            var folders = Directory.EnumerateDirectories(path);
            var trainingDict = folders.ToDictionary(x => int.Parse(Path.GetFileName(x)), x => Directory.EnumerateFiles(x).ToList());

            while (trainingDict.All(x => x.Value.Any()))
            {
                int number;
                do
                {
                    number = Rnd.Next(10);
                } while (!trainingDict[number].Any());

                var folder = trainingDict[number];
                var img = folder[Rnd.Next(folder.Count)];
                folder.Remove(img);

                var output = new double[10];
                output[number] = 1;

                var imgVector = Helper.GetBinaryVectorFromImage(img).Select(x => (double)x).ToList();
                var test = new Test(imgVector, number);
                _learningTuples.Add(test);
            }
        }

        public void Learn()
        {
            _neuralNetwork.Teach(_learningTuples, 0.05, _label);
        }

        public List<double> Recognize(List<double> input)
        {
            ;
            return _neuralNetwork.Calculate(input);
        }
    }
}
