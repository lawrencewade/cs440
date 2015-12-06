using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RandomForest;

namespace Mao
{
    class MaoAI : Player
    {
        Deck _ImaginaryDeck = new Deck();

        Forest _Forest;

        public Card MakePlay(Card DownCard, Hand Hand)
        {
            if (_Forest == null) return Hand[0];

            AttributeValue[] Entry = new AttributeValue[5];

            Entry[0] = new IntegerValue(DownCard.Number);
            Entry[1] = new IntegerValue(DownCard.Suit);

            List<Card> V = new List<Card>();
            foreach (Card Card in Hand)
            {
                Entry[2] = new IntegerValue(Card.Number);
                Entry[3] = new IntegerValue(Card.Suit);
                bool a = Convert.ToBoolean(_Forest.MakeDecision(Entry).ToString());
                if (a) V.Add(Card);
            }

            int M = Int32.MaxValue;
            Card Choice = null;
            foreach (Card Card in V)
            {
                int N = 0;
                Entry[2] = new IntegerValue(Card.Number);
                Entry[3] = new IntegerValue(Card.Suit);
                foreach (Card Down in _ImaginaryDeck)
                {
                    Entry[0] = new IntegerValue(DownCard.Number);
                    Entry[1] = new IntegerValue(DownCard.Suit);
                    bool a = Convert.ToBoolean(_Forest.MakeDecision(Entry).ToString());
                    if (a) N++;
                }
                if (N < M)
                {
                    M = N;
                    Choice = Card;
                }
            }
            return Choice;
        }

        public bool ValidatePlay(Card DownCard, Card Played, Player played)
        {
            AttributeValue[] Entry = new AttributeValue[5];
            Entry[0] = new IntegerValue(DownCard.Number);
            Entry[1] = new IntegerValue(DownCard.Suit);
            Entry[2] = new IntegerValue(Played.Number);
            Entry[3] = new IntegerValue(Played.Suit);
            Entry[4] = new BooleanValue(false);

            if (_Forest == null) return true;
            else return Convert.ToBoolean(_Forest.MakeDecision(Entry).ToString());
        }

        public void VerifyPlay(Card DownCard, Card Played, bool Valid)
        {
            AttributeValue[] Entry = new AttributeValue[5];
            Entry[0] = new IntegerValue(DownCard.Number);
            Entry[1] = new IntegerValue(DownCard.Suit);
            Entry[2] = new IntegerValue(Played.Number);
            Entry[3] = new IntegerValue(Played.Suit);
            Entry[4] = new BooleanValue(Valid);

            if (_Forest == null) _Forest = new Forest(1, 1, delegate(int i) { DataSet D = new DataSet(5); D.AddEntry(Entry); return D; }, 4);
            else _Forest.AddEntry(Entry);
        }

        public override string ToString()
        {
            return _Forest.ToString();
        }
    }
}
