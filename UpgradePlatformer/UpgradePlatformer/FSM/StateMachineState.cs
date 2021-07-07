using System.Collections.Generic;

namespace UpgradePlatformer.FSM
{
  public unsafe class StateMachineState
  {
    public List<Flag> Conds;

    /// <summary>
    /// creates a state machine state
    /// </summary>
    /// <param name="flags">the flags that causes state changes</param>
    public StateMachineState(List<Flag> flags)
    {
      Conds = flags;
    }

    /// <summary>
    /// checks for a flag to be set off
    /// </summary>
    /// <returns>true if a flag is set off</returns>
    public bool CheckConds()
    {
      for (int j = 0; j < Conds.Count; j++)
        if (Conds[j].Value)
        {
          return true;
        }
      return false;
    }

    /// <summary>
    /// gets the next state of the machine
    /// </summary>
    /// <returns>the next state</returns>
    public int CheckCondsNext()
    {
      for (int j = 0; j < Conds.Count; j++)
        if (Conds[j].Value)
        {
          Conds[j].Value = false;
          return Conds[j].NextState;
        }
      return -1;
    }
  }
}
