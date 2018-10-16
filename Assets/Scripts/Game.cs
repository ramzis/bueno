namespace Tadget
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using Random = UnityEngine.Random;

    public class Game : MonoBehaviour
    {
        [SerializeField] protected Deck deck;
        [SerializeField] protected Room room;
        [SerializeField] protected GameSettings gameSettings;
        [SerializeField] protected GameEvents gameEvents;

        protected List<string> playerNames;
        protected int currentPlayerIdx;
        protected bool waitingForMove;
        protected bool isMovingClockwise;
        protected int pendingCardsToDraw = 0;
        protected bool isNextPlayerSkipped = false;

        protected virtual void Start()
        {
            JoinGame("Tadas");
            JoinGame("Aldona");
            playerNames = room.GetPlayerNames();
            deck.Reset();
            DealStartingHands();
            DrawFirstCard();
            StartCoroutine(GameLoop());
        }

        protected void DealStartingHands()
        {
            for (int i = 0; i < gameSettings.drawGameStartCardCount; i++)
            {
                foreach (string player in room.GetPlayerNames())
                {
                    room.GivePlayerCards(player, deck.Draw(1));
                }
            }
        }

        protected void DrawFirstCard()
        {
            List<Card> first = deck.Draw(1);
            // TODO: resolve first card draw effects.
            deck.Discard(first);
        }

        protected IEnumerator GameLoop()
        {
            Debug.Log("[GAME] Game loop started.");
            
            if(room.GetPlayerCount() < 2)
            {
                Debug.LogErrorFormat("[GAME] Not enough players to start game.");
                yield break;
            }

            while(true)
            {
                // Debug.LogFormat("[GAME] Player {0} turn. ({1} Cards.)", players[currPlayerIdx].name, players[currPlayerIdx].hand.Count);
                // Debug.LogFormat("[GAME] Current Card: {0}", GetCurrentCard());

                // Draw pending cards
                if (pendingCardsToDraw > 0)
                {
                    // Debug.LogFormat("[GAME] Player {0} drawing {1} pending cards.", players[currPlayerIdx].name, pendingCardsToDraw);
                    room.GivePlayerCards(GetCurrentPlayerName(), deck.Draw(pendingCardsToDraw));
                    pendingCardsToDraw = 0;
                }

                // Skip turn if necessary
                if (isNextPlayerSkipped)
                {
                    Debug.LogFormat("[GAME] Player {0} turn skipped.", GetCurrentPlayerName());
                    isNextPlayerSkipped = false;
                }
                else
                {
                    // Notify player of turn and wait for response.
                    Coroutine c = StartCoroutine(TimeOutMove());
                    yield return new WaitWhile(() => waitingForMove);
                    StopCoroutine(c);

                    // Check for win condition.
                    if(room.GetPlayerCardCount(GetCurrentPlayerName()) == 0)
                    {
                        Debug.LogFormat("[GAME] Player {0} wins!", GetCurrentPlayerName());
                        yield break;
                    }

                    // Debug.LogFormat("[GAME] Player {0} turn ended.", GetCurrentPlayer().name);
                }

                // Move to next player.
                if (isMovingClockwise)
                {
                    currentPlayerIdx = currentPlayerIdx + 1 < room.GetPlayerCount() ? currentPlayerIdx + 1 : 0;
                }
                else
                {
                    currentPlayerIdx = currentPlayerIdx - 1 >= 0 ? currentPlayerIdx - 1 : room.GetPlayerCount() - 1;
                }

                yield return new WaitForSeconds(gameSettings.afterTurnDelay);
                yield return new WaitForEndOfFrame();
            }
        }

        protected IEnumerator TimeOutMove()
        {
            var _currPlayer = currentPlayerIdx;
            waitingForMove = true;
            room.NotifyPlayerTurn(GetCurrentPlayerName());
            yield return new WaitForSecondsRealtime(gameSettings.timePerTurn);
            if(currentPlayerIdx == _currPlayer && waitingForMove)
            {
                Debug.LogFormat("[GAME] Player {0} turn timed out.", GetCurrentPlayerName());
                waitingForMove = false;
                RequestDraw(GetCurrentPlayerName());
            }
        }

        protected void ResolveMoveEffects(List<Card> cards) 
        {
            foreach(Card c in cards)
            {
                switch (c.type)
                {
                    case Card.Type._0:
                        break;
                    case Card.Type._1:
                        break;
                    case Card.Type._2:
                        break;
                    case Card.Type._3:
                        break;
                    case Card.Type._4:
                        break;
                    case Card.Type._5:
                        break;
                    case Card.Type._6:
                        break;
                    case Card.Type._7:
                        break;
                    case Card.Type._8:
                        break;
                    case Card.Type._9:
                        break;
                    case Card.Type._Skip:
                        isNextPlayerSkipped = true;
                        break;
                    case Card.Type._Reverse:
                        if(room.GetPlayerCount() > 2)
                            isMovingClockwise = !isMovingClockwise;
                        else
                            isNextPlayerSkipped = true;
                        break;
                    case Card.Type._DrawTwo:
                        pendingCardsToDraw += gameSettings.drawPlusTwoCardCount;
                        break;
                    case Card.Type._Wild:
                        break;
                    case Card.Type._WildDrawFour:
                        pendingCardsToDraw += gameSettings.drawWildCardCount;
                        break;
                    default:
                        break;
                }
            }
        }

        protected bool AttemptMove(string playerName, List<Card> cards)
        {
            // Ensure player has these cards
            if(!room.CheckIfPlayerHasCards(playerName, cards)) 
            {
                return false;
            }

            cards.Insert(0, GetCurrentCard());
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if(ValidateMove(cards[i], cards[i + 1]))
                {
                    // If more than one card is played (excluding current card), they must be the same type except for Wild cards.
                    if(i > 0 && cards.Count > 2)
                    {
                        if(cards[i].type != cards[i + 1].type || (cards[i].type == Card.Type._Wild && cards[i + 1].type == Card.Type._Wild)
                            || (cards[i].type == Card.Type._WildDrawFour && cards[i + 1].type == Card.Type._WildDrawFour))
                        {
                            Debug.LogFormat("[GAME] Player {0} move contains invalid card sequence.", playerName);
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            cards.RemoveAt(0);
            deck.Discard(cards);
            room.RemovePlayerCards(playerName, cards);
            return true;
        }

        // Public API

        public void MakeMove(string playerName, List<Card> cards)
        {
            if (!waitingForMove)
            {
                Debug.LogWarningFormat("[GAME] Unexpected Player {0} move.", playerName);
                return;
            }

            waitingForMove = false;
            if(AttemptMove(playerName, cards))
            {
                ResolveMoveEffects(cards);
            }
            else 
            {
                Debug.LogWarningFormat("[GAME] Player {0} provided invalid move.", playerName);
                RequestDraw(playerName);
            }
        }

        public void RequestDraw(string playerName)
        {
            // Debug.LogFormat("[GAME] Player {0} drawing 1 card.", player.name);
            if(GetCurrentPlayerName() == playerName)
                room.GivePlayerCards(playerName, deck.Draw(1));
            else
                Debug.LogWarningFormat("[GAME] Invalid draw request from player {0}", playerName);
        }

        public bool ValidateMove(Card bot, Card top)
        {
            if (top.color == bot.color) return true;
            if (top.type == bot.type) return true;
            if (top.type == Card.Type._Wild) return true;
            if (top.type == Card.Type._WildDrawFour) return true;
            
            return false;
        }

        public Card GetCurrentCard()
        {
            return deck.GetCurrentCard();
        }

        public string GetCurrentPlayerName()
        {
            return playerNames[currentPlayerIdx];
        }

        public void JoinGame(string playerName) 
        {
            room.Join(playerName, this, true);
            gameEvents.PlayerJoin(name);
        }
    }
}
