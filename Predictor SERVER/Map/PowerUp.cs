using Predictor_SERVER.Character;
using System;
using System.Drawing;


namespace Predictor_SERVER.Map
{
    public abstract class PowerUp : PickUp
    {
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
        public override void pickedUp(Class character, int matchID, int i)
        {
            this.ApplyPickUp(character);
            this.DeletePickUpFromVariables(matchID, i);
        }
        public override void ApplyPickUp(Class character)
        {
            character.applyPowerUp(this);
        }
        public override void DeletePickUpFromVariables(int matchID, int i)
        {
            Variables.pickables[matchID].Remove(Variables.pickables[matchID][i]);
        }
    }
}
