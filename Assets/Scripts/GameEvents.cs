namespace Tadget 
{
    using System;
    using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class GameEvents : MonoBehaviour {

		private delegate void PlayerCardEvent(string p, Card c);
		private event PlayerCardEvent DrawCardEvent;
		private event PlayerCardEvent DiscardCardEvent;
		private Action<string> PlayerJoinEvent;

		public void PlayerJoin(string playerName) 
		{
			if(PlayerJoinEvent != null) 
            {
                PlayerJoinEvent.Invoke(playerName);
            }
		}

		public void DrawCard(string playerName, Card card)
		{
			if(DrawCardEvent != null) 
                DrawCardEvent.Invoke(playerName, card);
		}

		public void DiscardCard(string playerName, Card card)
		{
			if(DiscardCardEvent != null)
			{
				DiscardCardEvent.Invoke(playerName, card);
			}
		}
	}
}
