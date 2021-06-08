using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Upgrade_Stuff;

namespace UpgradePlatformer.Shop_Stuff
{
    //HEADER=======================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/26/2021
    //Purpose: Manages upgrades
    //============================================
    
    public class HatNode
    {
        public HatNode parent;

        public Hat hat;
    }

    public class ShopManager
    {
        private static readonly Lazy<ShopManager>
            lazy =
            new Lazy<ShopManager>
                (() => new ShopManager());
        public static ShopManager Instance { get { return lazy.Value; } }

        public List<HatNode> Hats;

        /// <summary>
        /// Creates a new UpgradeManager class
        /// </summary>
        public ShopManager() 
        {
            Hats = new List<HatNode>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hat"></param>
        /// <param name="parent"></param>
        public void Add(Hat hat, int parent)
        {
            var node = new HatNode{hat = hat, parent = Hats[parent]};
            Hats.Add(node);
        }
    }
}
