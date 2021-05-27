using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Upgrade_Stuff;

namespace UpgradePlatformer.Entities
{
    //HEADER==================================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/26/2021
    //Purpose: Creates UpgradeEntities to be used in the game
    //========================================================
    class Pillar : CollectibleObject
    {
        private Upgrade upgrade;

        /// <summary>
        /// Creates a new Upgrade Entity object
        /// </summary>
        /// <param name="cost">cost of the upgrade</param>
        /// <param name="texture">texture of the upgrade</param>
        /// <param name="hitbox">hitbox of the entity</param>
        /// <param name="upgrade">the actual upgrade</param>
        public Pillar(int cost, Rectangle hitbox, Upgrade upgrade)
            :base(cost, hitbox, EntityKind.UPGRADE)
        {
            this.upgrade = upgrade;
        }


        public void Update()
        {
            spriteSize = hitbox.Size;
        }

        /// <summary>
        /// Checks if the player can obtain this upgrade
        /// </summary>
        /// <param name="obj">a living object</param>
        /// <returns></returns>
        public Upgrade Intersects(LivingObject obj, int totalMoney)
        {
            if (IsActive && obj != null)
            {
                if (this.hitbox.Intersects(obj.Hitbox))
                {
                    if(ValidIntersection(totalMoney) == true)
                    {
                        //the coin is no longer active
                        return this.upgrade;
                    }                 
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if the object can be picked up
        /// </summary>
        /// <param name="totalMoney">the total money of the player</param>
        /// <returns></returns>
        private bool ValidIntersection(int totalMoney)
        {
            List<Upgrade> upgrades = UpgradeManager.Instance.CanBeLearned();
           
            if (totalMoney - this.value < 0
                && !upgrades.Contains(upgrade))
            {
                return false;
            }

            else
            {
                IsActive = false;
                upgrade.IsLearned = true; 
                return true;
            }
        }
    }
}
