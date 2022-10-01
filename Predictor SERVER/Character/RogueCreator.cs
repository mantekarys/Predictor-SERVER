using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal class RogueCreator : ClassCreator
    {
        override public Class CreateClass(int size, int x, int y)
        {
            return new Rogue(size, x,y);
        }
    }
}
