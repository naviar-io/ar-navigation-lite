using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatesSource
{
    ALL_CHILD,
    ONLY_PRIMARY_CHILD
}

public class UIStateSwitcherManager : MonoBehaviour {

    [SerializeField]
    private AbstractSwitchAnimation defaultSwitchAnimation;
    [SerializeField]
    private State defaultState;

    public State currentState { get; private set; }
    public State[] allStates { get; private set; }

    public StatesSource statesSource;

    private State[] GetAllStates()
    {
        State[] allStates = null;

        switch (statesSource)
        {
            case StatesSource.ALL_CHILD:
                allStates = GetComponentsInChildren<State>(includeInactive: true);
                break;
            case StatesSource.ONLY_PRIMARY_CHILD:
                {
                    var list = new List<State>();
                    var allChildren = GetComponentsInChildren<State>(true);

                    foreach (var st in allChildren)
                    {
                        if (st.transform.parent == transform)
                            list.Add(st);
                    }

                    allStates = list.ToArray();
                }

                break;
        }

        return allStates;
    }

    private void Awake()
    {
        allStates = GetAllStates();

        // отключаем все дебажные стейты
        foreach (var state in allStates)
        {
            state.stateSwitcher = this;
            state.OnHideImidiately();
        }

        // устанавливаем текущий стейт
        currentState = defaultState;
        currentState.OnStatePreShown();
        currentState.OnStateReady();
    }

    public void SwitchState(State newState, AbstractSwitchAnimation animation)
    {
        currentState.OnStatePreHidden();
        newState.OnStatePreShown();

        var previousState = currentState;
        currentState = newState;

        if (animation == null)
            animation = defaultSwitchAnimation;

        var animInst = Instantiate(animation);
        animInst.OnComplete += () =>
        {
            previousState.OnStateHidden();
            newState.OnStateReady();
            Destroy(animInst);

        };

        animInst.SwitchStateAnimation(previousState, newState);
    }

    public void SwitchState(State newState)
    {
        SwitchState(newState, defaultSwitchAnimation);
    }

    public void SwitchState<T>() where T : State
    {
        var newState = GetState<T>();
        SwitchState(newState, defaultSwitchAnimation);
    }

    public void SwitchState<T>(AbstractSwitchAnimation animation) where T : State
    {
        var newState = GetState<T>();
        SwitchState(newState, animation);
    }

    public T GetState<T>() where T : State
    {
		return allStates.FirstOrDefault(st => st is T) as T;
    }

}
