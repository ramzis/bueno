namespace Tadget 
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// The room should completely be unaware of the rules of the game.
	/// It serves as a game->player tool which allows the game to manipulate data.
	/// Basically an interface for the game to interact with players without directly touching their data.
	public class Room : MonoBehaviour {
		
		[SerializeField] protected List<Player> players;

		public void Join(string playerName, Game game, bool isAI)
        {
            if (players == null)
                players = new List<Player>();
            AddPlayer(playerName, game, isAI);
			Debug.LogFormat("Player {0} joined", playerName);
        }

		public int GetPlayerCount()
		{
			if(players != null)
				return players.Count;
			else
				return 0;
		}

		public void GivePlayerCards(string playerName, List<Card> cards)
		{
			Player player = GetPlayer(playerName);
			if(player != null) 
			{
				player.GiveCards(cards);
			}
		}

		public void GivePlayerCards(string playerName, Card cards)
		{
			Player player = GetPlayer(playerName);
			if(player != null) 
			{
				player.GiveCards(cards);
			}
		}

		public List<string> GetPlayerNames() 
		{
			return players.ConvertAll(x => x.playerName);
		}

		public int GetPlayerCardCount(string playerName)
		{
			Player player = GetPlayer(playerName);
			if(player != null)
			{
				return player.GetCardCount();
			}
			else
			{
				return -1;
			}
		}

		public bool CheckIfPlayerHasCards(string playerName, List<Card> cards) 
		{
			Player player = GetPlayer(playerName);
			if(player != null) {
				return player.HasCards(cards);
			}
			else 
			{
				return false;
			}
		}

		public void RemovePlayerCards(string playerName, List<Card> cards)
		{
			Player player = GetPlayer(playerName);
			if(player != null) 
			{
				player.RemoveCards(cards);
			}
		}

		public void NotifyPlayerTurn(string playerName)
		{
			Player player = GetPlayer(playerName);
			if(player != null)
			{
				Debug.LogFormat("Notified {0} of turn through room", playerName);
				player.NotifyTurn();
			}			
		}

		protected Player GetPlayer(string playerName)
		{
			return players.Find(x => x.playerName == playerName);
		}

		protected void AddPlayer(string playerName, Game game, bool isAI)
		{
			players.Add(Player.Create(playerName, game, isAI));
		}

		protected void RemovePlayer(string playerName) 
		{
			throw new System.NotImplementedException();
		}
	}
}
