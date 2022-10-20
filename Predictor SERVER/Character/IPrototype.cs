using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal interface IPrototype
    {
        Npc shallowCopy();

        Npc deepCopy();

    }
}
