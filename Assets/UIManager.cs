namespace Tadget {
	
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;
	using System.Linq;
    using System.Collections.ObjectModel;

    public class UIManager : MonoBehaviour 
	{
		/*
		public List<Player> players;
		public List<Text> textBoxes;
		public Text discard;

		private Dictionary<Player, System.Collections.ObjectModel.ReadOnlyCollection<Card>> UIhands;
		[SerializeField] private Game game;

		[SerializeField] private GameObject cardPrefab;
		[SerializeField] private GameObject playerCardHolder;
		[SerializeField] private List<GameObject> playerCards;

		void Start () 
		{
			UIhands = new Dictionary<Player, ReadOnlyCollection<Card>>();
			playerCards = new List<GameObject>();
		}

		void Update () 
		{
			if(players != null)
			{
				for (int i = 0; i < players.Count; i++)
				{
					var cards = players[i].hand.Select(c => c.ToString()).ToArray();
					string h = string.Join(System.Environment.NewLine, cards);
					
					if(textBoxes != null) 
					{
						if(textBoxes.Count > i) 
						{
							textBoxes[i].text = h;
						}
					}
				}

				foreach (var p in players)
				{
					UIhands.Add(p, p.hand.AsReadOnly());
				}
			}

			if(game != null && discard != null) 
			{
				discard.text = game.GetCurrentCard().ToString();
			}

			UpdatePlayerHandDisplay();
		}

		private void UpdatePlayerHandDisplay() 
		{

		}*/
	}
}