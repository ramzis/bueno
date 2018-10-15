namespace Tadget 
{
    using System;
    using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class GameEvents : MonoBehaviour {

		private delegate void PlayerCardEvent(Player p, Card c);
		private event PlayerCardEvent DrawCardEvent;
		private event PlayerCardEvent DiscardCardEvent;
		private Action<string> PlayerJoinEvent;

		public void PlayerJoin(string name) 
		{
			if(PlayerJoinEvent != null) 
            {
                PlayerJoinEvent.Invoke(name);
            }
		}

		public void DrawCard(Player player, Card card)
		{
			if(DrawCardEvent != null) 
                DrawCardEvent.Invoke(player, card);
		}
	}
}
