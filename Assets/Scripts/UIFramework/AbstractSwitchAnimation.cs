using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSwitchAnimation : ScriptableObject {

    public event System.Action OnComplete;

    public abstract void SwitchStateAnimation(State previousState, State newState);

    protected void OnCompleteHandel()
    {
        if (OnComplete != null)
            OnComplete();
    }

}
