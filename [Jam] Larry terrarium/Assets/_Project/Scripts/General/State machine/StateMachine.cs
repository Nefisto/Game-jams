using System;
using System.Collections;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State CurrentState { get; set; }

    protected void Setup (State initialState)
    {
        initialState.Enter();
        
        CurrentState = initialState;
    }

    public void ChangeState(State newState)
    {
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return null;
            
            CurrentState.Exit();
            newState.Enter();

            CurrentState = newState;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        CurrentState.Exit();
    }
}