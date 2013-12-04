using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Attributes
{
    public class Attribute
    {
        readonly ArrayList _mValues;
        readonly string _mName;
        readonly object _mLabel;
        public Type Type { get; set; }

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
            get
            {
                return _mName;
            }
        }

        public string[] Values
        {
            get
            {
                if (_mValues != null)
                    return (string[])_mValues.ToArray(typeof(string));
                return null;
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
            return _mName != string.Empty ? _mName : _mLabel.ToString();
        }
    }
}
