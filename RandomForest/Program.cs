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
            return DownNumber + CardNumber == 14 || (DownSuit == 2 && CardNumber > 7) || (DownSuit == 4 && CardNumber < 8) || CardNumber == DownNumber;
        }

        static bool ValidPlayAlt(AttributeValue DownNumber, AttributeValue DownSuit, AttributeValue CardNumber, AttributeValue CardSuit)
        {
            return ValidPlay(Convert.ToInt32(DownNumber.ToString()), Convert.ToInt32(DownSuit.ToString()), Convert.ToInt32(CardNumber.ToString()), Convert.ToInt32(CardSuit.ToString()));
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
            DataSet R = new DataSet(5);
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
            AttributeValue[] E = new AttributeValue[5];
            E[0] = new IntegerValue(DownNumber);
            E[1] = new IntegerValue(DownSuit);
            int CardNumber = Random.Next(1, 14);
            int CardSuit = Random.Next(1, 5);
            E[2] = new IntegerValue(CardNumber);
            E[3] = new IntegerValue(CardSuit);
            E[4] = new BooleanValue(ValidPlay(DownNumber, DownSuit, CardNumber, CardSuit));

            return E;
        }

        static bool Validator(AttributeValue[] Entry)
        {
            bool a = Convert.ToBoolean(Entry[4].ToString());
            return a == ValidPlayAlt(Entry[0], Entry[1], Entry[2], Entry[3]);
        }

        static void Main(string[] args)
        {
            Forest D = new Forest(1, 2000, GenerateData, 4);

            Console.WriteLine(D);
            int Correct = 0;
            for(int c=0;c<500000; ++c)
            {
                AttributeValue[] E = GenerateEntry();
                AttributeValue Decision = D.MakeDecision(E);
                E[4] = Decision;
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
                //if (!Validator(E)) D.MakeDecision(E, true);
            }
            Console.WriteLine("{0}/{1} {2}", Correct, 500000, (double)Correct / 500000);
            Console.ReadLine();
        }
    }
}
