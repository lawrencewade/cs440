using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class CheckerAI : Player
    {
        Deck _ImaginaryDeck = new Deck();
        Func<Card, Card, bool> _Function;

        public CheckerAI(Func<Card, Card, bool> Function)
        {
            _Function = Function;
        }

        public Card MakePlay(Card Down, Hand Hand)
        {

            List<Card> V = new List<Card>();
            foreach (Card Card in Hand)
            {
                if (_Function.Invoke(Down, Card)) V.Add(Card);
            }

            int M = Int32.MaxValue;
            Card Choice = null;
            foreach (Card Card in V)
            {
                int N = 0;
                foreach (Card DownCard in _ImaginaryDeck)
                {
                    if (_Function.Invoke(DownCard, Card)) N++;
                }
                if (N < M)
                {
                    M = N;
                    Choice = Card;
                }
            }
            return Choice;
        }

        public bool ValidatePlay(Card Down, Card Played)
        {
            return _Function.Invoke(Down, Played);
        }

        public void VerifyPlay(Card Down, Card Played, bool Valid)
        {
            return;
        }
    }
}
