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

        protected List<string> playerNames;
        protected int currentPlayerIdx = 0;
        protected bool waitingForMove = false;
        protected bool isMovingClockwise = true;
        protected int pendingCardsToDraw = 0;
        protected bool isNextPlayerSkipped = false;
        protected bool didPlayerRequestDraw = false;
        protected bool startingHandsDealt = false;

        protected virtual void Start()
        {
            JoinGame("Tadas", false, true);
            JoinGame("Aldona", true, true);

            StartCoroutine(StartGame());
        }

        protected IEnumerator StartGame()
        {
            if(room.GetPlayerCount() < 2)
            {
                Debug.LogErrorFormat("[GAME] Not enough players to start game.");
                yield break;
            }

            playerNames = room.GetPlayerNames();
            currentPlayerIdx = 0;
            waitingForMove = false;
            isMovingClockwise = true;
            pendingCardsToDraw = 0;
            isNextPlayerSkipped = false;
            didPlayerRequestDraw = false;
            startingHandsDealt = false;
            
            deck.Reset();

            StartCoroutine(DealStartingHands());

            yield return new WaitUntil(() => startingHandsDealt);

            StartCoroutine(GameLoop());

            yield return null;
        }

        protected IEnumerator DealStartingHands()
        {
            for (int i = 0; i < gameSettings.drawGameStartCardCount; i++)
            {
                foreach (string player in room.GetPlayerNames())
                {
                    room.GivePlayerCards(player, deck.Draw(1));
                    yield return new WaitForSeconds(gameSettings.cardDealDelay);
                }
            }
            DrawFirstCard();
            startingHandsDealt = true;
            yield return null;
        }

        protected void DrawFirstCard()
        {
            List<Card> first = deck.Draw(1);
            ResolveMoveEffects(first);
            deck.Discard(first);
            room.DiscardFromDeck(first);
        }

        protected IEnumerator GameLoop()
        {
            Debug.Log("[GAME] Game loop started.");

            while(true)
            {
                Debug.LogFormat("[GAME] Player {0} turn. ({1} Cards.)", GetCurrentPlayerName(), room.GetPlayerCardCount(GetCurrentPlayerName()));
                // Debug.LogFormat("[GAME] Current Card: {0}", GetCurrentCard());

                // Reset single draw request
                didPlayerRequestDraw = false;

                // Draw pending cards
                if (pendingCardsToDraw > 0)
                {
                    Debug.LogFormat("[GAME] Player {0} drawing {1} pending cards.", GetCurrentPlayerName(), pendingCardsToDraw);
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
                    Debug.LogFormat("[GAME] Player {0} turn ended. ({1} Cards.)", GetCurrentPlayerName(), room.GetPlayerCardCount(GetCurrentPlayerName()));
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
            if(!room.CheckIfPlayerHasCards(playerName, cards) || cards.Count < 1) 
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
            var cs = string.Join(", ", cards.ConvertAll(x => x.ToString()).ToArray());
            Debug.LogFormat("--Player {0} played: {1}", playerName, cs);
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

            if(cards != null && cards.Count > 0)
            {
                if(AttemptMove(playerName, cards))
                {
                    ResolveMoveEffects(cards);
                }
                else 
                {
                    Debug.LogWarningFormat("[GAME] Player {0} provided invalid move.", playerName);
                    // Draw after invalid or empty move if player hasn't already done that.
                    if(!didPlayerRequestDraw)
                        RequestDraw(playerName);
                }
            }
            else
            {
                Debug.LogFormat("[GAME] Player {0} provided empty move.", playerName);
                // Draw after invalid or empty move if player hasn't already done that.
                if(!didPlayerRequestDraw)
                    RequestDraw(playerName);
            }
        }

        public void RequestDraw(string playerName)
        {
            if(GetCurrentPlayerName() == playerName && !didPlayerRequestDraw)
            {
                Debug.LogFormat("[GAME] Player {0} requested draw.", playerName);
                didPlayerRequestDraw = true;
                room.GivePlayerCards(playerName, deck.Draw(1));
            }
            else
            {
                Debug.LogWarningFormat("[GAME] Invalid draw request from player {0}", playerName);
            }
        }

        public void SayUno(string playerName)
        {
            if (!waitingForMove)
            {
                Debug.LogWarningFormat("[GAME] Unexpected Player {0} UNO.", playerName);
            }
            else 
            {
                Debug.LogFormat("[GAME] Player {0} says UNO!", playerName);
            }
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

        public void JoinGame(string playerName, bool isAI, bool isLocal) 
        {
            room.Join(playerName, this, isAI, isLocal);
        }

        public float GetTimePerTurn()
        {
            return gameSettings.timePerTurn;
        }
    }
}
