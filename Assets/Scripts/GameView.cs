namespace Tadget
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class GameView : MonoBehaviour {

        private static Dictionary<string, int> handCount;
        private static List<Card> hand;
        private static List<Card> discard;

        public Text handStatus;
        public Text currentCard;

        private void Awake()
        {
            hand = new List<Card>();
            handCount = new Dictionary<string, int>();
            discard = new List<Card>();
            SubscribeToRoomEvents();
        }

        private void SubscribeToRoomEvents()
        {
            RoomEvents roomEvents = GetComponent<RoomEvents>();
            roomEvents.PlayerDiscardCardEvent += PlayerDiscardCardEventHandler;
            roomEvents.PlayerDrawCardEvent += PlayerDrawCardEventHandler;
            roomEvents.PlayerAnonDrawEvent += PlayerAnonDrawEventHandler;
            roomEvents.PlayerJoinEvent += PlayerJoinEventHandler;
            roomEvents.DeckDiscardCardEvent += DeckDiscardCardEventHandler;
        }

        public static void PlayerDiscardCardEventHandler(string playerName, Card card)
        {
            int val;
            if(handCount.TryGetValue(playerName, out val))
            {
                handCount[playerName] = val - 1;
            }
            else 
            {
                Debug.LogWarningFormat("[GAME VIEW] Player {0} not found in handCount dictionary.", playerName);
            }

            discard.Add(card);
        }

        public static void PlayerDrawCardEventHandler(string playerName, Card card)
        {
            int val;
            if(handCount.TryGetValue(playerName, out val))
            {
                handCount[playerName] = val + 1;
            }
            else 
            {
                Debug.LogWarningFormat("[GAME VIEW] Player {0} not found in handCount dictionary.", playerName);
            }

            hand.Add(card);
        }

        public static void PlayerAnonDrawEventHandler(string playerName)
        {
            int val;
            if(handCount.TryGetValue(playerName, out val))
            {
                handCount[playerName] = val + 1;
            }
            else 
            {
                Debug.LogWarningFormat("[GAME VIEW] Player {0} not found in handCount dictionary.", playerName);
            }
        }

        public static void PlayerJoinEventHandler(string playerName) 
        {
            int val;
            if(handCount.TryGetValue(playerName, out val))
            {
                handCount[playerName] = 0;
                Debug.LogWarningFormat("[GAME VIEW] Player {0} existing hand count reset when player joined.", playerName);
            }
            else 
            {
                handCount.Add(playerName, 0);
            }
        }

        public static void DeckDiscardCardEventHandler(Card card)
        {
            discard.Add(card);
        }

        private void Update() 
        {
            string text = "";
            foreach(string v in handCount.Keys)
            {
                text += string.Format("Player {0}: {1} card(s).{2}", v, handCount[v], System.Environment.NewLine); 
            }
            if(handStatus != null)
            {
                handStatus.text = text;
            }
            if(discard != null && discard.Count > 0)
            {
                currentCard.text = discard.Last().ToString();
            }
        }
    }
}
