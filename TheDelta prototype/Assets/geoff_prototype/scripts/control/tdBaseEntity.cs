using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tdBaseEntity : tdIBrainFSM {
    [Header("Entity States")]
    public tdEntityStates[] EntityStates;
    public tdEntityStates FirstEntityState;
    [Header("Components")]
    public Rigidbody RigidBody;

    [Header("Horizontal Movement + Rotation")]
    public float CurrentSpeed = 7f;

    [Header("Vertical Movement")]
    public float JumpSpeed = 15f;
    public float JumpDelay = 0.25f;
    public float JumpTimer;

    [Header("Collision")]
    public float GroundLength = 0.6f;
    public Vector3 ColliderOffset;
    public bool OnGround;
    public string GroundLayerName = "ground";
    public int GroundLayer => 1 << LayerMask.NameToLayer(GroundLayerName);

    [Header("Physics")]
    [Range(1, 10)]
    public float CustomGravity = 1f;
    public float FallMultiplier = 5f;
    public float Drag = 4f;

    [Header("Debug")]
    public Vector3 Velocity;

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

    private void FixedUpdate() {
        if (JumpTimer > Time.time && OnGround) {
            // _animCtrl.JumpTrigger();
            SendMessageToBrain(tdMessageType.Jump);
        }

        tdPhysicsData.ModifyPhysics(OnGround, RigidBody, CustomGravity, Drag, FallMultiplier, Velocity.x);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 currentPos = this.transform.position;
        Gizmos.DrawLine(currentPos + ColliderOffset, currentPos + ColliderOffset + Vector3.down * GroundLength);
    }
}

public enum tdEntityStates {
    Unassigned,
    PlayerNavigation,
}