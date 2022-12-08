using Predictor_SERVER.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Server
{
    public class Match
    {
        public int id;
        public DateTime date;
        public string name;
        public int peopleAmount = 1;
        public int ready = 0;
        public List<Player> players;
        public int state = 0;
        public int ticks = 0;
        
        public Match(int id, string name)
        {
            this.name = name;
            this.id = id;
            this.players = new List<Player>();
        }

        public void start()
        {
            this.date = DateTime.Now;
        }
        public void end()
        {

        }

        public int getState()
        {
            if (state > 2)
            {
                state = 0;
            }
            return state++;
        }
        public bool getTick()
        {
            ticks++;
            if (ticks > 250)
            {
                ticks = 0;
                return true;
            }
            return false;
        }
        public void Accept(Visitor visitor)
        {
            foreach (Player p in players)
            {
                p.playerClass.Accept(visitor);
            }
        }

        public List<Player> CopyPlayers()
        {
            List<Player> otherPlayers = new List<Player>();
            foreach (Player p in this.players)
            {
                otherPlayers.Add(p.Clone());
            }
            return otherPlayers;
        }
    }
}
