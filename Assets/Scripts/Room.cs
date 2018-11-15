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
        public RoomEvents roomEvents;
        
        private void OnValidate()
        {
            Debug.AssertFormat(roomEvents != null, "Missing RoomEvents @ {0}", this);	
        }

        public void Join(string playerName, Game game, bool isAI, bool isLocal)
        {
            if(players == null)
                players = new List<IPlayer>();
            IPlayer player = AddPlayer(playerName, game, isAI, isLocal);
            if(!isAI && isLocal) 
            {
                PlayerView playerView = AddView(playerName, game);
                ((PlayerHuman) player).SetView(playerView);
                playerView.DisableView();
            }
            roomEvents.PlayerJoin(playerName);
            Debug.LogFormat("[ROOM] Player {0} joined", playerName);
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

        public void GivePlayerCards(string playerName, Card card)
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

        public List<string> GetPlayerNames() 
        {
            return players.ConvertAll(x => x.playerName);
        }

        public int GetPlayerCardCount(string playerName)
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

        public bool CheckIfPlayerHasCards(string playerName, List<Card> cards) 
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

        public void RemovePlayerCards(string playerName, List<Card> cards)
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

        public void DiscardFromDeck(List<Card> cards)
        {
            foreach(Card card in cards)
            {
                roomEvents.DeckDiscardCard(card);
            }
        }

        public void NotifyPlayerTurn(string playerName)
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

        public void NotifyTurnEnded(string playerName)
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

        protected void RemovePlayer(string playerName) 
        {
            throw new System.NotImplementedException();
        }
    }
}
