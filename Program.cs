using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomForest
{
    class Program
    {
        static bool ValidPlay(int DownNumber, int DownSuit, int CardNumber, int CardSuit)
        {
            return DownNumber == CardNumber || DownSuit == CardSuit;
        }

        static string DataToCard(int CardNumber, int CardSuit)
        {
            string n = "";
            switch (CardNumber)
            {
                case 11: n = "Jack"; break;
                case 12: n = "Queen"; break;
                case 13: n = "King"; break;
                case 1: n = "Ace"; break;
                default: n = CardNumber.ToString(); break;
            }
            string s = "";
            switch (CardSuit)
            {
                case 1: s = "Spades"; break;
                case 2: s = "Hearts"; break;
                case 3: s = "Clubs"; break;
                case 4: s = "Diamonds"; break;
            }
            return n + " of " + s;
        }

        static Random Random = new Random();
        static DataSet GenerateData(int DataSize)
        {
            List<string> A = new List<string>() { "DownNumber", "DownSuit", "Card0Number", "Card0Suit", "Card1Number", "Card1Suit", "Card2Number", "Card2Suit", "Play" };
            DataSet R = new DataSet();
            foreach (string a in A) R.AddAttribute(a, new DiscreteType());
            for (int c = 0; c < DataSize; ++c)
            {
                R.AddEntry(GenerateEntry());
            }
            return R;
        }

        static AttributeValue[] GenerateEntry()
        {
            int DownNumber = Random.Next(1, 14);
            int DownSuit = Random.Next(1, 5);
            AttributeValue[] E = new AttributeValue[9];
            E[0] = new IntegerValue(DownNumber);
            E[1] = new IntegerValue(DownSuit);
            int P = -1;
            for (int i = 0; i < 3; ++i)
            {
                int CardNumber = Random.Next(1, 14);
                int CardSuit = Random.Next(1, 5);
                E[i * 2 + 2] = new IntegerValue(CardNumber);
                E[i * 2 + 3] = new IntegerValue(CardSuit);
                if (ValidPlay(DownNumber, DownSuit, CardNumber, CardSuit)) P = i;
            }
            E[8] = new IntegerValue(P);

            return E;
        }

        static bool Validator(AttributeValue[] Entry)
        {
            int a = Convert.ToInt32(Entry[8].ToString());
            return Entry[0].CompareTo(Entry[a * 2 + 2]) == 0 || Entry[1].CompareTo(Entry[a * 2 + 3]) == 0;
        }

        static void Main(string[] args)
        {
            Forest D = new Forest(20, 5000, GenerateData, 8);

            int Total = 0;
            int Correct = 0;
            for(int c=0;c<50000; ++c)
            {
                AttributeValue[] E = GenerateEntry();
                AttributeValue Decision = D.MakeDecision(E);
                E[8] = Decision;
                Console.WriteLine(Decision);
                Total++;
                Correct += (Validator(E) ? 1 : 0);
            }
            Console.WriteLine(D);
            Console.WriteLine("{0}/{1}", Correct, Total);
            Console.ReadLine();
        }
    }
}
