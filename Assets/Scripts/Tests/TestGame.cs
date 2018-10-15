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
    public class TestGame : Game {
    /* 
        protected override void Start()
        {

        }

        [SetUp]
        public void Init()
        {
            deck = null;
            discard = null;
            players = null;
            gameSettings = null;
            currPlayerIdx = 0;
            playerCount = 0;
            waitingForMove = false;
            isMovingClockwise = false;
            pendingCardsToDraw = 0;
            isNextPlayerSkipped = false;
        }

        private void InitGameSettings()
        {
            GameSettings gs = ScriptableObject.CreateInstance<GameSettings>();
            gameSettings = gs;
        }

        [Test]
        public void PlayersJoinGame()
        {
            Debug.Assert(playerCount == 0);
            PlayerJoin("Aldona");
            Debug.Assert(playerCount == 1);
            Debug.Assert(players.Exists(p => p.name == "Aldona"));
            PlayerJoin("Tadas");
            Debug.Assert(playerCount == 2);
            Debug.Assert(players.Exists(p => p.name == "Tadas"));
        }

        [Test]
        public void DeckIsInstantiated()
        {
            InitDeck();
            Debug.Assert(deck.Count == 108);
        }

        [Test]
        public void CardsAreDrawn()
        {
            // Should throw error when deck is not instantiated.
            List<Card> cards = DrawCards(0);
            LogAssert.Expect(LogType.Error, "Deck is not available.");
            Debug.Assert(cards.Count == 0);

            deck = new List<Card>();

            // Should throw error when discard is not instantiated.
            cards = DrawCards(1);
            LogAssert.Expect(LogType.Error, "Discard is not available.");
            Debug.Assert(cards.Count == 0);

            InitDiscard();

            // Should not be able to draw when deck and discard are empty.
            cards = DrawCards(1);
            LogAssert.Expect(LogType.Warning, "Not enough cards to draw.");
            Debug.Assert(cards.Count == 0);

            // Should not be able to draw when deck is empty and discard has only one card to merge.
            Card c0 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1);
            discard.Add(c0);
            Debug.Assert(discard.Count == 1);
            cards = DrawCards(1);
            LogAssert.Expect(LogType.Warning, "Not enough cards to draw.");
            Debug.Assert(cards.Count == 0);

            // Should be able to draw when deck is empty and discard has more than 1 card to merge.
            Card c1 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);
            discard.Add(c1);
            Debug.Assert(discard.Count == 2);
            cards = DrawCards(1);
            Debug.Assert(cards.Count == 1);
            Debug.Assert(cards.Contains(c0));
            Debug.Assert(discard.Count == 1);
            Debug.Assert(discard.Contains(c1));
        }

        [Test]
        public void CardsAreDrawnFromFullDeck()
        {
            InitDeck();
            InitDiscard();
            List<Card> cards = new List<Card>();

            // Should be able to draw when deck is not empty.
            // Should not merge discard if there are enough cards to draw.
            LogAssert.NoUnexpectedReceived();
            Debug.Assert(deck.Count == 108);
            cards = DrawCards(2);
            Debug.Assert(cards.Count == 2);
            Debug.Assert(deck.Count == 106);
        }

        [Test]
        public void CardsAreDrawnUniquely()
        {
            InitDeck();
            InitDiscard();
            List<Card> cards = new List<Card>();
            List<Card> _deck = new List<Card>(deck);
            var deckCount = deck.Count;
            for (int i = 0; i < deckCount; i++)
            {
                cards.AddRange(DrawCards(1));   
            }
            _deck.Reverse();
            Debug.Assert(cards.SequenceEqual(_deck));

        }

        [Test]
        public void CardsAreMergedWithNonEmptyDeck()
        {
            deck = new List<Card>();
            InitDiscard();
            List<Card> cards = new List<Card>();

            Card c1 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);
            Card c2 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._2, Card.Color._1);
            Card c3 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._3, Card.Color._1);
            Card c4 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._4, Card.Color._1);

            deck.Add(c1);
            discard.Add(c2);
            discard.Add(c3);
            discard.Add(c4);

            LogAssert.Expect(LogType.Log, "Merging discard and draw piles.");
            cards = DrawCards(3);
            Debug.Assert(discard.Count == 1);
            Debug.Assert(cards.Count == 3);

        }

        [Test]
        public void DrawnCardsAreRemovedFromDeck()
        {
            deck = new List<Card>();
            List<Card> cards = new List<Card>();
            InitDiscard();
            
            Card c1 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);
            Card c2 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._2, Card.Color._1);
            Card c3 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._3, Card.Color._1);
            Card c4 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._4, Card.Color._1);

            deck.Add(c1);
            deck.Add(c2);
            deck.Add(c3);
            deck.Add(c4);

            Debug.Assert(deck.Contains(c1));
            Debug.Assert(deck.Contains(c2));
            Debug.Assert(deck.Contains(c3));
            Debug.Assert(deck.Contains(c4));

            var deckCount = deck.Count;
            
            cards = DrawCards(0);
            Debug.Assert(deck.Count == deckCount);

            cards = DrawCards(deckCount);
            Debug.Assert(deck.Count == 0);
            Debug.Assert(cards.Contains(c1));
            Debug.Assert(cards.Contains(c2));
            Debug.Assert(cards.Contains(c3));
            Debug.Assert(cards.Contains(c4));

            Debug.Assert(!deck.Contains(c1));
            Debug.Assert(!deck.Contains(c2));
            Debug.Assert(!deck.Contains(c3));
            Debug.Assert(!deck.Contains(c4));
            
            cards.Clear();

        }

        [Test]
        public void GameSettingsAvailable()
        {
            InitGameSettings();
            Debug.Assert(gameSettings != null);
        }

        [Test]
        public void StartingHandsDealt()
        {
            PlayerJoin("Tadas");
            PlayerJoin("Aldona");

            InitDeck();
            var deckCount = deck.Count;
            InitDiscard();
            InitGameSettings();
            DealStartingHands();

            foreach(Player p in players)
            {
                Debug.Assert(p.hand.Count == gameSettings.drawGameStartCardCount);
            }
            Debug.Assert(deck.Count == deckCount - (players.Count * gameSettings.drawGameStartCardCount));
        }

        [Test]
        public void FirstCardDrawn()
        {
            deck = new List<Card>();
            InitDiscard();

            Card c1 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);
            deck.Add(c1);

            Debug.Assert(deck.Contains(c1));
            Debug.Assert(discard.Count == 0);
            DrawFirstCard();
            Debug.Assert(discard.Contains(c1));
            Debug.Assert(!deck.Contains(c1));

        }

        [Test]
        public void PlayerReceivesDrawnCards()
        {
            InitDeck();
            InitDiscard();

            PlayerJoin("Tadas");
            Player p = players[0];

            Debug.Assert(p.hand.Count == 0);
            RequestDraw(players[0]);
            Debug.Assert(p.hand.Count == 1);
        }

        [Test]
        public void GetsLastPlayedCard()
        {
            InitDiscard();

            Card c1 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);
            Card c2 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._2, Card.Color._1);
            Card c3 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._3, Card.Color._1);

            Debug.Assert(GetCurrentCard() == null);
            discard.Add(c1);
            Debug.Assert(GetCurrentCard() == c1);
            discard.Add(c2);
            discard.Add(c3);
            Debug.Assert(GetCurrentCard() == c3);
        }

        [Test]
        public void ValidMoves()
        {
            PlayerJoin("Tester");
            Player p = players[0];

            InitDiscard();
            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));

            // Valid same card move
            var c0 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1);
            p.hand = new List<Card>() { c0 };
            Debug.Assert(
                AttemptMove(p, new List<Card>()
                {
                    c0
                })
            );

            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));

            // Valid same type move
            var c1 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._2);
            p.hand = new List<Card>() { c1 };
            Debug.Assert(
                AttemptMove(p, new List<Card>()
                {
                    c1
                })
            );

            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));

            // Valid same color move
            var c2 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);
            p.hand = new List<Card>() { c2 };
            Debug.Assert(
                AttemptMove(p, new List<Card>()
                {
                    c2
                })
            );

            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));
            
            // Valid Wild card move
            var c3 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._Wild, Card.Color._4);
            p.hand = new List<Card>() { c3 };
            Debug.Assert(
                AttemptMove(p, new List<Card>()
                {
                    c3
                })
            );

            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));

            // Valid Wild Draw Four card move
            var c4 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._WildDrawFour, Card.Color._4);
            p.hand = new List<Card>() { c4 };            
            Debug.Assert(
                AttemptMove(p, new List<Card>()
                {
                    c4
                })
            );

            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));

            // Valid with multiple same cards
            var c5 = new List<Card>()
            {
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1),
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1),
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1)
            };
            p.hand.AddRange(c5);   
            Debug.Assert(
                AttemptMove(p, c5)
            );

            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));

            // Valid with multiple same type different color
            var c6 = new List<Card>()
            {
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1),
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._3),
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._4)
            };
            p.hand.AddRange(c6);  
            Debug.Assert(
                AttemptMove(p, c6)
            );

            discard.Add(ScriptableObject.CreateInstance<Card>().Init(Card.Type._0, Card.Color._1));

            // Not valid with mixed cards
            var c7 = new List<Card>()
            {
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._2, Card.Color._1),
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._2, Card.Color._1),
                ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1)
            };
            p.hand.AddRange(c7);  
            Debug.Assert( 
                !AttemptMove(p, c7)
            );
        }

        [Test]
        public void HandContainsCorrectCards()
        {
            PlayerJoin("Tester");
            var p = players[0];

            Card c1 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);
            Card c2 = ScriptableObject.CreateInstance<Card>().Init(Card.Type._1, Card.Color._1);

            p.hand.Add(c1);

            Debug.Assert(p.hand.Contains(c1));
            Debug.Assert(!p.hand.Contains(c2));

        }*/
    }
}
