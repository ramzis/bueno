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
            AddPlayer(playerName, game, isAI, isLocal);
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
        }

        public void NotifyPlayerTurn(string playerName)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null)
            {
                //Debug.LogFormat("Notified {0} of turn through room", playerName);
                player.NotifyTurn();
            }
            if(!player.isAI)
            {
                PlayerView view = GetPlayerView(playerName);
                
                if(view != null)
                {
                    view.EnableView();
                }
            }
        }

        public void NotifyTurnEnded(string playerName)
        {
            IPlayer player = GetPlayer(playerName);
            if(player != null)
            {

            }
            PlayerView view = GetPlayerView(playerName);
            if(view != null)
            {
                view.DisableView();
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

        protected void AddPlayer(string playerName, Game game, bool isAI, bool isLocal)
        {
            players.Add(IPlayer.Create(playerName, game, isAI, isLocal));
        }

        protected void AddView(string playerName, Game game)
        {
            views.Add(PlayerView.Create(playerName, game.GetTimePerTurn()));
        }

        protected void RemovePlayer(string playerName) 
        {
            throw new System.NotImplementedException();
        }
    }
}
