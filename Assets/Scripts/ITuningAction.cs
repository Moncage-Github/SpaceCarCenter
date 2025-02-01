using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITuningAction
{
    void Execute();
}

public class DecompositionAction : ITuningAction
{
    private TuningObject _target;

    public DecompositionAction(TuningObject target)
    {
        _target = target;
    }

    public void Execute()
    {
       // _target.InteractionEvent.
    }
}
