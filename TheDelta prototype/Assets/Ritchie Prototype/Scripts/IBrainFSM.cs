using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Unassigned,
    Idle,
    Patrol,
    Chase,
    Attack,
    Combo1,
    Combo2,
    Combo3
}
public abstract class IBrainFSM : MonoBehaviour
{

    Dictionary<StateType, IState> _states = new Dictionary<StateType, IState>();
    StateType _currentState = StateType.Unassigned;
    public StateType CurrentState { get { return _currentState; } }
    IState _existing;

    public void RegisterState(IState state)
    {
        _states[state.StateType] = state;
    }

    public bool ChangeState(StateType type,params object[] args)
    {
        if (_currentState == type)
            return false;

        IState newstate;
        if (!_states.TryGetValue(type, out newstate))
            return false;

        IState currentstate;
        if (_states.TryGetValue(_currentState, out currentstate))
            currentstate.OnStateExit();

        _currentState = type;
        newstate.OnStateEnter(args);
        _existing = newstate;
        return true;
    }

    public void UpdateBrain()
    {
        //  IState existing;
        // if (_states.TryGetValue(_currentState, out existing))
        //  existing.OnStateUpdate();
        _existing.OnStateUpdate();
    }
}
public abstract class IState
{
    public IBrainFSM Brain;//Machine
    public IState(IBrainFSM brain)
    {
        Brain = brain;
    }
    public abstract StateType StateType { get; }
    public abstract void OnStateEnter(object[] args);
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}



