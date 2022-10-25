using System;
using Predictor_SERVER.Character;

namespace Predictor_SERVER.Map
{
    internal class MatchBoost
    {
        private Class _player;
        public MatchBoost(Class player)
        {
            this._player = player;
        }
        /// <summary>
        /// method for creation of an item and apllying it to player class
        /// </summary>
        public void ItemBoost()
        {
            Item item = new AttackSpeedPotion((0, 0));
            item.pickedUp(_player);
        }
        /// <summary>
        /// method for creation of powerup and apllying it to player class
        /// </summary>
        public void PowerUpBoost()
        {
            PowerUp powerUp = new AttackSpeedPowerUp((0,0));
            powerUp.pickedUp(_player);
        }
    }
}
