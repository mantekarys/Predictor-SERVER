using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class DamagePowerUp : PowerUp
    {
        public new string name = "Damage power up";
        public new string description = "Increases damage";
        public new int experationTime = 200;
        public new int remainingTime = 200;
        public DamagePowerUp((int, int) coord)
        {
            this.coordinates = coord;
        }
        public override void pickedUp(Class character)
        {
            character.ApplyPowerUp(this);
        }

    }
}
