using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public class ObstacleBuilder : Builder
    {
        private Obstacle obstacle = new Obstacle();
        public ObstacleBuilder()
        {
            this.Reset();
        }
        public void Reset()
        {
            this.obstacle = new Obstacle();
        }

        public void build(int x, int y)
        {
            addColor();
            addSize();
            addHealth();
            addCoordinates(x, y);
        }

        public override void addColor()
        {
            this.obstacle.color = "Red";
        }

        public override void addSize()
        {
            this.obstacle.size = 20;
        }

        public void addHealth()
        {
            this.obstacle.health = 5;
        }

        public override void addCoordinates(int x, int y)
        {
            this.obstacle.coordinates = (x, y);
        }


        public Obstacle GetObstacle()
        {
            Obstacle result = this.obstacle;
            this.Reset();
            return result;   
        }
    }
}
