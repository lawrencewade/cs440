using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class DecisionTree
    {
        AttributeValue _Value;
        Discriminator _Check;
        List<KeyValuePair<AttributeValue, DecisionTree>> _Children;

        public DecisionTree(DataSet DataSet, int Target)
        {
            KeyValuePair<AttributeValue, bool> S = DataSet.SingularValue(Target);
            if (S.Value) _Value = S.Key;
            else
            {
                _Check = DataSet.BestGain(Target);
                if (_Check == null)
                {
                    _Value = DataSet.MostCommonValue(Target);
                    return;
                }
                _Children = new List<KeyValuePair<AttributeValue, DecisionTree>>();
                List<AttributeValue> A = DataSet.SortedValues(_Check.Function);
                foreach (AttributeValue V in A)
                {
                    DecisionTree C = new DecisionTree(DataSet.Subset(_Check.Function, V), Target);
                    _Children.Add(new KeyValuePair<AttributeValue, DecisionTree>(V, C));
                }
            }
        }

        public AttributeValue MakeDecision(AttributeValue[] Data, bool Print = false)
        {
            if (_Children == null)
            {
                if (Print) Console.WriteLine(_Value);
                return _Value;
            }
            else
            {
                AttributeValue V = _Check.Function.Invoke(Data);
                if (Print) Console.WriteLine("{0} {1}", V, _Check);
                foreach (KeyValuePair<AttributeValue, DecisionTree> P in _Children)
                {
                    if (V.CompareTo(P.Key) <= 0) return P.Value.MakeDecision(Data, Print);
                }
                return _Children[_Children.Count - 1].Value.MakeDecision(Data, Print);
            }
        }

        private string ToStringAux(int Depth)
        {
            if (_Children != null)
            {
                string R = "".PadLeft(Depth, '\t') + _Check + '\n';
                foreach (KeyValuePair<AttributeValue, DecisionTree> p in _Children)
                {
                    R += "".PadLeft(Depth + 1, '\t') + "--> " + p.Key.ToString() + '\n' + p.Value.ToStringAux(Depth + 1);
                }
                return R + '\n';
            }
            else return "".PadLeft(Depth, '\t') + _Value.ToString() + '\n';
        }

        public override string ToString()
        {
            return ToStringAux(0);
        }
    }
}
