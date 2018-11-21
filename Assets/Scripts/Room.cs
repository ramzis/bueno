namespace Tadget 
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// The room should completely be unaware of the rules of the game.
    /// It serves as a game->player tool which allows the game to manipulate data.
    /// Basically an interface for the game to interact with players without directly touching their data.
    public class Room : MonoBehaviour {
        
        [SerializeField] protected List<IPlayer> players;
        [SerializeField] protected List<PlayerView> views;
        [SerializeField] protected Game game; // TODO make interface
        public RoomEvents roomEvents;

        private IPlayer currentPlayer;
        private bool isMovingClockwise;
        private bool waitingForMove = false;
        private int currentPlayerIdx = 0;

        private void Start()
        {

        }
        
        private void OnValidate()
        {
            Debug.AssertFormat(roomEvents != null, "Missing RoomEvents @ {0}", this);	
        }


        protected void GivePlayerCards(string playerName, List<Card> cards)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null) 
            {
                player.GiveCards(cards);
                foreach(Card c in cards)
                {
                    // TODO: filter this for multiplayer
                    if(!player.isAI)
                    {
                        roomEvents.PlayerDrawCard(playerName, c);				
                    }
                    else 
                    {
                        roomEvents.PlayerAnonDraw(playerName);
                    }
                }
            }
            else 
            {
                Debug.LogErrorFormat("[ROOM] Unable to give Player {0} cards", playerName);
            }
        }

        protected void GivePlayerCards(string playerName, Card card)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null) 
            {
                player.GiveCards(card);
                // TODO: filter this for multiplayer
                if(!player.isAI)
                {
                    roomEvents.PlayerDrawCard(playerName, card);					
                }
                else 
                {
                    roomEvents.PlayerAnonDraw(playerName);
                }
            }
            else 
            {
                Debug.LogErrorFormat("[ROOM] Unable to give Player {0} cards", playerName);
            }
        }

        private int GetPlayerCardCount(string playerName)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null)
            {
                return player.GetCardCount();
            }
            else
            {
                Debug.LogErrorFormat("[ROOM] Unable to retrieve Player {0} card count", playerName);
                return -1;
            }
        }

        private bool CheckIfPlayerHasCards(string playerName, List<Card> cards) 
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null) {
                return player.HasCards(cards);
            }
            else 
            {
                Debug.LogErrorFormat("[ROOM] Unable to check if Player {0} has cards", playerName);
                return false;
            }
        }

        private void RemovePlayerCards(string playerName, List<Card> cards)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null) 
            {
                player.RemoveCards(cards);
                foreach(Card c in cards)
                {
                    roomEvents.PlayerDiscardCard(playerName, c);
                }
            }
            else 
            {
                Debug.LogErrorFormat("[ROOM] Unable to remove Player {0} cards", playerName);
            }
        }



        private void NotifyPlayerTurn(string playerName)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null)
            {
                // Debug.LogFormat("Notified {0} of turn through room", playerName);
                if(player.isLocal)
                {
                    if(!player.isAI)
                    {
                        PlayerView playerView = GetPlayerView(playerName);
                        if(playerView != null)
                        {
                            playerView.EnableView();
                        }
                        else
                        {
                            Debug.LogErrorFormat("[ROOM] Player {0} view not found", playerName);
                        }
                    }
                    player.NotifyTurn();
                }
                else
                {
                    // TODO: remote player
                    // remote.NotifyTurn(playerName)
                }
            }
            else 
            {
                Debug.LogErrorFormat("[ROOM] Unable to notify Player {0} of turn", playerName);
            }
        }

        private void NotifyTurnEnded(string playerName)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null)
            {
                // Debug.LogFormat("Notified {0} of turn end through room", playerName);
                if(player.isLocal)
                {
                    if(!player.isAI)
                    {
                        PlayerView playerView = GetPlayerView(playerName);
                        if(playerView != null)
                        {
                            playerView.DisableView();
                        }
                        else
                        {
                            Debug.LogErrorFormat("[ROOM] Player {0} view not found", playerName);
                        }
                    }
                }
                else
                {
                    // TODO: remote player
                    // remote.NotifyTurn(playerName)
                }
            }
            else 
            {
                Debug.LogErrorFormat("[ROOM] Unable to notify Player {0} of turn end", playerName);
            }
            
        }



        protected IPlayer GetPlayer(string playerName)
        {
            return players.Find(x => x.playerName == playerName);
        }

        protected PlayerView GetPlayerView(string playerName)
        {
            return views.Find(x => x.playerName == playerName);
        }

        protected IPlayer AddPlayer(string playerName, Game game, bool isAI, bool isLocal)
        {
            IPlayer player = IPlayer.Create(playerName, game, isAI, isLocal);
            players.Add(player);
            return player;
        }

        protected PlayerView AddView(string playerName, Game game)
        {
            PlayerView playerView = PlayerView.Create(playerName, game.GetTimePerTurn()); 
            views.Add(playerView);
            return playerView;
        }


        // Public API

        public bool Play()
        {
            if (GetPlayerCount() >= 2)
            {
                game.Play();
                return true;
            }
            else
            {
                Debug.LogWarningFormat("[ROOM] Not enough players to start game.");
                return false;
            }
        }

        public void Join(string playerName, bool isAI, bool isLocal)
        {
            if (players == null)
                players = new List<IPlayer>();
            IPlayer player = AddPlayer(playerName, null, isAI, isLocal); //TODO fix game ref in player to be room ref
            if (!isAI && isLocal)
            {
                PlayerView playerView = AddView(playerName, null); // fix same
                ((PlayerHuman)player).SetView(playerView);
                playerView.DisableView();
            }
            roomEvents.PlayerJoin(playerName);
            Debug.LogFormat("[ROOM] Player {0} joined", playerName);
        }

        public void RemovePlayer(string playerName) 
        {
            throw new System.NotImplementedException();
        }

        public void NextPlayer()
        {
            if (isMovingClockwise)
            {
                currentPlayerIdx = currentPlayerIdx + 1 < GetPlayerCount() ? currentPlayerIdx + 1 : 0;
            }
            else
            {
                currentPlayerIdx = currentPlayerIdx - 1 >= 0 ? currentPlayerIdx - 1 : GetPlayerCount() - 1;
            }

            //Debug.LogFormat("[GAME] Player {0} turn ended. ({1} Cards.)", GetCurrentPlayerName(), room.GetPlayerCardCount(GetCurrentPlayerName()));
        }

        public void TurnStarted()
        {
            NotifyPlayerTurn(currentPlayer.playerName);
        }

        public void TurnEnded()
        {
            NotifyTurnEnded(currentPlayer.playerName);
        }

        public void ReverseDirectionOfPlay()
        {
            isMovingClockwise = !isMovingClockwise;
        }

        public int GetPlayerCount()
        {
            if (players != null)
                return players.Count;
            else
                return 0;
        }

        public string GetCurrentPlayerName()
        {
            return currentPlayer.playerName;
        }

        public List<string> GetPlayerNames()
        {
            return players.ConvertAll(x => x.playerName);
        }


        // Card specific

        public int GetCurrentPlayerCardCount()
        {
            return GetPlayerCardCount(currentPlayer.playerName);
        }

        public void GiveCurrentPlayerCards(List<Card> cards)
        {
            GivePlayerCards(currentPlayer.playerName, cards);
        }

        public void RemoveCurrentPlayerCards(List<Card> cards)
        {
            RemovePlayerCards(currentPlayer.playerName, cards);
        }

        public void GiveCurrentPlayerCards(Card cards)
        {
            GivePlayerCards(currentPlayer.playerName, cards);
        }

        public void DiscardFromDeck(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                roomEvents.DeckDiscardCard(card);
            }
        }


    }
}
