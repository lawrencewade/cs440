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
            Player P = new Human();
            for (int i = 0; i < Matches; ++i)
            {
                _Games.Add(new Game(P, new List<Player>() { new MaoAI()} , Random));
            }
        }

        static void Tester(Func<Card, Card, bool> Rule, ConsoleColor PrintColor/*, bool foreground*/)
        {
			int oldWon = 0;
            while(_Games.Count > 0)
            {
				oldWon = Won;
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
					continue;
                }
                lock (_Games)
                {
                    Console.ForegroundColor = PrintColor;
					if (oldWon != Won)
						Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine("{0}/{1} = {2}", Won, Total + 1, (double)Won / (Total + 1));
					Console.BackgroundColor = ConsoleColor.Black;
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
            Propagate(Rule, 64);
			int nThreads = 1;
			Thread[] pool = new Thread[nThreads];
			// Initialize threads;
			ConsoleColor[] colors = {ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Magenta,
				ConsoleColor.Gray, ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, ConsoleColor.DarkYellow, ConsoleColor.DarkCyan, ConsoleColor.DarkMagenta};
			for (int n=0; n < nThreads; ++n)
			{
				ConsoleColor c = (n < colors.Length) ? colors [n] : ConsoleColor.White;
	            pool[n] = new Thread(new ThreadStart(delegate() { Tester(Rule, c);}));
				pool[n].Start();
			}

			// Wait for finish..
			foreach (Thread t in pool)
			{
				t.Join();
			}
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("DONE");
            //Console.ReadLine();
        }
    }
}
