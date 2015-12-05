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
            string s = "";
            switch (_Number)
            {
                case 11: s += "J"; break;
                case 12: s += "Q"; break;
                case 13: s += "K"; break;
                case 1: s += "A"; break;
                default: s += (_Number).ToString(); break;
            }
            switch (_Suit)
            {
				case 1: s += "♠"; break;
				case 2: s += "♥"; break;
				case 3: s += "♣"; break;
				case 4: s += "♦"; break;
            }
			s += (_Number == 10) ? "" : " ";
            return s;
        }
    }
}
