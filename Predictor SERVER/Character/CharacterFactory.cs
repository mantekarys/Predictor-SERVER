using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{   
    internal class CharacterFactory
    {
        private Dictionary<int, Character> characters { get; set; } = new Dictionary<int, Character>();

        public CharacterFactory() {
            long total = GC.GetTotalMemory(true);
            var sw = Stopwatch.StartNew();

            Npc n1 = new Npc(15, 5, 5, 1, 30, 30);
            n1.selectDeathType("item");
            Npc n2 = n1.deepCopy();
            n2.coordinates = (655, 30);
            n2.selectDeathType("powerUp");
            Npc n3 = n1.deepCopy();
            n3.coordinates = (30, 655);
            n3.selectDeathType("item");
            Npc n4 = n1.deepCopy();
            n4.coordinates = (655, 655);
            n4.selectDeathType("powerUp");

            characters.Add(0, n1);
            characters.Add(1, n2);
            characters.Add(2, n3);
            characters.Add(3, n4);

            sw.Stop();
            Console.WriteLine($"Created Npcfactory {sw.Elapsed}");

            Console.WriteLine($"Diffrence in memory after an NpcFactory was created {GC.GetTotalMemory(true) - total}");

        }
        public Character GetCharacter(int key)
        {
            if (characters.ContainsKey(key)) return characters[key];
            else
            {
                var newC = new Npc(15, 5, 5, 1, 350, 350);
                newC.selectDeathType("item");
                characters[key] = newC;
                return newC;
            }
        }
    }


}
