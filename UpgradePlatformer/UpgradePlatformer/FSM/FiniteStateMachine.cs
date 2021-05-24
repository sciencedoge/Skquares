using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.FSM
{
    unsafe class FiniteStateMachine
    {
        public List<StateMachineState> States;
        public StateMachineState State => States[currentState];
        public int currentState;

        public FiniteStateMachine(List<StateMachineState> states) {
            currentState = 0;
            States = states;
        }
        public void SetFlag(int id)
        {
            for (int i = 0; i < States.Count; i++)
            {
                for (int j = 0; j < States[i].Conds.Count; j++) 
                    if ((States[i].Conds[j]).Id == id)
                        (States[i].Conds[j]).Value = true;
            }
            if (States[currentState].CheckConds()) currentState = States[currentState].CheckCondsNext();
            for (int i = 0; i < States.Count; i++)
                for (int j = 0; j < States[i].Conds.Count; j++)
                        (States[i].Conds[j]).Value = false;
        }
    }
}
