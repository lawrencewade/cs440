using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    public class Discriminator
    {
        Func<AttributeValue[], AttributeValue> _Function;
        int _Index;
        int _Comp = -1;
        int _FunctionNumber = -1;
        AttributeValue _Split;
        bool _Equality;

        public Func<AttributeValue[], AttributeValue> Function { get { return _Function; } }

        public Discriminator(int Index, AttributeValue Split, bool Equality)
        {
            _Index = Index;
            _Split = Split;
            _Equality = Equality;
            if (Split == null) _Function = delegate(AttributeValue[] E) { return E[_Index]; };
            else _Function = delegate(AttributeValue[] E) { return new BooleanValue(E[_Index].CompareTo(Split) == (Equality ? 0: -1));};
        }

        public Discriminator(int Index, int Comp, int Function, AttributeValue Split, bool Equality)
        {
            _Index = Index;
            _Comp = Comp;
            _Split = Split;
            _Equality = Equality;
            _FunctionNumber = Function;
            if (Split == null)
            {
                if (Function == 0) _Function = delegate(AttributeValue[] E) { return new IntegerValue(E[_Index].CompareTo(E[_Comp])); };
                else if (Function == 1) _Function = delegate(AttributeValue[] E) { return E[_Index].Add(E[_Comp]); };
                else _Function = delegate(AttributeValue[] E) { return E[_Index].Subtract(E[_Comp]); };
            }
            else
            {
                if (Function == 0) _Function = delegate(AttributeValue[] E) { return new BooleanValue(new IntegerValue(E[_Index].CompareTo(E[_Comp])).CompareTo(Split) == (Equality ? 0 : -1)); };
                else if (Function == 1) _Function = delegate(AttributeValue[] E) { return new BooleanValue(E[_Index].Add(E[_Comp]).CompareTo(Split)  == (Equality ? 0: -1)); };
                else _Function = delegate(AttributeValue[] E) { return new BooleanValue(E[_Index].Subtract(E[_Comp]).CompareTo(Split) == (Equality ? 0: -1)); };
            }
        }

        public override string ToString()
        {
            if (_Comp == -1) return "[Discriminate VALUE OF " + _Index.ToString() + (_Split != null ? ((_Equality ? " == " : " < ") + _Split.ToString()) : "") + "]";
            else
            {
                if (_FunctionNumber == 0) return "[Discriminate VALUE OF " + _Index.ToString() + " COMPARED TO " + _Comp.ToString() + (_Split != null ? ((_Equality ? " == " : " < ") + _Split.ToString()) : "" ) + "]";
                else if (_FunctionNumber == 1) return "[Discriminate VALUE OF " + _Index.ToString() + " + " + _Comp.ToString() + (_Split != null ? ((_Equality ? " == " : " < ") + _Split.ToString()) : "") + "]";
                else return "[Discriminate VALUE OF " + _Index.ToString() + " - " + _Comp.ToString() + (_Split != null ? ((_Equality ? " == " : " < ") + _Split.ToString()) : "") + "]";
            }
        }
    }
}
