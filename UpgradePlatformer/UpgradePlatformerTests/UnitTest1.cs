using System;
using Xunit;
using UpgradePlatformer.Entities;
using Microsoft.Xna.Framework;

namespace UpgradePlatformerTests
{
    public class UnitTestsHealth
    {

        [Fact]
        public void TestPlayerDeathDamage()
        {
            Player p = new Player(1, 0, new Rectangle(0, 0, 22, 22), 1);

            UpgradePlatformer.Upgrade_Stuff.UpgradeStructure.InitStructure();

            p.TakeDamage(1);
            Assert.Equal(0, p.CurrentHP);
            Assert.False(p.IsActive);
        }

        [Fact]
        public void TestPlayerDamageCoolDown()
        {

            Player p = new Player(2, 0, new Rectangle(0, 0, 22, 22), 1);

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
            Player p = new Player(2, 0, new Rectangle(0, 0, 22, 22), 1);
        }
    }
}
