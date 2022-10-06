using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal class SpeedCreator : PickUpCreator
    {
        public override Item createItem()
        {
            return new SpeedPotion();
        }

        public override PowerUp createPowerUp()
        {
            return new AttackSpeedPowerUp();
        }
    }
}
