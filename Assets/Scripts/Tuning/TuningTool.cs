using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuningTool
{
    public enum Type
    {
        Hand = 1,
        Driver,
        Hammer,
    }

    public Type ToolType { get; private set; }

    public TuningTool(Type type) => ToolType = type;
}
