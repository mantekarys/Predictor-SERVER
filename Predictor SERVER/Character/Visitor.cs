using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public abstract class Visitor
    {
        public abstract void Visit(Class character);
    }
}
