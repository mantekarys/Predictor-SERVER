using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal class SpeedCreator : PickUpCreator
    {
        public override Item createItem((int,int) cord)
        {
            return new SpeedPotion(cord);
        }

        public override PowerUp createPowerUp((int,int)cord)
        {
            return new AttackSpeedPowerUp(cord);
        }
    }
}
