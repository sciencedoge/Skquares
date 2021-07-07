namespace UpgradePlatformer.FSM
{
  public class Flag
  {
    /// <summary>
    /// creates a flag to set off a state machine change
    /// </summary>
    /// <param name="id">the flags id this can be repeated</param>
    /// <param name="nextState">the state the machine should change to when this is set off</param>
    public Flag(int id, int nextState)
    {
      Id = id;
      NextState = nextState;
      Value = false;
    }

    public int Id;
    public int NextState;
    public bool Value;
  }
}
