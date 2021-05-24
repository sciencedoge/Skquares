using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.FSM
{
    unsafe class FiniteStateMachine
    {
        public List<StateMachineState> States;
        public StateMachineState State => States[currentState];
        private Flag*[] flags;
        private int currentState;
        public void SetFlag (int id)
        {
            foreach (Flag *f in flags)
            {
                if ((*f).id == id) {
                    (*f).Value = true;
                }
            }
            while (States[currentState].cond.Value) currentState = States[currentState].nextState;
        }
    }
}
