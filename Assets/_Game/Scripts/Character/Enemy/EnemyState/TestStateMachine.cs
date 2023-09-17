using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStateMachine : MonoBehaviour
{
    StateMachine stateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine?.Execute();
    }

    private void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            Debug.Log("Start Idle");
        };

        onExecute = () =>
        {
            Debug.Log("Execute Idle");
        };

        onExit = () =>
        {
            Debug.Log("Exit Idle");
        };
    }

    
    private void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {

        };

        onExecute = () =>
        {

        };

        onExit = () =>
        {

        };
    }


}
