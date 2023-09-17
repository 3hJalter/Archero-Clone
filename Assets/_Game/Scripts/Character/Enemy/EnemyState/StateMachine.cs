using System;

[Serializable]
public class StateMachine
{
    public delegate void StateAction(out Action action, out Action onExecute1, out Action onExit1);

    //cai nay de biet duoc no dang o state nao
    public string name;

    private Action _onEnter, _onExecute, _onExit;

    public void Execute()
    {
        _onExecute?.Invoke();
    }

    public void ChangeState(StateAction stateAction)
    {
        _onExit?.Invoke();
        stateAction.Invoke(out _onEnter, out _onExecute, out _onExit);
        _onEnter?.Invoke();

#if UNITY_EDITOR
        //cai nay de biet duoc no dang o state nao
        name = stateAction.Method.Name;
#endif
    }
}
