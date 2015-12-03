using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class Deck : IEnumerable<Card>
    {
        List<Card> _Cards = new List<Card>();

        public int Count { get { return _Cards.Count; } }

        public IEnumerator<Card> GetEnumerator() { return _Cards.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public Deck(bool Empty = false)
        {
            if (!Empty)
            {
                for (int i = 1; i < 5; ++i)
                {
                    for (int j = 1; j < 14; ++j)
                    {
                        _Cards.Add(new Card(j, i));
                    }
                }
            }
        }

        public void Shuffle(Random Random)
        {
            for(int i=0; i<_Cards.Count;++i)
            {
                int j = Random.Next(0,_Cards.Count);
                Card temp = _Cards[j];
                _Cards[j] = _Cards[i];
                _Cards[i] = temp;
            }
        }

        public Card Draw()
        {
            Card C = _Cards[0];
            _Cards.RemoveAt(0);
            return C;
        }

        public void Return(Card Card)
        {
            _Cards.Add(Card);
        }

        public override string ToString()
        {
            string r = "";
            for (int i = 0; i < _Cards.Count; ++i)
            {
                r += i + ". " + _Cards[i].ToString() + '\n';
            }
            return r;
        }
    }
}
