using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public abstract class Character
    {
        public int size;
        public int speed;
        public int health;
        public int damage;
        public (int,int) coordinates;

        public abstract void move();//event
        public bool takeDamage(int damage)
        {
            this.health -= damage;
            return this.health <= 0;
        }

        public (int, int) collision((int, int) prev, (int, int) curr, int size)
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

            //RectangleF cRegion = new Rectangle(prev.Item1, prev.Item2, size, size);
            RectangleF obsRegion = new Rectangle(this.coordinates.Item1, this.coordinates.Item2, this.size, this.size);
            RectangleF intersectRectangleF = RectangleF.Intersect(PathRegion, obsRegion);

            if (intersectRectangleF.Height != 0 && intersectRectangleF.Width != 0)
            {
                if (distPrev < distCurr)
                {
                    return ((int)intersectRectangleF.X - size, (int)intersectRectangleF.Y - size);
                }
                else
                {
                    return ((int)intersectRectangleF.X + (int)intersectRectangleF.Width, (int)intersectRectangleF.Y + (int)intersectRectangleF.Height);
                }
            }
            return (-1, -1);
        }
    }
}
