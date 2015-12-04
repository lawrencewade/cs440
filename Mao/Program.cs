using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace Mao
{
    class Program
    {
        static List<Game> _Games = new List<Game>();

        static int Won = 0;
        static int Total = 0;

        static void Propagate(Func<Card, Card, bool> Rule, int Matches)
        {
            Random Random = new Random();
            Player P = new CheckerAI(Rule);
            for (int i = 0; i < Matches; ++i)
            {
                _Games.Add(new Game(P, new List<Player>() { new MaoAI()} , Random));
            }
        }

        static void Tester(Func<Card, Card, bool> Rule, ConsoleColor PrintColor)
        {
            while(_Games.Count > 0)
            {
                Game Game = null;
                lock (_Games)
                {
                    if (_Games.Count == 0) break;
                    Game = _Games[0];
                    _Games.RemoveAt(0);
                }

                try { int w = Game.Start() > -1 ? 1 : 0; lock (_Games) { Won += w; } }
                catch (Exception e)
                {
                    Console.ForegroundColor = PrintColor;
                    Console.WriteLine("GAME FAILED : {0}", e.Message);
                    lock (_Games)
                    {
                        _Games.Add(new Game(new CheckerAI(Rule), new List<Player>() { new MaoAI() }, new Random()));
                    }
                }
                lock (_Games)
                {
                    Console.ForegroundColor = PrintColor;
                    Console.WriteLine("{0}/{1} = {2}", Won, Total + 1, (double)Won / (Total + 1));
                    Total++;
                }
            }
        }

        static void Main(string[] args)
        {
            Func<Card, Card, bool> Rule = delegate(Card Down, Card Played)
            {
                return (Played.Number <= Down.Number && Played.Suit % 2 == 0) || (Played.Number >= Down.Number && Played.Suit % 2 == 1);
            };
            Propagate(Rule, 1000);
            Thread T1 = new Thread(new ThreadStart(delegate() { Tester(Rule, ConsoleColor.Red);}));
            Thread T2 = new Thread(new ThreadStart(delegate() { Tester(Rule, ConsoleColor.Green); }));
            Thread T3 = new Thread(new ThreadStart(delegate() { Tester(Rule, ConsoleColor.Blue); }));
            T1.Start();
            T2.Start();
            T3.Start();
            Tester(Rule, ConsoleColor.Yellow);
            T1.Join();
            T2.Join();
            T2.Join();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}
