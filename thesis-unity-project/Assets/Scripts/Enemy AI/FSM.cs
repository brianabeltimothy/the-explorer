using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class State : MonoBehaviour
{
    public abstract State RunCurrentState();
}

public class FSM<T>
{
    private T currentState;
    private T previousState;

    private readonly Dictionary<T, Action> enterCallbacks = new Dictionary<T, Action>();
    private readonly Dictionary<T, Action> updateCallbacks = new Dictionary<T, Action>();
    private readonly Dictionary<T, Action> exitCallbacks = new Dictionary<T, Action>();

    public FSM(T initialState)
    {
        currentState = initialState;
        previousState = initialState;
    }

    public void AddState(T state, Action onEnter, Action onUpdate, Action onExit)
    {
        enterCallbacks[state] = onEnter;
        updateCallbacks[state] = onUpdate;
        exitCallbacks[state] = onExit;
    }

    public void ChangeState(T newState)
    {
        if (currentState.Equals(newState))
            return;

        exitCallbacks[previousState]?.Invoke();
        enterCallbacks[newState]?.Invoke();

        previousState = currentState;
        currentState = newState;
    }

    public void Update()
    {
        updateCallbacks[currentState]?.Invoke();
    }
}
