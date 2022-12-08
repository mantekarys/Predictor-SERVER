using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class VisitorDamage : Visitor
    {
        public override void Visit(Class character)
        {
            character.damage++;
            Console.WriteLine("{0} visited, {1} damage", this.GetType().Name, character.damage);
        }
    }
}
