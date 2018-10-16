namespace Tadget
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : ScriptableObject {

        [SerializeField] public string playerName {get; protected set;}
        protected List<Card> hand;
        protected Game game;
        protected bool activeTurn;
        [SerializeField] public bool isAI {get; protected set;}

        public static Player Create(string playerName, Game game, bool isAI) 
        {
            return ScriptableObject.CreateInstance<Player>().Init(playerName, game, isAI);
        }

        protected Player Init(string playerName, Game game, bool isAI)
        {
            hand = new List<Card>();
            this.playerName = playerName;
            this.game = game;
            this.isAI = isAI;
            return this;
        }

        public void NotifyTurn()
        {
            activeTurn = true;
            if(isAI) MakeMove();
            activeTurn = false;
        }

        public void GiveCards(List<Card> cards) 
        {
            foreach(Card c in cards) 
            {
                GiveCards(c);
            }
        }

        public void GiveCards(Card card)
        {
            hand.Add(card);
        }

        public bool HasCards(List<Card> cards)
        {
            Dictionary<Card, int> handCardCount = new Dictionary<Card, int>();
            foreach(Card c in hand) 
            {
                int count;
                if(handCardCount.TryGetValue(c, out count)) 
                {
                    handCardCount[c] = count + 1;
                }
                else 
                {
                    handCardCount.Add(c, 1);
                }
            }

            foreach(Card c in cards)
            {
                int count;
                if(handCardCount.TryGetValue(c, out count)) 
                {
                    if(count < 1)
                        return false;
                    else
                        handCardCount[c] = count - 1;
                }
                else 
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveCards(List<Card> cards)
        {
            hand.RemoveAll(x => cards.Contains(x));
        }

        public int GetCardCount()
        {
            return hand.Count;
        }

        protected void MakeMove()
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
                Debug.LogFormat("[{0}] Playing {1}.", playerName, move[0]);
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
                    Debug.LogFormat("[{0}] Playing {1}.", playerName, move[0]);
                    game.MakeMove(playerName, move);
                }
                else
                {
                    Debug.LogFormat("[{0}] No move to make.", playerName);
                    game.MakeMove(playerName, move);
                }
            }
        }

        protected void SayUno()
        {
            Debug.LogFormat("[{0}] UNO!", playerName);
        }
    }
}
