using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class ProjectileComposite : Projectile
    {
        public List<Projectile> children = new List<Projectile>();
        public ProjectileComposite(int speed, int size, (int, int) coordinates, int direction, Class attacker, int width, int height)
        {
            this.speed = speed;
            this.size = size;
            this.coordinates = coordinates;
            this.direction = direction;
            this.attacker = attacker;
            (int, int) newCoordinates = (coordinates.Item1, coordinates.Item2);
            switch (direction)
            {
                case 0: // up
                    newCoordinates.Item2 = newCoordinates.Item2 - size - 2;
                    newCoordinates.Item1 = coordinates.Item1 + size / 2 - ((size + 2) * width - 2) / 2;
                    break;
                case 1: // right
                    newCoordinates.Item1 = newCoordinates.Item1 + size + 2;
                    newCoordinates.Item2 = coordinates.Item2 + size / 2 - ((size + 2) * width - 2) / 2;
                    break;
                case 2: //down
                    newCoordinates.Item2 = newCoordinates.Item2 + size + 2;
                    newCoordinates.Item1 = coordinates.Item1 + size / 2 - ((size + 2) * width - 2) / 2;
                    break;
                case 3: // left
                    newCoordinates.Item1 = newCoordinates.Item1 - size - 2;
                    newCoordinates.Item2 = coordinates.Item2 + size / 2 - ((size + 2) * width - 2) / 2;
                    break;
            }
            for (int i = 0; i < width; i++)
            {
                if (height > 0)
                {
                    add(new ProjectileComposite(speed, size, newCoordinates, direction, attacker, width, height - 1));
                }
                else
                {
                    add(new ProjectileLeaf(speed, size, newCoordinates, direction, attacker));
                }
                switch (direction)
                {
                    case 0:case 2:
                        newCoordinates.Item1 += size + 2;
                        break;
                    case 1: case 3: 
                        newCoordinates.Item2 += size + 2;
                        break;
                }
            }

        }

        public void add(Projectile projectile)
        {
            children.Add(projectile);
        }

        public bool remove(Projectile projectile)
        {
            return children.Remove(projectile);
        }

        public Projectile getChild(int index)
        {
            if(index < 0 || index >= children.Count) { return null; }
            return children[index];
        }
        public override List<ProjectileLeaf> getList()
        {
            List<ProjectileLeaf> list = new List<ProjectileLeaf>();
            list.Add(new ProjectileLeaf(speed, size, coordinates, direction, attacker));
            foreach (Projectile item in children)
            {
                list.AddRange(item.getList());
            }
            return list;
        }
    }
}
