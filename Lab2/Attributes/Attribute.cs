using System;
using System.Collections;

namespace Lab2.Attributes
{
    public class Attribute
    {
        ArrayList _mValues;
        readonly string _mName;
        readonly object _mLabel;
        public Type Type { get; set; }
        public double InfoGain { get; set; }

        public Attribute(string name, string[] values, Type type)
        {
            _mName = name;
            Type = type;

            _mValues = new ArrayList(values);
            _mValues.Sort();
        }

        public Attribute(object label)
        {
            _mLabel = label;
            _mName = string.Empty;
            _mValues = null;
        }

        public string AttributeName
        {
            get { return _mName; }
        }

        public string[] Values
        {
            get
            {
                if (_mValues != null)
                    return (string[])_mValues.ToArray(typeof(string));
                return null;
            }
            set
            {
                _mValues = new ArrayList(value);
            }
        }

        public bool IsValidValue(string value)
        {
            return IndexValue(value) >= 0;
        }

        public int IndexValue(string value)
        {
            if (_mValues != null)
                return _mValues.BinarySearch(value);
            return -1;
        }


        public override string ToString()
        {
            return _mName != string.Empty ? string.Format("{0} (gain: {1})", _mName, InfoGain) : _mLabel.ToString();
        }
    }
}
