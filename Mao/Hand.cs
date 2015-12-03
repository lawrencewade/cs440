using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class Hand : IEnumerable<Card>
    {
        List<Card> _Cards = new List<Card>();

        public int Count { get { return _Cards.Count; } }

        public Card this[int Index] { get { return _Cards[Index]; } }

        public IEnumerator<Card> GetEnumerator() { return _Cards.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public Hand(int Cards, Deck Deck)
        {
            for (int i = 0; i < Cards; ++i) _Cards.Add(Deck.Draw());
        }

        public void Draw(Deck Deck)
        {
            _Cards.Add(Deck.Draw());
        }
        
        public void Play(Card Card, Deck Return)
        {
            _Cards.Remove(Card);
            Return.Return(Card);
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
