using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class Human : Player
    {
		ConsoleColor cardColor = ConsoleColor.White;

        public Card MakePlay(Card Down, Hand Hand)
        {
			BigCard(Down, true);
			ShowHand(Hand);
            //Console.WriteLine(Hand);
            Console.Write("Your Play (Nothing for draw): ");
            while (true)
            {
				string input = Console.ReadLine();
				if (input.Length == 0) // Draw
					return null;
                try
                {
                    int C = Convert.ToInt32(input);
                    if (C > -1) return Hand[C];
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        public bool ValidatePlay(Card Down, Card Played)
        {
			Console.WriteLine("My Play:");
			BigCard (Played, false);
			Console.Write("vvvvv");
			BigCard (Down, true);
			Console.Write("Was this valid? ");
            while (true)
            {
				string input = Console.ReadLine();
				input = input.ToLower();
				switch (input[0])
				{
					case 'y': case 't': case '1':
						return true;
					case 'n': case 'f': case '0':
						return false;
					default:
						Console.WriteLine("(y/n)(t/f)(1/0):");
					break;
				}
				
            }
        }

        public void VerifyPlay(Card Down, Card Played, bool Valid)
        {
            return;
        }

		private void WriteCard(Card card)
		{
			ConsoleColor prev = Console.ForegroundColor;
			Console.ForegroundColor = (card.Suit == 2 || card.Suit == 4) ? ConsoleColor.Red : ConsoleColor.DarkGray;
			Console.Write(card);
			Console.ForegroundColor = prev;
		}

		// Draws a big card stack is true if card is not single but top of a stack
		private void BigCard(Card card, bool stack)
		{
			Console.ForegroundColor = cardColor;
			if (stack)
			{
				Console.Write("\n┌───╖\n│");
				WriteCard(card);
				Console.WriteLine("║\n│   ║\n╘═══╝", card);
			}
			else
			{
				Console.Write("\n┌───┐\n│");
				WriteCard(card);
				Console.WriteLine("│\n│   │\n└───┘");
			}
			Console.ResetColor();
		}
		// Draws the back of a card
		private void BackCard(bool stack)
		{
			Console.ForegroundColor = cardColor;
			ConsoleColor back = ConsoleColor.Gray;
			if (stack)
			{
				Console.Write("\n┌───╖\n│");
				Console.ForegroundColor = back;
				Console.Write("┼─┼");
				Console.ForegroundColor = cardColor;
				Console.Write("║\n│");
				Console.ForegroundColor = back;
				Console.Write("┼─┼");
				Console.ForegroundColor = cardColor;
				Console.WriteLine("║\n╘═══╝");

			}
			else
			{
				Console.Write("\n┌───┐\n│");
				Console.ForegroundColor = back;
				Console.Write("###");
				Console.ForegroundColor = cardColor;
				Console.Write("│\n│");
				Console.ForegroundColor = back;
				Console.Write("###");
				Console.ForegroundColor = cardColor;
				Console.WriteLine("│\n└───┘");
			}
		}

		private void ShowHand(Hand hand)
		{
			int i = 0;
			/*string s = "";
			foreach (Card c in hand)
				s += i++ + "\t";
			Console.WriteLine(s);*/

			foreach (Card c in hand)
			{
				Console.Write ("{0} | ", i);
				WriteCard (c);
				Console.WriteLine();

				++i;
			}
		}
    }
}
