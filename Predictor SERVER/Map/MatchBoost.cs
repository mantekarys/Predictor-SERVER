using System;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class MatchBoost
    {

        /// <summary>
        /// method for creation of an item and apllying it to player class
        /// </summary>
        public void grantItem(Class player)
        {
            Item item = new AttackSpeedPotion((0, 0));
            item.ApplyPickUp(player);
        }
        /// <summary>
        /// method for creation of powerup and apllying it to player class
        /// </summary>
        public void grantPowerUp(Class player)
        {
            PowerUp powerUp = new AttackSpeedPowerUp((0,0));
            powerUp.ApplyPickUp(player);
        }
    }
}
