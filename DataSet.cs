using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class DataSet
    {
        List<AttributeValue>[] _Values;

        public List<AttributeValue> this[int Key] { get { return _Values[Key]; } }
        public int Count { get { return _Values[0].Count; } }

        private DataSet() { }

        public DataSet(int Columns)
        {
            _Values = new List<AttributeValue>[Columns];
            for (int i = 0; i < Columns; ++i) _Values[i] = new List<AttributeValue>();
        }

        public void AddEntry(AttributeValue[] Entry)
        {
            for (int i = 0; i < Entry.Length;++i)
            {
                _Values[i].Add(Entry[i]);
            }
        }

        public DataSet Subset(Func<AttributeValue[], AttributeValue> Discriminator, AttributeValue Match)
        {
            List<AttributeValue>[] Values = new List<AttributeValue>[_Values.Length];
            for (int i = 0; i < _Values.Length; ++i) Values[i] = new List<AttributeValue>();
            AttributeValue[] Current = new AttributeValue[_Values.Length];
            for(int i=0;i < _Values[0].Count;++i)
            {
                for (int j = 0; j < _Values.Length; ++j)
                {
                    Current[j] = _Values[j][i];
                }
                if (Discriminator.Invoke(Current).CompareTo(Match) == 0)
                {
                    for (int j = 0; j < _Values.Length; ++j)
                    {
                        Values[j].Add(Current[j]);
                    }
                }
            }
            return new DataSet() { _Values = Values };
        }

        private List<Pair<AttributeValue, double>> Proportions(Func<AttributeValue[], AttributeValue> Discriminator)
        {
            List<Pair<AttributeValue, double>> P = new List<Pair<AttributeValue, double>>();
            List<AttributeValue> V = new List<AttributeValue>();
            AttributeValue[] Current = new AttributeValue[_Values.Length];
            for (int i = 0; i < _Values[0].Count; ++i)
            {
                for (int j = 0; j < _Values.Length; ++j)
                {
                    Current[j] = _Values[j][i];
                }
                V.Add(Discriminator.Invoke(Current));
            }
            V.Sort(delegate(AttributeValue V1, AttributeValue V2) { return V1.CompareTo(V2); });
            AttributeValue C = null;
            foreach (AttributeValue v in V)
            {
                if (C== null || C.CompareTo(v) != 0)
                {
                    C = v;
                    P.Add(new Pair<AttributeValue, double>(v, 1d / V.Count));
                }
                else P[P.Count - 1].Second += 1d / V.Count;
            }
            return P;
        }

        public Discriminator BestGain(int Target)
        {
            double G = 0;
            int I = -1;
            int C = -1;
            int F = -1;
            for (int i = 0; i < _Values.Length - 1; ++i)
            {
                for (int j = i + 1; j < _Values.Length; ++j)
                {
                    if (i != Target && j != Target)
                    {
                        Func<AttributeValue[], AttributeValue> S = delegate(AttributeValue[] E) { return new IntegerValue(E[i].CompareTo(E[j])); };
                        double g = Gain(Target, S);
                        if (g > G)
                        {
                            G = g;
                            I = i;
                            C = j;
                            F = 0;
                        }
                        S = delegate(AttributeValue[] E) { return E[i].Add(E[j]); };
                        g = Gain(Target, S);
                        if (g > G)
                        {
                            G = g;
                            I = i;
                            C = j;
                            F = 1;
                        }
                        S = delegate(AttributeValue[] E) { return E[i].Subtract(E[j]); };
                        g = Gain(Target, S);
                        if (g > G)
                        {
                            G = g;
                            I = i;
                            C = j;
                            F = 2;
                        }
                    }
                }
            }
            for(int i=0; i<_Values.Length; ++i)
            {
                if (i != Target)
                {
                    Func<AttributeValue[], AttributeValue> S = delegate(AttributeValue[] E) { return E[i]; };
                    double g = Gain(Target, S);
                    if (g > G)
                    {
                        G = g;
                        I = i;
                        C = -1;
                    }
                }
            }
            Console.WriteLine(G);
            if (Math.Abs(G) < .00001) return null;
            if (C == -1) return new Discriminator(I);
            else return new Discriminator(I, C, F);
        }

        public double Gain(int Target, Func<AttributeValue[], AttributeValue> Discriminator)
        {
            double G = Entropy(Target);
            List<Pair<AttributeValue, double>> P = Proportions(Discriminator);
            foreach (Pair<AttributeValue, double> p in P)
            {
                DataSet D = Subset(Discriminator, p.First);
                G -= p.Second * D.Entropy(Target);
            }
            return G / Math.Log(P.Count + 1);
        }

        public double Entropy(int Target)
        {
            double E = 0;
            foreach(Pair<AttributeValue, double> P in Proportions(delegate(AttributeValue[] e) { return e[Target]; }))
            {
                E -= P.Second * Math.Log(P.Second);
            }
            return E;
        }

        public AttributeValue MostCommonValue(int Target)
        {
            double m = 0;
            AttributeValue R = null;
            List<Pair<AttributeValue, double>> P = Proportions(delegate(AttributeValue[] E) { return E[Target]; });
            foreach (Pair<AttributeValue, double> p in P)
            {
                if (p.Second > m || R == null)
                {
                    m = p.Second;
                    R = p.First;
                }
            }
            return R;
        }

        public KeyValuePair<AttributeValue, bool> SingularValue(int Target)
        {
            AttributeValue A = _Values[Target][0];
            foreach (AttributeValue v in _Values[Target])
            {
                if (A.CompareTo(v) != 0) return new KeyValuePair<AttributeValue,bool>(null, false);
            }
            return new KeyValuePair<AttributeValue,bool>(A, true);
        }

        public List<AttributeValue> SortedValues(Func<AttributeValue[], AttributeValue> Discriminator)
        {
            List<AttributeValue> R = new List<AttributeValue>();
            AttributeValue[] Current = new AttributeValue[_Values.Length];
            for (int i = 0; i < _Values[0].Count; ++i)
            {
                for (int j = 0; j < _Values.Length; ++j)
                {
                    Current[j] = _Values[j][i];
                }
                R.Add(Discriminator.Invoke(Current));
            }
            R.Sort(delegate(AttributeValue V1, AttributeValue V2) { return V1.CompareTo(V2); });
            List<AttributeValue> N = new List<AttributeValue>();
            foreach (AttributeValue V in R)
            {
                if (N.Count == 0 || V.CompareTo(N[N.Count - 1]) != 0) N.Add(V);
            }
            return N;
        }
    }
}
