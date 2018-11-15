namespace Tadget 
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RoomEvents : MonoBehaviour {

        public delegate void PlayerCardEvent(string p, Card c);
        public event PlayerCardEvent PlayerDiscardCardEvent;
        public event PlayerCardEvent PlayerDrawCardEvent;
        public Action<string> PlayerAnonDrawEvent;
        public Action<string> PlayerJoinEvent;
        public Action<Card> DeckDiscardCardEvent; 

        void Awake()
        {
            // PlayerDiscardCardEvent += (p, c) => Debug.LogFormat("[ROOM EVENTS] Player {0} discarded {1}", p, c);
            // PlayerDrawCardEvent += (p, c) => Debug.LogFormat("[ROOM EVENTS] Player {0} drew {1}", p, c);
            // PlayerAnonDrawEvent += (p) => Debug.LogFormat("[ROOM EVENTS] Player {0} drew a card.", p);
            // PlayerJoinEvent += (p) => Debug.LogFormat("[ROOM EVENTS] Player {0} joined.", p);
            
            PlayerDiscardCardEvent += GameView.PlayerDiscardCardEventHandler;
            PlayerDrawCardEvent += GameView.PlayerDrawCardEventHandler;
            PlayerAnonDrawEvent += GameView.PlayerAnonDrawEventHandler;
            PlayerJoinEvent += GameView.PlayerJoinEventHandler;
            DeckDiscardCardEvent += GameView.DeckDiscardCardEventHandler;
        }

        public void PlayerJoin(string playerName) 
        {
            if(PlayerJoinEvent != null) 
            {
                PlayerJoinEvent.Invoke(playerName);
            }
        }

        public void PlayerAnonDraw(string playerName)
        {
            if(PlayerAnonDrawEvent != null) 
                PlayerAnonDrawEvent.Invoke(playerName);
        }

        public void PlayerDrawCard(string playerName, Card card)
        {
            if(PlayerDrawCardEvent != null) 
                PlayerDrawCardEvent.Invoke(playerName, card);
        }

        public void PlayerDiscardCard(string playerName, Card card)
        {
            if(PlayerDiscardCardEvent != null)
            {
                PlayerDiscardCardEvent.Invoke(playerName, card);
            }
        }

        public void DeckDiscardCard(Card card)
        {
            if(DeckDiscardCardEvent != null)
            {
                DeckDiscardCardEvent.Invoke(card);
            }
        }
    }
}
