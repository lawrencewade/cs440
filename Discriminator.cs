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
        int _Comp = -1;
        int _FunctionNumber = -1;

        public Func<AttributeValue[], AttributeValue> Function { get { return _Function; } }

        public Discriminator(int Index)
        {
            _Index = Index;
            _Function = delegate(AttributeValue[] E) { return E[_Index]; };
        }

        public Discriminator(int Index, int Comp, int Function)
        {
            _Index = Index;
            _Comp = Comp;
            _FunctionNumber = Function;
            if(Function == 0) _Function = delegate(AttributeValue[] E) { return new IntegerValue(E[_Index].CompareTo(E[_Comp])); };
            else if (Function == 1) _Function = delegate(AttributeValue[] E) { return E[_Index].Add(E[_Comp]); };
            else _Function = delegate(AttributeValue[] E) { return E[_Index].Subtract(E[_Comp]); };
        }

        public override string ToString()
        {
            if (_Comp == -1) return "VALUE OF " + _Index.ToString();
            else
            {
                if (_FunctionNumber == 0) return _Index.ToString() + " COMPARED TO " + _Comp.ToString();
                else if (_FunctionNumber == 1) return _Index.ToString() + " + " + _Comp.ToString();
                else return _Index.ToString() + " - " + _Comp.ToString();
            }
        }
    }
}
