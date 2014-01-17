using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3.Core
{
    class NeuralNetwork
    {
        public int Inputs { get; set; }
        public int Depth { get; set; }
        public int HiddenLayerSize { get; set; }
        public int Outputs { get; set; }
        public double LearningSpeed { get; set; }
        public double Momentum { get; set; }

        private double _testError;
        private List<Layer> _layers;

        public NeuralNetwork(int inputs, int depth, int hiddenLayerSize, int outputs, double learningSpeed, double momentum)
        {
            Inputs = inputs;
            Depth = depth;
            HiddenLayerSize = hiddenLayerSize;
            Outputs = outputs;
            LearningSpeed = learningSpeed;
            Momentum = momentum;
            Init();
        }

        public void Teach(List<Tuple<List<double>, List<double>>> tests, double error, Label label)
        {
            var rnd = new Random();
            var currError = error + 1;
            while (currError > error)
            {
                tests = tests.OrderBy(x => rnd.Next()).ToList();
                currError = 0;
                foreach (var test in tests)
                {
                    ForwardPass(test.Item1);
                    BackwardPass(test.Item2);
                    currError += _testError;
                }
                currError /= tests.Count;
                label.Text = "Ошибка: " + currError;
                Application.DoEvents();
            }
            label.Text = "Обучение закончено";
        }
        
        private void Init()
        {
            _layers = new List<Layer>(Depth + 2);
            _layers.Add(new Layer(Inputs, 0));
            for (int i = 1; i < Depth - 1; i++)
            {
                _layers.Add(new Layer(HiddenLayerSize, _layers[i - 1].Size));
            }
            _layers.Add(new Layer(Outputs, HiddenLayerSize));
        }

        public List<double> Calculate(List<double> input)
        {
            var answer = new List<double>(Outputs);
            ForwardPass(input);
            for (int i = 0; i < Outputs; i++)
            {
                answer.Add(_layers.Last().Neurons[i].Output);
            }
            return answer;
        }

        private void ForwardPass(List<double> test)
        {
            for (int i = 0; i < Inputs; i++)
            {
                _layers[0].Neurons[i].Output = test[i];
            }

            for (int i = 1; i < Depth; i++)
            {
                for (int j = 0; j < _layers[i].Size; j++)
                {
                    var arg = 0.0;
                    for (int k = 0; k < _layers[i].Neurons[j].Inputs; k++)
                    {
                        arg += (_layers[i - 1].Neurons[k].Output * _layers[i].Neurons[j].Weights[k]);
                    }
                    _layers[i].Neurons[j].Output = 1.0 / (1.0 + Math.Exp(-1.0 * arg));
                }
            }
        }

        private void BackwardPass(List<double> testAnswer)
        {
            _testError = 0;
            for (int i = 0; i < Outputs; i++)
            {
                var currOut = _layers.Last().Neurons[i].Output;
                _testError += (testAnswer[i] - currOut) * (testAnswer[i] - currOut);
                _layers.Last().Neurons[i].Delta = (testAnswer[i] - currOut) * currOut * (1.0 - currOut);
                for (int j = 0; j < _layers.Last().Neurons[i].Inputs; j++)
                {
                    _layers.Last().Neurons[i].DeltaWeights[j] = Momentum * _layers.Last().Neurons[i].DeltaWeights[j] +
                                                                LearningSpeed * _layers.Last().Neurons[i].Delta *
                                                                _layers[Depth - 2].Neurons[j].Output;
                }
            }
            _testError /= 2;

            for (int i = Depth - 2; i >= 0; i--)
            {
                for (int j = 0; j < _layers[i].Size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < _layers[i + 1].Size; k++)
                    {
                        sum += _layers[i + 1].Neurons[k].Delta * _layers[i + 1].Neurons[k].Weights[j];
                    }
                    _layers[i].Neurons[j].Delta = sum * _layers[i].Neurons[j].Output * (1.0 - _layers[i].Neurons[j].Output);
                    for (int k = 0; k < _layers[i].Neurons[j].Inputs; k++)
                    {
                        _layers[i].Neurons[j].DeltaWeights[k] = Momentum * _layers[i].Neurons[j].DeltaWeights[k] +
                                                                LearningSpeed * _layers[i].Neurons[j].Delta *
                                                                _layers[i - 1].Neurons[k].Output;
                    }
                }
            }

            for (int i = 0; i < Depth; i++)
            {
                for (int j = 0; j < _layers[i].Size; j++)
                {
                    for (int k = 0; k < _layers[i].Neurons[j].Inputs; k++)
                    {
                        _layers[i].Neurons[j].Weights[k] += _layers[i].Neurons[j].DeltaWeights[k];
                    }
                }
            }
        }
    }
}
