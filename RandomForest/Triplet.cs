using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    public class Triplet<T,K,J>
    {
        T _First;
        K _Second;
        J _Third;

        public T First { get { return _First; } set { _First = value; } }
        public K Second { get { return _Second; } set { _Second = value; } }
        public J Third { get { return _Third; } set { _Third = value; } }

        public Triplet(T First, K Second, J Third)
        {
            _First = First;
            _Second = Second;
            _Third = Third;
        }
    }
}
