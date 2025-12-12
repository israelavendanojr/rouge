using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class StateMachine : MonoBehaviour
{
    protected State _currentState;
    public virtual State InitialState() => null;
    void Start()
    {
        _currentState = InitialState();
        _currentState.Enter();
    }
    void FixedUpdate()
    {
        _currentState.Update();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        _currentState.OnTriggerEnter(collider);
    }
    public void SetState(State state)
    {
        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }
    // void OnDrawGizmos() 
    // {
    //     if (_currentState == null)
    //         return;

    //     Handles.color = Color.green;
    //     Handles.Label(transform.position, _currentState.ToString());
    // }

}
