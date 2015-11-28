using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class Forest
    {
        List<DecisionTree> _Trees = new List<DecisionTree>();

        public Forest(int Trees, int DataSize, Func<int, DataSet> DataGenerator, string Target)
        {
            for (int i = 0; i < Trees; ++i)
            {
                _Trees.Add(new DecisionTree(DataGenerator.Invoke(DataSize), Target));
            }
        }

        public AttributeValue MakeDecision(Dictionary<string, AttributeValue> Data)
        {
            Dictionary<AttributeValue, int> P = new Dictionary<AttributeValue, int>();
            foreach (DecisionTree D in _Trees)
            {
                AttributeValue V = D.MakeDecision(Data);
                bool Found = false;
                foreach (AttributeValue Key in P.Keys.ToList())
                {
                    if (Key.CompareTo(V) == 0)
                    {
                        P[Key]++;
                        Found = true;
                    }
                }
                if (!Found)
                {
                    P.Add(V, 1);
                }
            }
            AttributeValue M = null;
            int N = 0;
            foreach(KeyValuePair<AttributeValue, int> p in P)
            {
                if (p.Value > N)
                {
                    N = p.Value;
                    M = p.Key;
                }
            }
            return M;
        }
    }
}
