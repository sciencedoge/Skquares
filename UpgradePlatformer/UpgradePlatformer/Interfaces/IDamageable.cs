namespace UpgradePlatformer.Interfaces
{
  //HEADER============================================
  //Name: Sami Chamberlain, Preston Precourt
  //Date: 5/22/2021
  //Purpose: Provides methods to entities that can
  //be damaged
  //==================================================

  interface IDamageable : IGameEntity
  {
    //Methods

    /// <summary>
    /// Reduces health from an object
    /// </summary>
    /// <param name="damageAmt">Amount of damage taken</param>
    public void TakeDamage(int damageAmt);
  }
}
