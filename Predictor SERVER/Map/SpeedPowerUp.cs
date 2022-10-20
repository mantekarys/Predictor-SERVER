using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class SpeedPowerUp : PowerUp
    {
        public new string name = "Speed Power up";
        public new string description = "Increases movement speed";
        public new DateTime experationTime = DateTime.MinValue;
        public new int remainingTime = 200;
        public SpeedPowerUp((int, int) coord)
        {
            this.coordinates = coord;
        }
        public override void pickedUp(Class character)
        {
            character.applyPowerUp(this);
        }
    }
}
