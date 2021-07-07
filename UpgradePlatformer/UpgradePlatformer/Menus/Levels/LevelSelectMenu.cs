using Microsoft.Xna.Framework;
using UpgradePlatformer.UI;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Menus.HatShop;

namespace UpgradePlatformer.Menus.Levels
{
  /// <summary>
  /// stores the ui for the LevelSelect
  /// </summary>
  public class LevelSelectMenu
  {
    // a list of ui elements
    public UIGroup menu;

    /// <summary>
    /// ctor
    /// </summary>
    public LevelSelectMenu()
    {
      menu = new UIGroup();
    }

    /// <summary>
    /// creates a single button from a level and adds it to elements
    /// </summary>
    /// <param name="level">level to add</param>
    public void add(Level level)
    {
      menu.Add(new UIButton(ShopManager.font, new Rectangle(0, 0, 20, 20)));
    }
  }
}