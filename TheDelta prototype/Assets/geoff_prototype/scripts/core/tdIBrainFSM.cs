﻿using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class tdIBrainFSM : MonoBehaviour {
    Dictionary<Type, tdIState> _states = new Dictionary<Type, tdIState>();
    Type _currentState;
    tdIState _existing;
    public bool BrainEnabled = true;

    public void RegisterState(tdIState state) {
        _states[state.GetType()] = state;
    }

    public bool ChangeState(Type stateType, params object[] args) {
        //if same type
        if (_currentState == stateType)
            return false;

        //if not the one to be changing to
        if (!_states.TryGetValue(stateType, out tdIState newState))
            return false;

        //correct one
        if (_currentState != null && _states.TryGetValue(_currentState, out tdIState currentState))
            currentState.OnStateExit(args);

        _currentState = stateType;
        newState.OnStateEnter(args);
        _existing = newState;
        return true;
    }

    public void UpdateBrain() {
        if (!BrainEnabled)
            return;
        _existing.OnStateUpdate();
        print(_existing);
    }

    //still testing
    public bool HasState(Type key) {
        return _states.ContainsKey(key);
    }

    /// <summary>
    /// Will be used on the entity controller to separate inputs and behaviour.
    /// Can also be used as a reaction for AI
    /// </summary>
    /// <param name="msgtype">message type</param>
    /// <param name="args">anything to pass through</param>
    public void SendMessageToBrain(tdMessageType msgType, params object[] args) {
        _existing.OnReceiveMessage(msgType, args);
    }
}

public abstract class tdIState {
    public tdIBrainFSM Brain;
    public tdIState(tdIBrainFSM brain) {
        Brain = brain;
    }
    public abstract void OnStateEnter(object[] args);
    public abstract void OnStateUpdate();
    public abstract void OnStateExit(object[] args);
    public abstract void OnReceiveMessage(tdMessageType msgType, object[] args);
}

public abstract class tdIBaseState<T> : tdIState where T : tdIBrainFSM {
    protected T Entity;
    protected int InitConstruct;
    protected tdIBaseState(T brain, int initConstruct) : base(brain) {
        Entity = brain;
        InitConstruct = initConstruct; //to resolved default constructor issues
    }
    protected tdIBaseState(T brain) : base(brain) {
        Entity = brain;
    }
}

public enum tdMessageType {
    None,
    Move,
    Jump,
    Attack,
    Flinch
}