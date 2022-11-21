using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class AttackSpeedPowerUp : PowerUp
    {
        public new string name = "Attack Speed Power up";
        public new string description = "Increases attack rate";
        public new DateTime experationTime = DateTime.MinValue;
        public new int remainingTime = 200;
        public AttackSpeedPowerUp((int, int) coord)
        {
            this.coordinates = coord;
        }
    }
}
