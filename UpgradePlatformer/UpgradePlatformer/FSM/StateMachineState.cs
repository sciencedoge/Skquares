using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.FSM
{
    public unsafe class StateMachineState
    {
        public List<Flag> Conds;
        public StateMachineState(List<Flag> flags) {
            Conds = flags;
        }

        public bool CheckConds()
        {
            for (int j = 0; j < Conds.Count; j++)
                if (Conds[j].Value)
                {
                    return true;
                }
            return false;
        }
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

    public class Flag
    {
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
