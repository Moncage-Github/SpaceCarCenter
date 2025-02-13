using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuning
{
    public class TuningTool
    {
        public enum Type
        {
            Hand = 1,
            Driver ,
            Hammer,
            PaintingGun,
            None,
        }

        public Type ToolType { get; private set; }

        public TuningTool(Type type) => ToolType = type;
    }
}