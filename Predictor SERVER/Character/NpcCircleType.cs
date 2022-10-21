using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal class NpcCircleType: NpcMovement
    {
        int dir = 0;
        public override void move(int matchId, Npc npc)
        {
            dir = (dir + 1) % 4;
            changeCoordinates(dir, matchId, npc);
        }
    }
}
