using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public class Trap : MapObject
    {
        public int damage;
        string description;
        string name;
        public Trap()
        {

            description = "";
            name = "";
        }

        public bool collision((int, int) prev, (int, int) curr, int size)
        {
            int distPrev = prev.Item1 + prev.Item2;
            int distCurr = curr.Item1 + curr.Item2;
            (int, int) pathSize = (Math.Abs(prev.Item1 - curr.Item1) + size, Math.Abs(prev.Item2 - curr.Item2) + size);
            RectangleF PathRegion;
            if (distPrev < distCurr)
            {
                PathRegion = new Rectangle(prev.Item1, prev.Item2, pathSize.Item1, pathSize.Item2);
            }
            else
            {
                PathRegion = new Rectangle(curr.Item1, curr.Item2, pathSize.Item1, pathSize.Item2);
            }

            RectangleF trpRegion = new Rectangle(this.coordinates.Item1, this.coordinates.Item2, this.size, this.size);
            RectangleF intersectRectangleF = RectangleF.Intersect(PathRegion, trpRegion);

            if (intersectRectangleF.Height != 0 && intersectRectangleF.Width != 0)
            {
                return true;
            }
            return false;
        }
    }

    

    
}
