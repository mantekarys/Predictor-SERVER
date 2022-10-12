using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class SpeedPowerUp : PowerUp
    {
        public SpeedPowerUp((int, int) coord)
        {
            this.coordinates = coord;
        }
        public override void pickedUp(Class character)
        {
            character.ApplyPowerUp(this);
        }
    }
}
