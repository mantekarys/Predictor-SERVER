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
        public new DateTime experationTime = DateTime.MinValue;
        public new int remainingTime = 200;
        public DamagePotion((int, int) position)
        {
            coordinates = position;
        }
    }
}
