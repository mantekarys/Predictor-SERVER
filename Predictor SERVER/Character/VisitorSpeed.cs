using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class VisitorSpeed : Visitor
    {
        public override void Visit(Class character)
        {
            character.speed++;
            Console.WriteLine("{0} visited, {1} speed", this.GetType().Name, character.speed);
        }
    }
}
