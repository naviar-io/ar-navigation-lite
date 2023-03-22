using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleStateSwitcher : MonoBehaviour {

    public UIStateSwitcherManager stateSwitcher;
    private int i;

    public void NextState()
    {
        var states = stateSwitcher.allStates;

        i++;
        if (i >= states.Length)
            i = 0;

        var nextState = states[i];
        stateSwitcher.SwitchState(nextState);
    }
}
