using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.UI;

namespace UpgradePlatformer.Menus.HatShop
{
  //HEADER=======================================
  //Name: Sami Chamberlain, Preston Precourt
  //Date: 5/26/2021
  //Purpose: Manages upgrades
  //============================================

  public class ShopManager
  {
    private static readonly Lazy<ShopManager>
        lazy =
        new Lazy<ShopManager>
            (() => new ShopManager());
    public static ShopManager Instance { get { return lazy.Value; } }

    public static SpriteFont font;
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
      HatNode node;
      if (parent == -1)
        node = new HatNode { hat = hat, parent = null };
      else
        node = new HatNode { hat = hat, parent = Hats[parent] };
      Hats.Add(node);
    }

    public UIGroup ConstructUI(int state)
    {
      UIGroup result = new UIGroup();
      result.IsActive = state == 6;
      int pos = 45;
      foreach (HatNode hatNode in Hats)
      {
        Hat hat = hatNode.hat;
        if (hatNode.parent == null || hatNode.parent.owned)
        {
          UIButton button = new UIButton(font, new Rectangle(5, pos, 620, 40));
          Sprite s = hat.sprite.Copy();
          s.Origin = new Vector2(0, 0);
          button.SetIcon(s);
          button.Text.Text = hat.Name;
          pos += 50;
          result.Add(button);
        }
      }
      return result;
    }
  }
}
