using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public abstract class Builder
    {
        //https://refactoring.guru/design-patterns/builder/csharp/example
        public abstract void addColor();
        public abstract void addSize();



    }
}
