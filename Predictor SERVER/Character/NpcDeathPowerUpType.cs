using Predictor_SERVER.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal class NpcDeathPowerUpType: NpcDeath
    {
        public override PickUp onDeath((int,int) coordinates)
        {
            Random random = new Random();
            PickUpCreator _pickUpCreator = PickUpCreator.PickUpCreate(random.Next(3));
            return _pickUpCreator.createPowerUp(coordinates);
        }
    }
}
