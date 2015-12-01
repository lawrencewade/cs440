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

        static string DataToCardAlt(AttributeValue CardNumber, AttributeValue CardSuit)
        {
            return DataToCard(Convert.ToInt32(CardNumber.ToString()), Convert.ToInt32(CardSuit.ToString()));
        }

        static Random Random = new Random();
        static DataSet GenerateData(int DataSize)
        {
            DataSet R = new DataSet(9);
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
            if (a > -1) return Entry[0].CompareTo(Entry[a * 2 + 2]) == 0 || Entry[1].CompareTo(Entry[a * 2 + 3]) == 0;
            else
            {
                for (int i = 0; i < 3; ++i)
                {
                    if(Entry[0].CompareTo(Entry[i * 2 + 2]) == 0 || Entry[1].CompareTo(Entry[i * 2 + 3]) == 0) return false;
                }
                return true;
            }
        }

        static void Main(string[] args)
        {
            Forest D = new Forest(1, 20000, GenerateData, 8);

            Console.WriteLine(D);
            int Correct = 0;
            for(int c=0;c<50000; ++c)
            {
                AttributeValue[] E = GenerateEntry();
                AttributeValue Decision = D.MakeDecision(E);
                E[8] = Decision;
                /*
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("DOWN IS {0}", DataToCardAlt(E[0], E[1]));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("I HAVE");
                Console.ForegroundColor = ConsoleColor.Green;
                for (int i = 0; i < 3; ++i) Console.WriteLine(DataToCardAlt(E[i * 2 + 2], E[i * 2 + 3]));
                Console.ForegroundColor = ConsoleColor.Magenta;
                int p = Convert.ToInt32(E[8].ToString());
                Console.WriteLine("I PLAY {0}", (p > -1 ? DataToCardAlt(E[p * 2 + 2], E[p * 2 + 3]) : "DRAW"));
                 */
                Correct += Validator(E) ? 1 : 0;
                if (!Validator(E)) D.MakeDecision(E, true);
            }
            Console.WriteLine("{0}/{1}", Correct, 50000);
            Console.ReadLine();
        }
    }
}
