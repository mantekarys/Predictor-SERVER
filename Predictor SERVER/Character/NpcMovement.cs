using Predictor_SERVER.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal abstract class NpcMovement
    {
        public abstract void move(int matchId, Npc npc);
        public void changeCoordinates(int dir, int matchId, Npc npc)
        {
            int pad = 5;
            int mSize = 700;

            var prev = npc.coordinates;
            if (dir == 0)
            {
                if (npc.coordinates.Item1 > npc.speed + pad)
                {
                    npc.coordinates.Item1 -= npc.speed;
                }
                else
                {
                    npc.coordinates.Item1 = pad;
                }
            }
            else if (dir == 2)
            {
                if (npc.coordinates.Item1 + npc.speed + npc.size < mSize)
                {
                    npc.coordinates.Item1 += npc.speed;
                }
                else
                {
                    npc.coordinates.Item1 = mSize - npc.size + pad;
                }
            }
            if (dir == 1)
            {
                if (npc.coordinates.Item2 > npc.speed + pad)
                {
                    npc.coordinates.Item2 -= npc.speed;
                }
                else
                {
                    npc.coordinates.Item2 = pad;
                }

            }
            else if (dir == 3)
            {
                if (npc.coordinates.Item2 + npc.speed + npc.size < mSize)
                {
                    npc.coordinates.Item2 += npc.speed;
                }
                else
                {
                    npc.coordinates.Item2 = mSize - npc.size + pad;
                }
            }

            Iterator iterator = Variables.obstacles[matchId].CreateIterator();
            var obs = (Obstacle)iterator.First();
            while (obs != null)
            {
                var k = obs.collision(prev, npc.coordinates, npc.size);
                var diff = (prev.Item1 - npc.coordinates.Item1, prev.Item2 - npc.coordinates.Item2);
                if (k != (-1, -1))
                {
                    if (diff.Item1 == 0)
                    {
                        npc.coordinates.Item2 = k.Item2;
                    }
                    else
                    {
                        npc.coordinates.Item1 = k.Item1;
                    }
                }
                obs = (Obstacle)iterator.Next();
            }
            foreach (var player in Variables.matches[matchId].players)
            {
                if (player.playerClass.health <= 0) continue;
                var k = player.playerClass.collision(prev, npc.coordinates, npc.size);
                var diff = (prev.Item1 - npc.coordinates.Item1, prev.Item2 - npc.coordinates.Item2);
                if (k != (-1, -1))
                {
                    if (diff.Item1 == 0)
                    {
                        npc.coordinates.Item2 = k.Item2;
                    }
                    else
                    {
                        npc.coordinates.Item1 = k.Item1;
                    }
                    player.playerClass.takeDamage(npc.damage);
                }
            }
        }
    }
}
