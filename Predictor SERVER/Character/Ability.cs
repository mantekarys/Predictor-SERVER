using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class Ability
    {
        public string name;
        string description;
        public int cooldown;
        public int duration;
        public int durationLeft= 0;
        public int cooldownLeft = 30;
        public bool activated = false;
        public Ability(int cooldown, string description, int duration, string name)
        {
            this.cooldown = cooldown;
            this.description = description;
            this.duration = duration;
            this.name = name;
        }

        public void activate()
        {
            throw new NotImplementedException();
        }
    }
}
