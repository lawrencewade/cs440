using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class Discriminator
    {
        Func<AttributeValue[], AttributeValue> _Function;
        int _Index;
        int _Comp;

        public Func<AttributeValue[], AttributeValue> Function { get { return _Function; } }

        public Discriminator(int Index)
        {
            _Index = Index;
            _Function = delegate(AttributeValue[] E) { return E[_Index]; };
        }

        public Discriminator(int Index, int Comp)
        {
            _Index = Index;
            _Comp = Comp;
            _Function = delegate(AttributeValue[] E) { return new IntegerValue(E[_Index].CompareTo(E[_Comp])); };
        }

        public override string ToString()
        {
            return _Index.ToString();
        }
    }
}
