using Predictor_SERVER.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal abstract class NpcDeath
    {
        public abstract PickUp onDeath((int, int) coordinates);
    }
}
