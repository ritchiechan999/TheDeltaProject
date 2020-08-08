using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tdBaseEntity : tdIBrainFSM {
    [Header("Components")]
    public tdEntityStates[] EntityStates;
    public tdEntityStates FirstEntityState;
    public Rigidbody RigidBody;

    [Header("Horizontal Movement + Rotation")]
    public float CurrentSpeed = 7f;


    // Start is called before the first frame update
    void Start() {
        InitializeFSM();
    }

    void InitializeFSM() {
        foreach (tdEntityStates s in EntityStates) {
            string strType = $"td{s}State";
            Type t = Type.GetType(strType);
            if (t == null)
                throw new Exception(strType + " doesnt implement yet");
            tdIState state = (tdIState)Activator.CreateInstance(t, this, 0);
            this.RegisterState(state);
        }
        string csString = $"td{FirstEntityState}State";
        Type cs = Type.GetType(csString);
        ChangeState(cs);
    }

    // Update is called once per frame
    void Update() {
        UpdateBrain();
    }
}

public enum tdEntityStates {
    Unassigned,
    PlayerNavigation,

}