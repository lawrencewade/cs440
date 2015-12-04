using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class Program
    {
        static void Main(string[] args)
        {
            Player P = new CheckerAI(delegate(Card Down, Card Played)
                {
                    return Played.Number == Down.Number || Played.Suit == Down.Suit;
                });
            int Won = 0;
            int Total = 100;
            for (int i = 0; i < Total; ++i)
            {
                Game Game = new Game(P, new List<Player>() { new MaoAI() });
                Won += (Game.Start() > -1 ? 1 : 0);
                Console.WriteLine("{0} {1}", Won, i + 1);
            }
            Console.ReadLine();
        }
    }
}
