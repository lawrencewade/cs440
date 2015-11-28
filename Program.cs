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
            List<string> A = new List<string>() { "DownNumber", "DownSuit", "CardNumber", "CardSuit" };
            DataSet R = new DataSet();
            foreach (string a in A) R.AddAttribute(a, new DiscreteType());
            R.AddAttribute("Valid", new DiscreteType());
            for (int c = 0; c < DataSize; ++c)
            {
                int DownNumber = Random.Next(1, 14);
                int DownSuit = Random.Next(1, 5);
                Dictionary<string, AttributeValue> E = new Dictionary<string, AttributeValue>();
                E.Add("DownNumber", new IntegerValue(DownNumber));
                E.Add("DownSuit", new IntegerValue(DownSuit));
                int CardNumber = Random.Next(1, 14);
                int CardSuit = Random.Next(1, 5);
                E.Add("CardNumber", new IntegerValue(CardNumber));
                E.Add("CardSuit", new IntegerValue(CardSuit));
                E.Add("Valid", new BooleanValue(ValidPlay(DownNumber, DownSuit, CardNumber, CardSuit)));
                R.AddEntry(E);
            }
            return R;
        }

        static void Main(string[] args)
        {
            Forest D = new Forest(1000, 100, GenerateData, "Valid");

            int Total = 0;
            int Correct = 0;
            for(int c=0;c<5000; ++c)
            {
                int DownNumber = Random.Next(1, 14);
                int DownSuit = Random.Next(1, 5);
                Dictionary<string, AttributeValue> E = new Dictionary<string, AttributeValue>();
                E.Add("DownNumber", new IntegerValue(DownNumber));
                E.Add("DownSuit", new IntegerValue(DownSuit));
                Console.WriteLine(DataToCard(DownNumber, DownSuit));
                int CardNumber = Random.Next(1, 14);
                int CardSuit = Random.Next(1, 5);
                Console.WriteLine(DataToCard(CardNumber, CardSuit));
                E.Add("CardNumber", new IntegerValue(CardNumber));
                E.Add("CardSuit", new IntegerValue(CardSuit));
                AttributeValue Decision = D.MakeDecision(E);
                Console.WriteLine(Decision);
                Total++;
                Correct += (new BooleanValue(ValidPlay(DownNumber, DownSuit, CardNumber, CardSuit)).CompareTo(Decision) == 0 ? 1 : 0);
            }

            Console.WriteLine("{0}/{1}", Correct, Total);
            Console.ReadLine();
        }
    }
}
