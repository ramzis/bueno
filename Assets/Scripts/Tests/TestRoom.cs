namespace Tadget.Test
{
    using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    [TestFixture]
	public class TestRoom : Room {

		[SetUp]
        public void Init()
        {
			players = null;
        }

		private void InitPlayerList()
		{
			players = new List<Player>();
		}

		[Test]
        public void PlayersJoinGame()
        {
            Debug.Assert(players == null);
			Join("Aldona", null, true);
            Debug.Assert(players.Count == 1);
            Debug.Assert(players.Exists(p => p.playerName == "Aldona"));
			Join("Tadas", null, true);
            Debug.Assert(players.Count == 2);
            Debug.Assert(players.Exists(p => p.playerName == "Tadas"));
        }

		[Test]
		public void PlayersAreCountedCorrectly()
		{
			Debug.Assert(GetPlayerCount() == 0);
			Join("Aldona", null, true);
			Debug.Assert(GetPlayerCount() == 1);
			Join("Tadas", null, true);
			Debug.Assert(GetPlayerCount() == 2);
		}

		[Test]
		public void PlayersAreGivenOneCard()
		{
			Join("Aldona", null, true);
			Player a = GetPlayer("Aldona");
			Debug.Assert(a.GetCardCount() == 0);

			Card c1 = Card.Create(Card.Type._0, Card.Color._1);
			GivePlayerCards("Aldona", c1);
			Debug.Assert(a.GetCardCount() == 1);

			Card c2 = Card.Create(Card.Type._0, Card.Color._1);
			GivePlayerCards("Aldona", c2);
			Debug.Assert(a.GetCardCount() == 2);
		}

	}
}