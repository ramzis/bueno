﻿namespace Tadget
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
        }

        public static void PlayerDiscardCardEventHandler(string playerName, Card card)
        {
            // Debug.LogFormat("[GAME VIEW] PlayerDiscardCardEventHandler()");

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
            //hand.Remove(card);
        }

        public static void PlayerDrawCardEventHandler(string playerName, Card card)
        {
            // Debug.LogFormat("[GAME VIEW] PlayerDrawCardEventHandler()");

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
            // Debug.LogFormat("[GAME VIEW] PlayerAnonDrawEventHandler()");

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
            // Debug.LogFormat("[GAME VIEW] PlayerJoinEventHandler()");

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
