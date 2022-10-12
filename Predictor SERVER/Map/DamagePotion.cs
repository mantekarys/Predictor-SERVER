using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class DamagePotion : Item
    {
        public new string name = "Damage potion";
        public new string description = "Increases damage temporarly";
        public new int experationTime = 200;
        public new int remainingTime = 200;
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
