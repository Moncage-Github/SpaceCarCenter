using System.Collections;
using System.Collections.Generic;
using Tuning;
using UnityEngine;

namespace Tuning
{
    public class Screw : PartsBase
    {
        public enum State { Dropped, PickUped, Composed };
        private State _state;

    }
}