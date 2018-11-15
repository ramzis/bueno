namespace Tadget
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerAI : IPlayer {

        public override void MakeMove()
        {
            List<Card> move = new List<Card>();
            foreach(var c in hand)
            {
                if (game.ValidateMove(game.GetCurrentCard(), c))
                {
                    move.Add(c);
                    break;
                }
            }
            if (move.Count > 0)
            {
                if (hand.Count - move.Count == 1) SayUno();
                // Debug.LogFormat("[{0}] Playing {1}.", playerName, move[0]);
                game.MakeMove(playerName, move);
            }
            else
            {
                game.RequestDraw(playerName);
                if (game.ValidateMove(game.GetCurrentCard(), hand.Last()))
                {
                    move.Clear();
                    move.Add(hand.Last());
                    if (hand.Count - move.Count == 1) SayUno();
                    // Debug.LogFormat("[{0}] Playing {1}.", playerName, move[0]);
                    game.MakeMove(playerName, move);
                }
                else
                {
                    // Debug.LogFormat("[{0}] No move to make.", playerName);
                    game.MakeMove(playerName, move);
                }
            }
        }
    }
}
