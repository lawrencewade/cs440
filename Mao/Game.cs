using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    class Game
    {
        Random _Random;
        Deck _Deck;
        Deck _Return;
        Card _DownCard;

        MaoAI _AI = new MaoAI();
        Hand _Human;
        Hand _Computer;

        int _Plays;

        public Game()
        {
            _Random = new Random();
            _Deck = new Deck();
            _Deck.Shuffle(_Random);
            _Return = new Deck(true);
            _Human = new Hand(10, _Deck);
            _Computer = new Hand(10, _Deck);
            _DownCard = _Deck.Draw();
        }

        public void HumanTurn()
        {
            Console.WriteLine("DOWN IS {0}", _DownCard);
            Console.WriteLine(_Human);
            Console.Write("YOU PLAY: ");
            int C = -1;
            bool valid = false;
            while (!valid)
            {
                try
                {
                    C = Convert.ToInt32(Console.ReadLine());
                    if (C == -2) Console.WriteLine(_AI);
                    else valid = true;
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            if (C > -1)
            {
                Card P = _Human[C];
                _Human.Play(P,_Return);
                _AI.WatchPlay(_DownCard, P);
                _DownCard = P;
            }
            else
            {
                _Human.Draw(_Deck);
                CheckDeck();
            }
        }

        public void AITurn()
        {
            Console.WriteLine("DOWN IS {0}", _DownCard);
            Card C = _AI.MakePlay(_DownCard, _Computer);
            if (C == null)
            {
                Console.WriteLine("I DRAW");
                _Computer.Draw(_Deck);
                CheckDeck();
            }
            else
            {
                Console.WriteLine("I PLAY {0}", C);
                Console.Write("IS THIS OKAY? (True/False): ");
                bool valid = false;
                bool a= false;
                while (!valid)
                {
                    try { a = Convert.ToBoolean(Console.ReadLine()); valid = true; }
                    catch (Exception E) { Console.WriteLine(E.Message); }
                }
                _AI.VerifyPlay(_DownCard, C, a);
                if (a)
                {
                    _DownCard = C;
                    _Computer.Play(C, _Return);
                }
                else
                {
                    _Computer.Draw(_Deck);
                    CheckDeck();
                }
            }
        }

        private void CheckDeck()
        {
            if (_Deck.Count == 0)
            {
                _Return.Shuffle(_Random);
                _Deck = _Return;
                _Return = new Deck(true);
            }
        }

        public void Start()
        {
            while (_Human.Count > 0 && _Computer.Count > 0)
            {
                PrintGameData();
                HumanTurn();
                Console.Clear();
                AITurn();
                Console.Clear();
                _Plays++;
            }
            if (_Human.Count == 0) Console.WriteLine("YOU HAVE DEFEATED ME.");
            else Console.WriteLine("I HAVE DEFEATED YOU!");
            Console.WriteLine(_AI);
            Console.ReadLine();
        }

        public void PrintGameData()
        {
            Console.WriteLine("TURN NUMBER {0} | {1} VS. {2}", _Plays, _Human.Count, _Computer.Count);
        }
    }
}
