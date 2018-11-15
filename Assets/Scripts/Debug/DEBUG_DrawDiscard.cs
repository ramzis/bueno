namespace Tadget
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DEBUG_DrawDiscard : MonoBehaviour {

        public RoomEvents roomEvents;
        public int aDrawCount = 0, aDiscardCount = 0;
        public int tDrawCount = 0, tDiscardCount = 0;

        private void Awake()
        {
            roomEvents = GetComponent<RoomEvents>();

            roomEvents.PlayerDiscardCardEvent += PlayerDiscardCardEventHandler;
            roomEvents.PlayerDrawCardEvent += PlayerDrawCardEventHandler;
            roomEvents.PlayerAnonDrawEvent += PlayerAnonDrawEventHandler;
        }

        private void PlayerDiscardCardEventHandler(string playerName, Card card)
        {
            if(playerName == "Tadas")
            {
                tDiscardCount++;
            }
            else if(playerName == "Aldona")
            {
                aDiscardCount++;
            }
        }

        private void PlayerDrawCardEventHandler(string playerName, Card card)
        {
            if(playerName == "Tadas")
            {
                tDrawCount++;
            }
            else if(playerName == "Aldona")
            {
                aDrawCount++;
            }
        }

        private void PlayerAnonDrawEventHandler(string playerName)
        {
            if(playerName == "Tadas")
            {
                tDrawCount++;
            }
            else if(playerName == "Aldona")
            {
                aDrawCount++;
            }
        }
    }
}
