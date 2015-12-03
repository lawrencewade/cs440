using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    public class Forest
    {
        List<DecisionTree> _Trees = new List<DecisionTree>();
        List<DataSet> _DataSets = new List<DataSet>();
        int _Target;

        public Forest(int Trees, int DataSize, Func<int, DataSet> DataGenerator, int Target)
        {
            _Target = Target;
            for (int i = 0; i < Trees; ++i)
            {
                DataSet D = DataGenerator.Invoke(DataSize);
                _Trees.Add(new DecisionTree(D, Target));
                _DataSets.Add(D);
            }
        }

        public Forest(int Trees, int DataSize, int Rounds, Func<int, DataSet> DataGenerator, Func<AttributeValue[]> EntryGenerator, Func<AttributeValue[], bool> Validator, int Target)
        {
            _Target = Target;
            for (int i = 0; i < Trees; ++i)
            {
                DataSet D = DataGenerator.Invoke(DataSize);
                _Trees.Add(new DecisionTree(D, Target));
                _DataSets.Add(D);
            }
            for (int i = 0; i < Rounds; ++i)
            {
                Console.WriteLine(i);
                AttributeValue[] E = EntryGenerator.Invoke();
                for (int j = 0; j < _Trees.Count; ++j)
                {
                    AttributeValue Answer = _Trees[j].MakeDecision(E);
                    E[Target] = Answer;
                    if (!Validator.Invoke(E))
                    {
                        Console.WriteLine("RECONSTRUCT TREE {0}", j);
                        _DataSets[j].AddEntry(E);
                        _Trees[j] = new DecisionTree(_DataSets[j], Target);
                    }
                }
            }
        }

        public AttributeValue MakeDecision(AttributeValue[] Data, bool Print = false)
        {
            Dictionary<AttributeValue, int> P = new Dictionary<AttributeValue, int>();
            foreach (DecisionTree D in _Trees)
            {
                AttributeValue V = D.MakeDecision(Data, Print);
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

        public void AddEntry(AttributeValue[] Entry)
        {
            foreach (DataSet D in _DataSets) D.AddEntry(Entry);
            for (int i = 0; i < _DataSets.Count; ++i) _Trees[i] = new DecisionTree(_DataSets[i], _Target);
        }

        public override string ToString()
        {
            string R = "";
            foreach (DecisionTree D in _Trees)
            {
                R += D.ToString() + '\n';
            }
            return R;
        }
    }
}
