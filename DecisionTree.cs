using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class DecisionTree
    {
        AttributeValue _Value;
        string _Check;
        List<KeyValuePair<AttributeValue, DecisionTree>> _Children;

        public DecisionTree(DataSet DataSet, string Target)
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
                    List<AttributeValue> A = DataSet.SortedValues(_Check);
                    foreach (AttributeValue V in A)
                    {
                        _Children.Add(new KeyValuePair<AttributeValue, DecisionTree>(V, new DecisionTree(DataSet.Subset(_Check, V, true), Target)));
                    }
                }
            }
        }

        public AttributeValue MakeDecision(Dictionary<string, AttributeValue> Data)
        {
            if (_Children == null) return _Value;
            else
            {
                AttributeValue V = Data[_Check];
                foreach (KeyValuePair<AttributeValue, DecisionTree> P in _Children)
                {
                    if (V.CompareTo(P.Key) <= 0) return P.Value.MakeDecision(Data);
                }
                return _Children[_Children.Count - 1].Value.MakeDecision(Data);
            }
        }
    }
}
