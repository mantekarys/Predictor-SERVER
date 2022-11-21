using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Predictor_SERVER.Map
{
    internal class SpeedPotion : Item
    {
        public new string name = "Speed potion";
        public new string description = "Increases movement speed temporarly";
        public new DateTime experationTime = DateTime.MinValue;
        public new int remainingTime = 200;
        public SpeedPotion((int, int) position)
        {
            coordinates = position;
        }
    }
}
