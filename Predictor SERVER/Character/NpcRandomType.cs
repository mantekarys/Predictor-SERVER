using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal class NpcRandomType: NpcMovement
    {
        int dir = 0;
        Random random = new Random();
        public override void move(int matchId, Npc npc)
        {
            dir = random.Next(0, 5);
            changeCoordinates(dir, matchId, npc);
        }
    }
}
