using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public class Map
    {
        public int size = 700;
        public string name;
        public List<MapObject> mapO;

        public Map(string name, List<MapObject> mapO)
        {
            this.name = name;//mayby delete
            this.mapO = mapO;
        }
    }
}
