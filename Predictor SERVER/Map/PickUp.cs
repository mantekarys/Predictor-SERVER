using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Predictor_SERVER.Character;


namespace Predictor_SERVER.Map
{
    public abstract class PickUp
    {
        public  string name { get; set; }
        public  string description { get; set; }
        public  int experationTime { get; set; }
        public  DateTime remainingTime { get; set; }
        public int size = 6;
        public (int, int) coordinates;
        public abstract void pickedUp(Class @class, int matchID, int i);
        public abstract void ApplyPickUp(Class @class);
        public abstract void DeletePickUpFromVariables(int matchID, int i);

    }
}
