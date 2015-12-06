using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mao
{
    interface Player
    {
        Card MakePlay(Card Down, Hand Hand);
        bool ValidatePlay(Card Down, Card Played, Player player);
        void VerifyPlay(Card Down, Card Played, bool Valid);
    }
}
