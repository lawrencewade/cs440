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

        Hand _MasterHand;
        List<Hand> _LearnerHands = new List<Hand>();

        Player _Master;
        List<Player> _Learners;

        int _Plays;

        public Game(Player Master, List<Player> Learners, Random Random)
        {
            _Random = Random;
            _Deck = new Deck();
            _Deck.Shuffle(_Random);
            _Return = new Deck(true);
            _Master = Master;
            _Learners = Learners;
            _MasterHand = new Hand(10, _Deck);
            foreach(Player L in _Learners) _LearnerHands.Add(new Hand(10, _Deck));
            _DownCard = _Deck.Draw();
        }

        private void Turn(Player Player, Hand Hand, bool Master= false)
        {
			//Console.WriteLine (Player);
            Card C = Player.MakePlay(_DownCard, Hand);
            if (C == null)
            {
                Hand.Draw(_Deck);
                CheckDeck();
                return;
            }
            bool Valid = (Master ? true : _Master.ValidatePlay(_DownCard, C));
            foreach (Player L in _Learners) L.VerifyPlay(_DownCard, C, Valid);
            if (Valid)
            {
                Hand.Play(C, _Return);
                _DownCard = C;
            }
            else
            {
                Hand.Draw(_Deck);
                CheckDeck();
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

        public int Start()
        {
            while (!Done())
            {
                //PrintGameData();
                Turn(_Master, _MasterHand, true);
                //Console.Clear();
                for (int i = 0; i < _Learners.Count; ++i)
                {
                    Turn(_Learners[i], _LearnerHands[i]);
                    //Console.Clear();
                }
                _Plays++;
                if (_Plays > 500) throw new Exception("Game exceeded 500 turns");
            }
            if (_MasterHand.Count == 0) return -1;
            else
            {
                for (int i = 0; i < _LearnerHands.Count; ++i)
                {
                    if (_LearnerHands[i].Count == 0) return i;
                }
            }
            return -1;
        }

        private bool Done()
        {
            foreach (Hand Hand in _LearnerHands)
            {
                if (Hand.Count == 0) return true;
            }
            return _MasterHand.Count == 0;
        }

        private string HandCounts()
        {
            string r = "";
            foreach (Hand Hand in _LearnerHands) r += Hand.Count + " ";
            return r;
        }

        public void PrintGameData()
        {
            Console.WriteLine("TURN NUMBER {0} | {1} {2}", _Plays, _MasterHand.Count, HandCounts());
        }
    }
}
