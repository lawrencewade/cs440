﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    interface AttributeValue
    {
        int CompareTo(AttributeValue Value);
    }
}
