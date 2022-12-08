using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class VisitorHealth : Visitor
    {
        public override void Visit(Class character)
        {
            character.takeDamage(1);
            Console.WriteLine("{0} visited, {1} health left",this.GetType().Name, character.health);
        }
    }
}
