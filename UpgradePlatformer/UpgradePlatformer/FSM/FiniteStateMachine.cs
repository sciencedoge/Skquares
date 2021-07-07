using System.Collections.Generic;

namespace UpgradePlatformer.FSM
{
  public class FiniteStateMachine
  {
    public List<StateMachineState> States;
    public StateMachineState State => States[currentState];
    public int currentState;

    /// <summary>
    /// creates a finite state machine
    /// </summary>
    /// <param name="states">the states the machine can be in</param>
    public FiniteStateMachine(List<StateMachineState> states)
    {
      currentState = 0;
      States = states;
    }

    /// <summary>
    /// copys a FSM
    /// </summary>
    /// <returns>the clone</returns>
    public FiniteStateMachine Copy()
    {
      return new FiniteStateMachine(States);
    }

    /// <summary>
    /// changes the state of the fsm based off flags and the current state
    /// </summary>
    /// <param name="id">the flag to set off</param>
    public void SetFlag(int id)
    {
      for (int j = 0; j < States[currentState].Conds.Count; j++)
        if ((States[currentState].Conds[j]).Id == id)
          (States[currentState].Conds[j]).Value = true;
      if (States[currentState].CheckConds()) currentState = States[currentState].CheckCondsNext();
      for (int i = 0; i < States.Count; i++)
        for (int j = 0; j < States[i].Conds.Count; j++)
          (States[i].Conds[j]).Value = false;
    }
  }
}
