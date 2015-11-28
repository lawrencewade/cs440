using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class DataSet
    {
        Dictionary<string, List<AttributeValue>> _Values = new Dictionary<string, List<AttributeValue>>();
        Dictionary<string, AttributeType> _Types = new Dictionary<string, AttributeType>();

        public List<AttributeValue> this[string Key] { get { return _Values[Key]; } }
        public AttributeType Type(string Attribute) { return _Types[Attribute]; }
        public int AttributeCount { get { return _Values.Count; } }

        public string RandomAttribute(Random Random) { return _Values.Keys.ToList()[Random.Next(0, _Values.Count)]; }

        public void AddAttribute(string Name, AttributeType Type)
        {
            _Values.Add(Name, new List<AttributeValue>());
            _Types.Add(Name, Type);
        }

        public void AddEntry(Dictionary<string, AttributeValue> Entry)
        {
            foreach (KeyValuePair<string, AttributeValue> E in Entry)
            {
                _Values[E.Key].Add(E.Value);
            }
        }

        public DataSet Subset(string Attribute, AttributeValue Value, bool Remove=false)
        {
            Dictionary<string, List<AttributeValue>> Values = new Dictionary<string,List<AttributeValue>>();
            Dictionary<string, AttributeType> Types = new Dictionary<string,AttributeType>();
            for(int i=0;i < _Values[Attribute].Count;++i)
            {
                AttributeValue V = _Values[Attribute][i];
                if (V.CompareTo(Value) == 0)
                {
                    foreach (string S in _Values.Keys)
                    {
                        if (S != Attribute || !Remove)
                        {
                            if (!Values.ContainsKey(S)) { Values.Add(S, new List<AttributeValue>()); Types.Add(S, _Types[S]); }
                            Values[S].Add(_Values[S][i]);
                        }
                    }
                }
            }
            return new DataSet() { _Values = Values, _Types = Types };
        }

        private Dictionary<AttributeValue, double> Proportions(string Attribute)
        {
            Dictionary<AttributeValue, double> P = new Dictionary<AttributeValue, double>();
            foreach (AttributeValue V in _Values[Attribute])
            {
                bool Found = false;
                foreach (AttributeValue Key in P.Keys.ToList())
                {
                    if (Key.CompareTo(V) == 0)
                    {
                        P[Key] += 1d / _Values[Attribute].Count;
                        Found = true;
                    }
                }
                if (!Found)
                {
                    P.Add(V, 1d / _Values[Attribute].Count);
                }
            }
            return P;
        }

        public string BestGain(string Target)
        {
            double G = 0;
            string A = "";
            foreach (string S in _Values.Keys)
            {
                if (S != Target)
                {
                    double g = Gain(Target, S);
                    if (g > G || A == "")
                    {
                        G = g;
                        A = S;
                    }
                }
            }
            return A;
        }

        public double Gain(string Target, string Removal)
        {
            double G = Entropy(Target);
            Dictionary<AttributeValue, double> P = Proportions(Removal);
            foreach (KeyValuePair<AttributeValue, double> p in P)
            {
                DataSet D = Subset(Removal, p.Key);
                G -= p.Value * D.Entropy(Target);
            }
            return G;
        }

        public double Entropy(string Target)
        {
            AttributeType TargetType = _Types[Target];


            double E = 0;
            foreach(KeyValuePair<AttributeValue, double> P in Proportions(Target))
            {
                E -= P.Value * Math.Log(P.Value) / Math.Log(2);
            }
            return E;
        }

        public AttributeValue MostCommonValue(string Target)
        {
            double m = 0;
            AttributeValue R = null;
            Dictionary<AttributeValue, double> P = Proportions(Target);
            foreach (KeyValuePair<AttributeValue, double> p in P)
            {
                if (p.Value > m || R == null)
                {
                    m = p.Value;
                    R = p.Key;
                }
            }
            return R;
        }

        public KeyValuePair<AttributeValue, bool> SingularValue(string Target)
        {
            double m = 0;
            AttributeValue R = null;
            Dictionary<AttributeValue, double> P = Proportions(Target);
            foreach (KeyValuePair<AttributeValue, double> p in P)
            {
                if (p.Value > m || R == null)
                {
                    m = p.Value;
                    R = p.Key;
                }
            }
            return new KeyValuePair<AttributeValue,bool>(R, Math.Abs(m - 1) < .000001);
        }

        public List<AttributeValue> SortedValues(string Attribute)
        {
            List<AttributeValue> R = new List<AttributeValue>(_Values[Attribute]);
            R.Sort(delegate(AttributeValue V1, AttributeValue V2) { return V1.CompareTo(V2); });
            List<AttributeValue> N = new List<AttributeValue>();
            foreach (AttributeValue V in R)
            {
                if (N.Count == 0 || V != N[N.Count - 1]) N.Add(V);
            }
            return N;
        }
    }
}
