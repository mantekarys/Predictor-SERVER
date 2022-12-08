using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Server
{
    public class Player
    {
        public string userName;
        public int score = 0;
        public Class playerClass;
        public Player()
        {
        }
        public Player(Class c, string userName = "Bill")
        {
            playerClass = c;
            this.userName = userName;
        }
        public Player Clone()
        {
            Player otherPlayer = (Player)this.MemberwiseClone();
            otherPlayer.playerClass = playerClass.Clone();
            return otherPlayer;
        }
    }
}
