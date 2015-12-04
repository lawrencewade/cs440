using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class Human : Player
    {
        public Card MakePlay(Card Down, Hand Hand)
        {
            Console.WriteLine("DOWN IS {0}", Down);
            Console.WriteLine(Hand);
            Console.Write("YOU PLAY: ");
            while (true)
            {
                try
                {
                    int C = Convert.ToInt32(Console.ReadLine());
                    if (C > -1) return Hand[C];
                    else return null;
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        public bool ValidatePlay(Card Down, Card Played)
        {
            Console.WriteLine("DOWN IS {0}", Down);
            Console.WriteLine("I PLAY {0}", Played);
            Console.Write("IS THIS OKAY? (True/False): ");
            while (true)
            {
                try { return Convert.ToBoolean(Console.ReadLine()); }
                catch (Exception E) { Console.WriteLine(E.Message); }
            }
        }

        public void VerifyPlay(Card Down, Card Played, bool Valid)
        {
            return;
        }
    }
}
