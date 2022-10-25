using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public class TrapBuilder : Builder
    {
        private Trap trap = new Trap();
        public TrapBuilder()
        {
            this.Reset();
        }
        public void Reset()
        {
            this.trap = new Trap();
        }

        public void build(int x, int y)
        {
            addColor();
            addSize();
            addDamage();
            addCoordinates(x, y);
        }

        public override void addColor()
        {
            this.trap.color = "Blue";
        }

        public override void addSize()
        {
            this.trap.size = 5;
        }

        public void addDamage()
        {
            this.trap.damage = 1;
        }

        public override void addCoordinates(int x, int y)
        {
            this.trap.coordinates = (x, y);
        }


        public Trap GetTrap()
        {
            Trap result = this.trap;
            this.Reset();
            return result;   
        }
    }
}
