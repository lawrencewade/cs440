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

        public T First { get { return _First; } }
        public K Second { get { return _Second; } }
        public J Third { get { return _Third; } }

        public Triplet(T First, K Second, J Third)
        {
            _First = First;
            _Second = Second;
            _Third = Third;
        }
    }
}
