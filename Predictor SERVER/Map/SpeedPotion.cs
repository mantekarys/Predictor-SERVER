using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class SpeedPotion : Item
    {
        public override void Use()
        {

        }
        public override void pickedUp(Class character)
        {
            character.AddToInventory(this);
        }

        public SpeedPotion((int, int) position)
        {
            coordinates = position;
        }
    }
}
