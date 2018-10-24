namespace Tadget
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class IPlayer : ScriptableObject {
		
		[SerializeField] public string playerName {get; protected set;}
        protected List<Card> hand;
        protected Game game;
        protected bool activeTurn;
        [SerializeField] public bool isAI {get; protected set;}

        public static IPlayer Create(string playerName, Game game, bool isAI) 
        {
			if(isAI)
			{
            	return ScriptableObject.CreateInstance<PlayerAI>().Init(playerName, game, isAI);
			}
			else
			{
            	return ScriptableObject.CreateInstance<PlayerHuman>().Init(playerName, game, isAI);
			}
        }

        private IPlayer Init(string playerName, Game game, bool isAI)
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
			MakeMove();
            activeTurn = false;
        }

		public abstract void MakeMove();

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

        public void SayUno()
        {
            Debug.LogFormat("[{0}] UNO!", playerName);
        }
	}
}
