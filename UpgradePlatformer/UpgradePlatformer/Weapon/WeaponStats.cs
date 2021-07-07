namespace UpgradePlatformer.Weapon
{
  //HEADER===================================================
  //Names: Sami Chamberlain, Preston Precourt
  //Date: 5/30/2021
  //Purpose: Gives stats for the player's weapon
  //=========================================================
  public class WeaponStats
  {
    public float knockBack;
    public int knockBackTime;
    public int cap;
    public float projSpeed;
    public int pierce;
    public WeaponStats(float knockBack, int cap, int knockBackTime, float projSpeed, int pierce)
    {
      this.projSpeed = projSpeed;
      this.knockBack = knockBack;
      this.knockBackTime = knockBackTime;
      this.cap = cap;
      this.pierce = pierce;
    }
  }
}
