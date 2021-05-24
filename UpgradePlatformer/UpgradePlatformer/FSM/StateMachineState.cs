using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.FSM
{
    public class StateMachineState
    {
        public int id;
        public int nextState;

        public Flag cond;
    }

    public unsafe struct Flag
    {
        public int id;
        public bool _value;
        public bool Value {
            get {
                bool old = _value;
                _value = false;
                return old;
            }
            set => _value = value;
        }
    }
}
