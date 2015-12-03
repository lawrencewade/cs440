using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    public interface AttributeValue
    {
        int CompareTo(AttributeValue Value);
        AttributeValue Add(AttributeValue Value);
        AttributeValue Subtract(AttributeValue Value);
    }
}
