﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    public class BooleanValue : AttributeValue
    {
        bool _Value;

        public BooleanValue(bool Value) { _Value = Value; }

        public int CompareTo(AttributeValue Value)
        {
            return _Value.CompareTo(((BooleanValue)Value)._Value);
        }

        public AttributeValue Add(AttributeValue Value)
        {
            return new BooleanValue(_Value ^ ((BooleanValue)Value)._Value);
        }

        public AttributeValue Subtract(AttributeValue Value)
        {
            return new BooleanValue(_Value ^ ((BooleanValue)Value)._Value);
        }

        public override string ToString()
        {
            return _Value.ToString();
        }
    }
}
