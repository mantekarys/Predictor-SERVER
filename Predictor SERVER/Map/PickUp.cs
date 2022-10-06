using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal class PickUp
    {
        public string name { get; set; }
        public string description { get; set; }
        public int experationTime { get; set; }
        public int remainingTime { get; set; }

        public PowerUp powerUp { get; set; }
        public Item item { get; set; }

        public void pickedUp()
        {

        }
    }
}
