using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    public class DoubleValue : AttributeValue
    {
        double _Value;

        public DoubleValue(double Value) { _Value = Value; }

        public int CompareTo(AttributeValue Value)
        {
            return _Value.CompareTo(((DoubleValue)Value)._Value);
        }

        public AttributeValue Add(AttributeValue Value)
        {
            return new DoubleValue(_Value + ((DoubleValue)Value)._Value);
        }

        public AttributeValue Subtract(AttributeValue Value)
        {
            return new DoubleValue(_Value - ((DoubleValue)Value)._Value);
        }

        public override string ToString()
        {
            return _Value.ToString();
        }
    }
}