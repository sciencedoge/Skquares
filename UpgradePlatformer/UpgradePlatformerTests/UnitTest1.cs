using System;
using Xunit;
using UpgradePlatformer.Entities;
using Microsoft.Xna.Framework;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformerTests
{
    public class UnitTestsHealth
    {

        [Fact]
        public void TestPlayerDeathDamage()
        {
            Player p = new Player(1, 0, new Rectangle(0, 0, 22, 22), 1);
            p.Demo = false;

            UpgradePlatformer.Upgrade_Stuff.UpgradeStructure.InitStructure();

            p.TakeDamage(1);
            Assert.Equal(0, p.CurrentHP);
            Assert.False(p.IsActive);
        }

        [Fact]
        public void TestPlayerDamageCoolDown()
        {

            Player p = new Player(2, 0, new Rectangle(0, 0, 22, 22), 1);
            p.Demo = false;

            UpgradePlatformer.Upgrade_Stuff.UpgradeStructure.InitStructure();

            Assert.Equal(0, p.cooldown);
            p.TakeDamage(1);
            Assert.Equal(1, p.CurrentHP);
            Assert.True(p.IsActive);
            p.Update(new GameTime(TimeSpan.FromMilliseconds(2500), TimeSpan.FromMilliseconds(2500)));
            Assert.Equal(0, p.cooldown);
        }
    }
    public class UnitTestsPhysics
    {
        [Fact]
        public void TestConstantGravity()
        {
            Sprite.graphics = new GraphicsDeviceManager(new Game());
            Player p = new Player(2, 0, new Rectangle(300, 0, 22, 22), 1);
            p.Demo = false;
            Enemy e = new Enemy(2, 0, new Rectangle(300, 0, 22, 22), 1);

            for (int i = 0; i < 100; i++)
            {
                p.Update(new GameTime(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500)));
                e.Update(new GameTime(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500)));
                Assert.Equal(300, p.Position.X);
                Assert.Equal(300, e.Position.X);
            }

            Assert.Equal(0, p.Position.Y - e.Position.Y);
        }
    }
}
