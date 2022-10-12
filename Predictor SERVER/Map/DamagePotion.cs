using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class DamagePotion : Item
    {
        public override void Use()
        {

        }
        public override void pickedUp(Class character)
        {
            character.AddToInventory(this);
        }

        public DamagePotion((int, int) position)
        {
            coordinates = position;
        }
    }
}
