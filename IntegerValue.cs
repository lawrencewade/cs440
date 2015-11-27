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

        public override string ToString()
        {
            return _Value.ToString();
        }
    }
}
