using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Upgrade_Stuff;
using UpgradePlatformer.Levels;

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
        public Pillar(int cost, Rectangle hitbox, Upgrade upgrade, Tile t)
            :base(-cost, hitbox, EntityKind.UPGRADE, t)
        {
            this.upgrade = upgrade;
            this.SpriteBounds = new Rectangle(0, 45, 16, 16);
            UpdateSprite();
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
        public override int Intersects(EntityObject o)
        {
            if (o == null) return 0;
            if (o.Kind != EntityKind.PLAYER) return 0;
            LivingObject obj = (LivingObject)o;
            if (IsActive && obj != null)
            {
                if (this.hitbox.Intersects(obj.Hitbox))
                {
                    if(ValidIntersection() == true)
                    {
                        //the coin is no longer active
                        return this.upgrade.Cost * -1;
                    }                 
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// Checks if the object can be picked up
        /// </summary>
        /// <param name="totalMoney">the total money of the player</param>
        /// <returns></returns>
        private bool ValidIntersection()
        {
            List<Upgrade> upgrades = UpgradeManager.Instance.CanBeLearned();
           
            if (EntityManager.Instance.PlayerMoney - upgrade.Cost < 0
                || !upgrades.Contains(upgrade))
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
