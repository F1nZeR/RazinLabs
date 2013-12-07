using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Lab2.Attributes
{
    public class DecisionTreeId3
    {
        private DataTable _mSamples;
        private int _mTotal;
        private string _mTargetAttribute;
        private double _mEntropySet;

        private Dictionary<string, int> CountResValuesCount(DataTable samples)
        {
            var dictVals = new Dictionary<string, int>();
            foreach (DataRow aRow in samples.Rows)
            {
                if (dictVals.ContainsKey(aRow[_mTargetAttribute].ToString()))
                {
                    dictVals[aRow[_mTargetAttribute].ToString()]++;
                }
                else
                {
                    dictVals.Add(aRow[_mTargetAttribute].ToString(), 1);
                }
            }
            return dictVals;
        }

        public double CalcEntropy(Dictionary<string,int> dictVals)
        {
            int total = dictVals.Sum(x => x.Value);
            double result = 0;
            foreach (var dictVal in dictVals)
            {
                var curPart = (double)dictVal.Value / total;
                var curRatio = curPart > 0 ? -curPart*Math.Log(curPart, 2) : 0;
                result += curRatio;
            }

            return result;
        }

        private Dictionary<string, int> GetValuesToAttribute(DataTable samples, Attribute attribute, string value)
        {
            var dictVals = new Dictionary<string, int>();

            foreach (DataRow aRow in samples.Rows)
            {
                if ((aRow[attribute.AttributeName].ToString() == value))
                {
                    if (dictVals.ContainsKey(aRow[_mTargetAttribute].ToString()))
                    {
                        dictVals[aRow[_mTargetAttribute].ToString()]++;
                    }
                    else
                    {
                        dictVals.Add(aRow[_mTargetAttribute].ToString(), 1);
                    }
                }
            }
            return dictVals;
        }

        private double Gain(DataTable samples, Attribute attribute)
        {
            string[] values = attribute.Values;
            double sum = 0.0;

            foreach (string val in values)
            {
                var valsDict = GetValuesToAttribute(samples, attribute, val);

                double entropy = CalcEntropy(valsDict);
                sum += -(double)(valsDict.Sum(x => x.Value)) / _mTotal * entropy;
            }
            var result = _mEntropySet + sum;
            Logger.AddLogEntry(this, attribute.AttributeName + ": " + result);
            attribute.InfoGain = result;
            return result;
        }

        private Attribute GetBestAttribute(DataTable samples, IEnumerable<Attribute> attributes)
        {
            double maxGain = 0.0;
            Attribute result = null;

            foreach (var attribute in attributes)
            {
                var aux = Gain(samples, attribute);
                if (aux > maxGain)
                {
                    maxGain = aux;
                    result = attribute;
                }
            }
            return result;
        }

        public bool AllSamplesAreSame(DataTable samples, string targetAttribute)
        {
            var first = samples.Rows.Cast<DataRow>().First();
            return samples.Rows.Cast<DataRow>().All(row => first == row);
        }

        public ArrayList GetDistinctValues(DataTable samples, string targetAttribute)
        {
            var distinctValues = new ArrayList(samples.Rows.Count);

            foreach (DataRow row in samples.Rows)
            {
                if (distinctValues.IndexOf(row[targetAttribute]) == -1)
                    distinctValues.Add(row[targetAttribute]);
            }

            return distinctValues;
        }

        private object GetMostCommonValue(DataTable samples, string targetAttribute)
        {
            var distinctValues = GetDistinctValues(samples, targetAttribute);
            var count = new int[distinctValues.Count];

            foreach (DataRow row in samples.Rows)
            {
                int index = distinctValues.IndexOf(row[targetAttribute]);
                count[index]++;
            }

            int maxIndex = 0;
            int maxCount = 0;
            
            for (int i = 0; i < count.Length; i++)
            {
                if (count[i] > maxCount)
                {
                    maxCount = count[i];
                    maxIndex = i;
                }
            }

            return distinctValues[maxIndex];
        }

        private TreeNode InternalMountTree(DataTable samples, string targetAttribute, Attribute[] attributes)
        {
            if (AllSamplesAreSame(samples, targetAttribute))
            {
                var item = samples.Rows[0][targetAttribute];
                return new TreeNode(new Attribute(item));
            }

            if (attributes.Length == 0)
                return new TreeNode(new Attribute(GetMostCommonValue(samples, targetAttribute)));

            _mTotal = samples.Rows.Count;
            _mTargetAttribute = targetAttribute;
            
            var dictVals = CountResValuesCount(samples);
            _mEntropySet = CalcEntropy(dictVals);

            var bestAttribute = GetBestAttribute(samples, attributes) ?? attributes[0];
            var root = new TreeNode(bestAttribute);
            
            var aSample = samples.Clone();

            foreach (var value in bestAttribute.Values)
            {
                aSample.Rows.Clear();

                if (bestAttribute.Type == typeof (int))
                {
                    // имеем дело с циферками
                    for (int i = 0; i < 2; i++)
                    {
                        var sign = i == 0 ? "<=" : ">";
                        var rows = samples.Select(bestAttribute.AttributeName + " " + sign + " " + value);
                        foreach (var row in rows)
                        {
                            aSample.Rows.Add(row.ItemArray);
                        }

                        var aAttributes = new ArrayList(attributes.Length - 1);
                        foreach (var attr in attributes.Where(attr => attr.AttributeName != bestAttribute.AttributeName))
                        {
                            aAttributes.Add(attr);
                        }

                        if (aSample.Rows.Count == 0)
                        {
                            return new TreeNode(new Attribute(GetMostCommonValue(samples, targetAttribute)));
                        }

                        var dc3 = new DecisionTreeId3();
                        var childNode = dc3.MountTree(aSample, targetAttribute, (Attribute[])aAttributes.ToArray(typeof(Attribute)));

                        root.AddTreeNode(childNode, value, sign);
                    }
                }
                else
                {
                    var rows = samples.Select(bestAttribute.AttributeName + " = " + "'" + value + "'");
                    foreach (var row in rows)
                    {
                        aSample.Rows.Add(row.ItemArray);
                    }

                    var aAttributes = new ArrayList(attributes.Length - 1);
                    foreach (var attr in attributes.Where(attr => attr.AttributeName != bestAttribute.AttributeName))
                    {
                        aAttributes.Add(attr);
                    }

                    if (aSample.Rows.Count == 0)
                    {
                        return new TreeNode(new Attribute(GetMostCommonValue(samples, targetAttribute)));
                    }

                    var dc3 = new DecisionTreeId3();
                    var childNode = dc3.MountTree(aSample, targetAttribute, (Attribute[])aAttributes.ToArray(typeof(Attribute)));
                    root.AddTreeNode(childNode, value);
                }
            }

            return root;
        }

        public TreeNode MountTree(DataTable samples, string targetAttribute, Attribute[] attributes)
        {
            _mSamples = samples;
            return InternalMountTree(_mSamples, targetAttribute, attributes);
        }
    }
}
