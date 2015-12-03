using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class Card
    {
        int _Number;
        int _Suit;

        public int Number { get { return _Number; } }
        public int Suit { get { return _Suit; } }

        public Card(int Number, int Suit)
        {
            _Number = Number;
            _Suit = Suit; 
        }

        public override string ToString()
        {
            string n = "";
            switch (_Number)
            {
                case 11: n = "Jack"; break;
                case 12: n = "Queen"; break;
                case 13: n = "King"; break;
                case 1: n = "Ace"; break;
                default: n = (_Number).ToString(); break;
            }
            string s = "";
            switch (_Suit)
            {
                case 1: s = "Spades"; break;
                case 2: s = "Hearts"; break;
                case 3: s = "Clubs"; break;
                case 4: s = "Diamonds"; break;
            }
            return n + " of " + s;
        }
    }
}
