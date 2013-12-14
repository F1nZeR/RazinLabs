using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2_Correct.Helper;

namespace Lab2_Correct.Core
{
    public class DecisionTree
    {
        private readonly List<Attribute> _samples;
        private readonly string _targetAttrName;
        private double _entropyForSet = 0;

        public DecisionTree(List<Attribute> samples, string targetAttrName)
        {
            _samples = samples;
            _targetAttrName = targetAttrName;
        }

        public TreeNode MountTree()
        {
            if (_samples.All(x => x.Name == _targetAttrName))
            {
                var resAttr = _samples.Single(x => x.Name == _targetAttrName);
                var mostCommon = GetMostCommonValue(_samples);
                return new TreeNode(resAttr, mostCommon);
            }

            if (AllResValuesAreSame(_samples))
            {
                var resAttr = _samples.Single(x => x.Name == _targetAttrName);
                return new TreeNode(resAttr, resAttr.Values.First());
            }

            var dictVals = CountResValuesCount(_samples);
            _entropyForSet = CalcEntropy(dictVals);
            var attr = GetBestAttribute(_samples);
            var root = new TreeNode(attr, "");


            if (attr.IsQuantity)
            {
                var resultAttr = _samples.Single(x => x.Name == _targetAttrName);
                var maxEntropyValue = CalcMaxEntropy(_samples, attr, false).Item1;
                var lessSamples = RemoveAllWhich(0, _samples, attr, maxEntropyValue);
                var dtLess = new DecisionTree(lessSamples, _targetAttrName);

                var curNode = new TreeNode(attr, "<" + maxEntropyValue);
                root.AddChildren(curNode);
                curNode.AddChildren(lessSamples.All(x => x.Values.Count == 0)
                    ? new TreeNode(resultAttr, GetMostCommonValue(_samples))
                    : dtLess.MountTree());

                var geSamples = RemoveAllWhich(1, _samples, attr, maxEntropyValue);
                var dtGe = new DecisionTree(geSamples, _targetAttrName);
                var curNode2 = new TreeNode(attr, ">=" + maxEntropyValue);
                root.AddChildren(curNode2);
                curNode2.AddChildren(geSamples.All(x => x.Values.Count == 0)
                    ? new TreeNode(resultAttr, GetMostCommonValue(_samples))
                    : dtGe.MountTree());
            }
            else
            {
                var distinctVals = attr.Values.Distinct();
                foreach (var distinctVal in distinctVals)
                {
                    var curSamples = RemoveAllWhich(2, _samples, attr, distinctVal);
                    var eqDt = new DecisionTree(curSamples, _targetAttrName);

                    var curNode = new TreeNode(attr, distinctVal);
                    root.AddChildren(curNode);
                    curNode.AddChildren(eqDt.MountTree());
                }
            }

            return root;
        }

        private Tuple<int,double> CalcMaxEntropy(List<Attribute> sample, Attribute curAttr, bool isNeedToLog = true)
        {
            var totalDic = new Dictionary<int, double>();
            for (int i = 0; i < curAttr.Values.Count(); i++)
            {
                var col = sample.Single(x => x.Name == curAttr.Name).Values.ToList();
                var totalCountValues = col.Count;
                var curT = int.Parse(curAttr.Values[i]);
                var lessT = new List<int>();
                for (int j = 0; j < totalCountValues; j++)
                {
                    //if (i == j) continue;
                    var curX = int.Parse(col[j]);
                    if (curX < curT)
                    {
                        lessT.Add(j);
                    }
                }
                var pLess = (double) lessT.Count/totalCountValues;
                var dictValsLess = CalcDiffsOnRes(sample, lessT);
                var lessEntropy = CalcEntropy(dictValsLess);

                var geT = new List<int>();
                for (int j = 0; j < totalCountValues; j++)
                {
                    //if (i == j) continue;
                    var curX = int.Parse(col[j]);
                    if (curX >= curT)
                    {
                        geT.Add(j);
                    }
                }
                var pGe = (double) geT.Count/totalCountValues;
                var dictValsGe = CalcDiffsOnRes(sample, geT);
                var geEntropy = CalcEntropy(dictValsGe);

                var entropyForT = pLess*lessEntropy + pGe*geEntropy;
                var resEntropy = _entropyForSet - entropyForT;
                
                if (!totalDic.ContainsKey(curT))
                {
                    totalDic.Add(curT, resEntropy);
                }
            }

            if (isNeedToLog)
            {
                Logger.AddLogEntry(this, "Циферки: " + curAttr.Name);
                foreach (var d in totalDic)
                {
                    Logger.AddLogEntry(this, d.Key + ": " + d.Value);
                }
            }

            var maxEntropy = totalDic.Max(x => x.Value);
            var best = totalDic.First(x => x.Value.Equals(maxEntropy));
            return Tuple.Create(best.Key, best.Value);
        }

        private Dictionary<string, int> CalcDiffsOnRes(List<Attribute> sample, List<int> indexes)
        {
            var dictVals = new Dictionary<string, int>();
            for (int i = 0; i < sample.Last().Values.Count; i++)
            {
                if (indexes.Contains(i))
                {
                    var resValue = sample.Single(x => x.Name == _targetAttrName).Values[i];
                    if (dictVals.ContainsKey(resValue))
                    {
                        dictVals[resValue]++;
                    }
                    else
                    {
                        dictVals.Add(resValue, 1);
                    }
                }
            }
            return dictVals;
        }

        private Attribute GetBestAttribute(List<Attribute> samples)
        {
            var dic = new Dictionary<Attribute, double>();
            foreach (var attribute in samples.Where(x => x.Name != _targetAttrName))
            {
                dic.Add(attribute, Gain(samples, attribute));
            }

            foreach (var d in dic)
            {
                Logger.AddLogEntry(this, string.Format("{0}: {1}", d.Key.Name, d.Value));
            }

            return dic.First(x => Math.Abs(x.Value - dic.Max(y => y.Value)) < 0.01).Key;
        }

        private double CalcEntropy(Dictionary<string, int> dictVals)
        {
            var total = dictVals.Sum(x => x.Value);
            double result = 0;
            foreach (var dictVal in dictVals)
            {
                var curPart = (double)dictVal.Value / total;
                var curRatio = curPart > 0 ? -curPart * Math.Log(curPart, 2) : 0;
                result += curRatio;
            }

            return result;
        }

        private double Gain(List<Attribute> samples, Attribute attribute)
        {
            // частота этого атрибута
            var dict = new Dictionary<string, int>();
            foreach (var value in attribute.Values)
            {
                if (dict.ContainsKey(value))
                {
                    dict[value]++;
                }
                else
                {
                    dict.Add(value, 1);
                }
            }

            double sum = 0;

            if (attribute.IsQuantity)
            {
                //var cDic = new Dictionary<string, double>();
                //foreach (var val in dict)
                //{
                //    var valsDict = GetValuesToAttribute(samples, attribute, val.Key);
                //    var entropy = CalcEntropy(valsDict);
                //    var valProb = (double)val.Value / dict.Sum(x => x.Value);
                //    var targEntropy = valProb * entropy;
                //    cDic.Add(val.Key, targEntropy);
                //}
                //var best = cDic.Last(x => x.Value.Equals(cDic.Min(y => y.Value)));
                //var temp = attribute.Clone();
                //temp.Values.Clear();
                //temp.Values.Add(best.Key);
                //var entopyGain = CalcMaxEntropy(_samples, temp);
                var maxEntropyValue = CalcMaxEntropy(_samples, attribute);
                return maxEntropyValue.Item2;
            }
            else
            {
                foreach (var val in dict)
                {
                    var valsDict = GetValuesToAttribute(samples, attribute, val.Key);
                    var entropy = CalcEntropy(valsDict);
                    var valProb = (double)val.Value / dict.Sum(x => x.Value);

                    var targEntropy = valProb * entropy;
                    sum += targEntropy > 0 ? targEntropy : 0;
                }
            }
            
            var result = _entropyForSet - sum;
            attribute.InfoGain = result;
            return result;
        }

        private Dictionary<string, int> GetValuesToAttribute(List<Attribute> samples, Attribute attribute, string value)
        {
            var dictVals = new Dictionary<string, int>();

            for (int i = 0; i < attribute.Values.Count; i++)
            {
                if (attribute.Values[i] == value)
                {
                    var resValue = samples.Single(x => x.Name == _targetAttrName).Values[i];
                    if (dictVals.ContainsKey(resValue))
                    {
                        dictVals[resValue]++;
                    }
                    else
                    {
                        dictVals.Add(resValue, 1);
                    }
                }
            }
            return dictVals;
        }

        private Dictionary<string, int> CountResValuesCount(IEnumerable<Attribute> samples)
        {
            return samples.Single(x => x.Name == _targetAttrName)
                .Values.GroupBy(x => x)
                .ToDictionary(x => x.Key, y => y.Count());
        }

        private bool AllResValuesAreSame(IEnumerable<Attribute> samples)
        {
            return CountResValuesCount(samples).Count == 1;
        }

        private string GetMostCommonValue(IEnumerable<Attribute> samples)
        {
            var count = CountResValuesCount(samples);
            return count.First(x => x.Value == count.Max(y => y.Value)).Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0: less, 1: ge, 2: equal</param>
        /// <param name="sample"></param>
        /// <param name="targetAttr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<Attribute> RemoveAllWhich(byte type, List<Attribute> sample, Attribute targetAttr, object value)
        {
            var elems = new List<int>();
            if (type == 0)
            {
                for (int j = 0; j < targetAttr.Values.Count; j++)
                {
                    var curX = int.Parse(targetAttr.Values[j]);
                    if (curX < (int)value)
                    {
                        elems.Add(j);
                    }
                }
            }
            else if (type == 1)
            {
                for (int j = 0; j < targetAttr.Values.Count; j++)
                {
                    var curX = int.Parse(targetAttr.Values[j]);
                    if (curX >= (int) value)
                    {
                        elems.Add(j);
                    }
                }
            }
            else
            {
                for (int j = 0; j < targetAttr.Values.Count; j++)
                {
                    var curX = targetAttr.Values[j];
                    if (curX == (string)value)
                    {
                        elems.Add(j);
                    }
                }
            }

            var tempSample = sample.Clone();
            for (int i = 0; i < tempSample.Count; i++)
            {
                tempSample[i].Values.Clear();
                foreach (var index in elems)
                {
                    tempSample[i].Values.Add(sample[i].Values[index]);
                }
            }
            tempSample.RemoveAll(x => x.Name == targetAttr.Name);
            return tempSample;
        }
    }
}
