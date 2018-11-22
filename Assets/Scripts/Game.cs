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
        [SerializeField] protected StateMachine state;

        protected int pendingCardsToDraw = 0;
        protected bool isNextPlayerSkipped = false;
        protected bool didPlayerRequestDraw = false;

        private void Start()
        {
            state.Set("STATE_Null");
            StartCoroutine(StateMachine());
        }

        protected IEnumerator StateMachine()
        {
            while(true)
            {
                switch (state.Get())
                {
                    case "STATE_NewGame":
                    {
                        state.Set("STATE_InitVars");
                        break;
                    }
                    case "STATE_InitVars":
                    {
                        pendingCardsToDraw = 0;
                        isNextPlayerSkipped = false;
                        didPlayerRequestDraw = false;
                        state.Set("STATE_InitDeck");
                        break;
                    }
                    case "STATE_InitDeck":
                    {
                        deck.Reset();
                        state.Set("STATE_DealStartingHands");
                        break;
                    }
                    case "STATE_DealStartingHands":
                    {
                        StartCoroutine(DealStartingHands());
                        break;
                    }
                    case "STATE_DrawFirstCard":
                    {
                        DrawFirstCard();
                        state.Set("STATE_StartGame");
                        break;
                    }
                    case "STATE_StartGame":
                    {
                        StartCoroutine(GameLoop());
                        state.Set("STATE_GameRunning");
                        break;
                    }
                    case "STATE_GameRunning":
                        break;
                    case "STATE_GameRunning_WaitingForMove":
                        break;
                    case "STATE_GameRunning_MakingMove":
                        break;
                    case "STATE_GameRunning_ResolvingMoveEffects":
                        break;
                    case "STATE_Null":
                        break;
                    default:
                        Debug.LogFormat("[GAME] Unknown state: {0}", state);
                        break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        protected IEnumerator DealStartingHands()
        {
            for (int i = 0; i < gameSettings.drawGameStartCardCount; i++)
            {
                foreach (string player in room.GetPlayerNames())
                {
                    //room.GivePlayerCards(player, deck.Draw(1));
                    room.GiveCurrentPlayerCards(deck.Draw(1));
                    yield return new WaitForSeconds(gameSettings.cardDealDelay);
                }
            }
            state.Set("STATE_DrawFirstCard");
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
                //Debug.LogFormat("[GAME] Player {0} turn. ({1} Cards.)", GetCurrentPlayerName(), room.GetPlayerCardCount(GetCurrentPlayerName()));
                // Debug.LogFormat("[GAME] Current Card: {0}", GetCurrentCard());

                // Reset single draw request
                didPlayerRequestDraw = false;

                // Draw pending cards
                if (pendingCardsToDraw > 0)
                {
                    //Debug.LogFormat("[GAME] Player {0} drawing {1} pending cards.", GetCurrentPlayerName(), pendingCardsToDraw);
                    room.GiveCurrentPlayerCards(deck.Draw(pendingCardsToDraw));
                    pendingCardsToDraw = 0;
                }

                // Skip turn if necessary
                if (isNextPlayerSkipped)
                {
                    //Debug.LogFormat("[GAME] Player {0} turn skipped.", GetCurrentPlayerName());
                    Debug.LogFormat("[GAME] Player {0} turn skipped.", room.GetCurrentPlayerName());
                    isNextPlayerSkipped = false;
                }
                else
                {
                    // Notify player of turn and wait for response.
                    Coroutine c = StartCoroutine(TimeOutMove());
                    yield return new WaitWhile(() => state.Get() == "STATE_GameRunning_WaitingForMove");
                    StopCoroutine(c);

                    room.TurnEnded();

                    // Check for win condition.
                    if(room.GetCurrentPlayerCardCount() == 0)
                    {
                        Debug.LogFormat("[GAME] Player {0} wins!", room.GetCurrentPlayerName());
                        yield break;
                    }
                    
                }

                // Move to next player.
                room.NextPlayer();

                yield return new WaitForSeconds(gameSettings.afterTurnDelay);
                yield return new WaitForEndOfFrame();
            }
        }

        protected IEnumerator TimeOutMove()
        {
            //var _currPlayer = currentPlayerIdx;
            //waitingForMove = true;
            state.Set("STATE_GameRunning_WaitingForMove");
            room.TurnStarted();
            yield return null;
            /*yield return new WaitForSecondsRealtime(gameSettings.timePerTurn);
            if(currentPlayerIdx == _currPlayer && waitingForMove)
            {
                Debug.LogFormat("[GAME] Player {0} turn timed out.", GetCurrentPlayerName());
                waitingForMove = false;
                RequestDraw(GetCurrentPlayerName());
            }*/
        }

        protected void ResolveMoveEffects(List<Card> cards) 
        {
            state.Set("STATE_GameRunning_ResolvingMoveEffects");
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
                        if (room.GetPlayerCount() > 2)
                            room.ReverseDirectionOfPlay();
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
            // TODO: move this to room
            // Ensure player has these cards
            //if(!room.CheckIfPlayerHasCards(playerName, cards) || cards.Count < 1) 
            //{
            //    return false;
            //}

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
            room.RemoveCurrentPlayerCards(cards);
            var cs = string.Join(", ", cards.ConvertAll(x => x.ToString()).ToArray());
            Debug.LogFormat("--Player {0} played: {1}", playerName, cs);
            return true;
        }

        // Public API

        public void MakeMove(string playerName, List<Card> cards)
        {
            /* TODO: move to room
            if (!waitingForMove)
            {
                Debug.LogWarningFormat("[GAME] Unexpected Player {0} move.", playerName);
                return;
            }
            */

            // waitingForMove = false;

            state.Set("STATE_GameRunning_MakingMove");

            if (cards != null && cards.Count > 0)
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
            /*if(GetCurrentPlayerName() == playerName && */ // TODO: Move to room
            if(!didPlayerRequestDraw)
            {
                Debug.LogFormat("[GAME] Player {0} requested draw.", playerName);
                didPlayerRequestDraw = true;
                room.GiveCurrentPlayerCards(deck.Draw(1));
            }
            else
            {
                Debug.LogWarningFormat("[GAME] Invalid draw request from player {0}", playerName);
            }
        }

        public void SayUno(string playerName)
        {
            if (state.Get() != "STATE_GameRunning_WaitingForMove")
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

        public float GetTimePerTurn()
        {
            return gameSettings.timePerTurn;
        }

        public void Play()
        {
            if(state.Get() == "STATE_Null")
            {
                state.Set("STATE_NewGame");
            }
            else
            {
                Debug.LogFormat("[GAME] Can't start game from state {0}.", state);
            }
        }
    }
}
