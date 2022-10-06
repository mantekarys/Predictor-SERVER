using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal abstract class Item
    {
        
        public abstract int boostPercent { get; set; } // nustatyti kokiam stiprumui gal?? 
        public abstract void Use();

    }
}
