using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class DoubleValue : AttributeValue
    {
        bool _Value;

        public DoubleValue(bool Value) { _Value = Value; }

        public int CompareTo(AttributeValue Value)
        {
            return _Value.CompareTo(((DoubleValue)Value)._Value);
        }

        public override string ToString()
        {
            return _Value.ToString();
        }
    }
}