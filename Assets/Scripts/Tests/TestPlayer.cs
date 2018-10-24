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
	public class TestPlayer {
		
		[SetUp]
        public void Init()
        {
        }
		
		[Test]
		public void PlayerIsCreated()
		{
			IPlayer p = IPlayer.Create("Aldona", null, true);
			
			Debug.Assert(p.playerName == "Aldona");
			Debug.Assert(p.isAI == true);
		}

		[Test]
		public void CardIsReceived()
		{
			IPlayer p = IPlayer.Create("Aldona", null, true);
			Debug.Assert(p.GetCardCount() == 0);
			
			Card c1 = Card.Create(Card.Type._0, Card.Color._1);
			p.GiveCards(c1);
			
			Debug.Assert(p.GetCardCount() == 1);
		}

		[Test]
		public void CardsAreReceived()
		{
			IPlayer p = IPlayer.Create("Aldona", null, true);
			Debug.Assert(p.GetCardCount() == 0);
			
			Card c1 = Card.Create(Card.Type._0, Card.Color._1);
			Card c2 = Card.Create(Card.Type._0, Card.Color._1);
			p.GiveCards(new List<Card>(){ c1, c2 });
			
			Debug.Assert(p.GetCardCount() == 2);
		}

		[Test]
		public void HasCardsCheckIsCorrect()
		{
			IPlayer p = IPlayer.Create("Aldona", null, true);
			
			Card c1 = Card.Create(Card.Type._0, Card.Color._1);
			Card c2 = Card.Create(Card.Type._0, Card.Color._1);
			Card c3 = Card.Create(Card.Type._1, Card.Color._2);
			Card c4 = Card.Create(Card.Type._1, Card.Color._2);
			
			Debug.Assert(p.HasCards(new List<Card>(){}));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c2 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c3 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c2 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c2, c3 }));

			p.GiveCards(new List<Card>(){ c1 });

			Debug.Assert(p.HasCards(new List<Card>(){ c1 }));
			Debug.Assert(p.HasCards(new List<Card>(){ c2 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c3 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c4 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c2 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c1 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c3 }));

			p.GiveCards(new List<Card>(){ c2 });

			Debug.Assert(p.HasCards(new List<Card>(){ c1 }));
			Debug.Assert(p.HasCards(new List<Card>(){ c2 }));
			Debug.Assert(p.HasCards(new List<Card>(){ c1, c2 }));
			Debug.Assert(p.HasCards(new List<Card>(){ c2, c1 }));
			Debug.Assert(p.HasCards(new List<Card>(){ c1, c1 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c1, c2 }));

			p.GiveCards(new List<Card>(){ c4 });

			Debug.Assert(p.HasCards(new List<Card>(){ c1, c2, c3 }));
			Debug.Assert(p.HasCards(new List<Card>(){ c1, c2, c4 }));

			p.GiveCards(new List<Card>(){ c3 });

			Debug.Assert(p.HasCards(new List<Card>(){ c1, c1, c3, c3 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c3, c3, c4 }));
		}
		
		[Test]
		public void PlayerRemovesCards()
		{
			IPlayer p = IPlayer.Create("Aldona", null, true);
			
			Card c1 = Card.Create(Card.Type._0, Card.Color._1);
			Card c2 = Card.Create(Card.Type._0, Card.Color._2);
			Card c3 = Card.Create(Card.Type._0, Card.Color._3);
			Card c4 = Card.Create(Card.Type._0, Card.Color._4);

			p.GiveCards(new List<Card>(){ c1, c2, c3, c4 });
			Debug.Assert(p.HasCards(new List<Card>(){ c1, c2, c3, c4 }));

			p.RemoveCards(new List<Card>(){ c1 });
			Debug.Assert(p.HasCards(new List<Card>(){ c2, c3, c4 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c1, c2, c3, c4 }));

			p.RemoveCards(new List<Card>(){ c2, c3 });
			Debug.Assert(p.HasCards(new List<Card>(){ c4 }));
			Debug.Assert(!p.HasCards(new List<Card>(){ c2, c3 }));
	
			p.RemoveCards(new List<Card>(){ c4 });
			Debug.Assert(p.HasCards(new List<Card>(){}));
			Debug.Assert(!p.HasCards(new List<Card>(){ c4 }));
		}

		[Test]
		public void CardCountCheckIsCorrect()
		{
			IPlayer p = IPlayer.Create("Aldona", null, true);

			Card c1 = Card.Create(Card.Type._0, Card.Color._1);
			Card c2 = Card.Create(Card.Type._0, Card.Color._2);
			Card c3 = Card.Create(Card.Type._0, Card.Color._3);
			Card c4 = Card.Create(Card.Type._0, Card.Color._4);

			Debug.Assert(p.GetCardCount() == 0);
			p.GiveCards(new List<Card>() { c1 });
			Debug.Assert(p.GetCardCount() == 1);
			p.GiveCards(new List<Card>() { c2, c3 });
			Debug.Assert(p.GetCardCount() == 3);
			p.GiveCards(new List<Card>() { c4 });
			Debug.Assert(p.GetCardCount() == 4);
			p.RemoveCards(new List<Card>{ c4, c3 });
			Debug.Assert(p.GetCardCount() == 2);
			p.RemoveCards(new List<Card>{ c2 });
			Debug.Assert(p.GetCardCount() == 1);
			p.RemoveCards(new List<Card>{ c1 });
			Debug.Assert(p.GetCardCount() == 0);
		}
	}
}