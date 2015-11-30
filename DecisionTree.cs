using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class DecisionTree
    {
        AttributeValue _Value;
        Pair<int, Discriminator> _Check;
        List<KeyValuePair<AttributeValue, DecisionTree>> _Children;

        public DecisionTree(DataSet DataSet, int Target)
        {
            if (DataSet.AttributeCount == 0) return;
            else if (DataSet.AttributeCount == 1)
            {
                _Value = DataSet.MostCommonValue(Target);
            }
            else
            {
                KeyValuePair<AttributeValue, bool> S = DataSet.SingularValue(Target);
                if (S.Value) _Value = S.Key;
                else
                {
                    _Check = DataSet.BestGain(Target);
                    _Children = new List<KeyValuePair<AttributeValue, DecisionTree>>();
                    List<AttributeValue> A = DataSet.SortedValues(_Check.Second.Function);
                    foreach (AttributeValue V in A)
                    {
                        DecisionTree C = new DecisionTree(DataSet.Subset(_Check.Second.Function, V, _Check.First), Target - (_Check.First > -1 ? 1 : 0));
                        _Children.Add(new KeyValuePair<AttributeValue, DecisionTree>(V, C));
                    }
                }
            }
        }

        public AttributeValue MakeDecision(AttributeValue[] Data)
        {
            if (_Children == null) return _Value;
            else
            {
                AttributeValue V = _Check.Second.Function.Invoke(Data);
                AttributeValue[] d = RemoveUsed(Data);
                foreach (KeyValuePair<AttributeValue, DecisionTree> P in _Children)
                {
                    if (V.CompareTo(P.Key) <= 0) return P.Value.MakeDecision(d);
                }
                return _Children[_Children.Count - 1].Value.MakeDecision(d);
            }
        }

        private AttributeValue[] RemoveUsed(AttributeValue[] Data)
        {
            if (_Check.First == -1) return Data;

            AttributeValue[] R = new AttributeValue[Data.Length - 1];
            int j = 0;
            for (int i = 0; i < Data.Length; ++i)
            {
                if (i != _Check.First) R[j++] = Data[i];
            }
            return R;
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
