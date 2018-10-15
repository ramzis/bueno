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
	public class TestPlayer : Player {
		
		[SetUp]
        public void Init()
        {
			playerName = null;
			hand = null;
			game = null;
			activeTurn = false;
			isAI = false;
        }
	
		[Test]
		public void PlayerIsCreated()
		{
			/*
			Debug.Assert(playerName == null);
			Debug.Assert(hand == null);
			Debug.Assert(game == null);
			Debug.Assert(activeTurn == false);
			Debug.Assert(isAI == false);

			Player a = Init("Aldona", null, true);

			Debug.Assert(a == this);
			Debug.Assert(playerName == "Aldona");
			Debug.Assert(hand != null);
			Debug.Assert(hand.Count == 0);
			Debug.Assert(game == null);
			Debug.Assert(activeTurn == false);
			Debug.Assert(isAI == true);
			*/
		}

		[Test]
		public void CardIsReceived()
		{

		}

		[Test]
		public void CardsAreReceived()
		{
			
		}

		[Test]
		public void HasCardsCheckIsCorrect()
		{

		}

		[Test]
		public void PlayerRemovesCards()
		{
				
		}

		[Test]
		public void CardCountCheckIsCorrect()
		{
			
		}
	}
}