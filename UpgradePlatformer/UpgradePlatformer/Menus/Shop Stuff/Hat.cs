using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Upgrade_Stuff;

namespace UpgradePlatformer.Menus.Shop_Stuff
{
  //HEADER=======================================
  //Name: Sami Chamberlain, Preston Precourt
  //Date: 5/26/2021
  //Purpose: Manages hats
  //============================================
  public class Hat
  {
    public UpgradeBonus bonus;
    public Sprite sprite;
    public string Name;

    public Hat(int texture, UpgradeType boost, float value, String name)
    {
      Name = name;
      bonus = new UpgradeBonus { value = value, type = boost };
      sprite = new Sprite(new Rectangle(35 + 7 * texture, 0, 7, 7), new Vector2(3, 7), Color.White);
    }

    public void Draw(SpriteBatch sb, GameTime gt)
    {
      Player p = EntityManager.Instance.Player();
      if (p.animation.finiteStateMachine.currentState % 2 == 0)
      {
        float wave = MathF.Cos((float)gt.TotalGameTime.TotalSeconds) / 10 + (MathF.PI / 4 * 7);
        sprite.Draw(sb, p.hitbox.Location + new Point(3, 3), wave, new Vector2(15, 15));
      }
      else
      {
        float wave = MathF.Cos((float)gt.TotalGameTime.TotalSeconds) / 10 - (MathF.PI / 4 * 7);
        sprite.Draw(sb, p.hitbox.Location + new Point(p.hitbox.Size.X - 3, 3), wave, new Vector2(15, 15));
      }
    }
  }
}