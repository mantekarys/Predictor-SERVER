using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public abstract class Aggregate
    {
        public abstract Iterator CreateIterator();
    }
}
