using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class IntegerValue : AttributeValue
    {
        int _Value;

        public IntegerValue(int Value) { _Value = Value; }

        public int CompareTo(AttributeValue Value)
        {
            return _Value.CompareTo(((IntegerValue)Value)._Value);
        }

        public AttributeValue Add(AttributeValue Value)
        {
            return new IntegerValue(_Value + ((IntegerValue)Value)._Value);
        }

        public AttributeValue Subtract(AttributeValue Value)
        {
            return new IntegerValue(_Value - ((IntegerValue)Value)._Value);
        }

        public override string ToString()
        {
            return _Value.ToString();
        }
    }
}
