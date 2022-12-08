using Predictor_SERVER.Character;
using Predictor_SERVER.Map;
using System;
using System.Collections.Generic;

namespace Predictor_SERVER.Server
{
    public struct MemState
    {
        public int matchId { get; set; }
        public List<Player> players { get; set; }
        private List<Player> CopyPlayers()
        {
            List<Player> otherPlayers = new List<Player>();
            foreach (Player p in players)
            {
                otherPlayers.Add(p.Clone());
            }
            return otherPlayers;
        }
        public MemState Clone()
        {
            MemState otherMemState = (MemState)this.MemberwiseClone();
            otherMemState.players = this.CopyPlayers();
            return otherMemState;
        }
    }
    internal class Memento
    {
        MemState memState;

        public Memento(int MacthId, List<Player> Players)
        {
            memState.matchId = MacthId;
            memState.players = Players;
        }

        public MemState GetState()
        {
            return memState.Clone();
        }

        public int GetMatchId()
        {
            return memState.matchId;
        }
        

    }
}
