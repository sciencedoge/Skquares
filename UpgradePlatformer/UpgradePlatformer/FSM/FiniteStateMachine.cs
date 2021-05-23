using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.FSM
{
    unsafe class FiniteStateMachine
    {
        public List<StateMachineState> States;
        public Flag*[] flags;

        public void SetFlag (String Name)
        {
            foreach (Flag* f in flags)
            {

            }
        }
    }
}
