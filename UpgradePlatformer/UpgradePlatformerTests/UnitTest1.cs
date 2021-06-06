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
            Player p = new Player(2, 0, new Rectangle(0, 0, 22, 22), 1);

            UpgradePlatformer.Upgrade_Stuff.UpgradeStructure.InitStructure();

            p.TakeDamage(1);
            Assert.Equal(1, p.CurrentHP);
            Assert.True(p.IsActive);
            p.Update(new GameTime(TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(3000)));
            Assert.Equal(0, p.cooldown);
            p.TakeDamage(1);
            Assert.Equal(0, p.CurrentHP);
            Assert.False(p.IsActive);
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
