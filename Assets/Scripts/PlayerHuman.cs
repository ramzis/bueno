namespace Tadget
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerHuman : IPlayer {

        public void SetView(PlayerView playerView)
        {
            this.playerView = playerView;
        }

        public override void MakeMove()
        {
            //List<Card> move = new List<Card>();
            //game.MakeMove(playerName, move);
            //MakeRandomMove(hand);
        }

        public void DrawButton()
        {
            game.RequestDraw(playerName);
            List<Card> move = new List<Card>();
            game.MakeMove(playerName, move);
        }

        public void MakeRandomMove(List<Card> hand)
        {
            List<Card> move = new List<Card>();
            Card current = game.GetCurrentCard();
            foreach(Card c in hand)
            {
                if(game.ValidateMove(current, c))
                {
                    move.Add(c);
                    break;
                }
            }
            game.MakeMove(playerName, move);
        }
    }
}
