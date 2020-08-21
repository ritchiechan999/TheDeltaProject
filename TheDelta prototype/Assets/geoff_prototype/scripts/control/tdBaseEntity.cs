using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tdBaseEntity : tdIBrainFSM {
    [Header("Entity States")]
    public tdEntityState[] EntityStates;
    public tdEntityState FirstEntityState;

    [Header("Components")]
    public Rigidbody RgdBdy;
    public Animator AnimCtrl;

    [Header("Horizontal Movement")]
    public Vector2 MinMaxMoveSpeed = new Vector2(7f, 15f);
    public float MoveSmoothSpeed = 0.15f;
    public float CurrentSpeed = 7f;

    [Header("Vertical Movement")]
    public float JumpSpeed = 15f;
    public float JumpDelay = 0.25f;
    public float JumpTimer;

    [Header("Rotation")]
    public bool IsFacingRight = true;
    Quaternion _lastRotation;
    public float RotateSmoothSpeed = 0.15f;

    [Header("Collision")]
    public float GroundLength = 0.6f;
    public Vector3 ColliderOffset;
    public bool OnGround;

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

        _lastRotation = new Quaternion(0, 0.7f, 0, 0.7f);
        RgdBdy.useGravity = false;
    }

    void InitializeFSM() {
        foreach (tdEntityState s in EntityStates) {
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

    public void RotateEntity(float xDir) {
        if (xDir == 0) {
            this.transform.rotation = _lastRotation;
            return;
        }
        Quaternion rotation = Quaternion.Slerp(this.transform.rotation,
                                Quaternion.LookRotation(new Vector3(xDir, 0, 0)), RotateSmoothSpeed);
        transform.rotation = rotation;
        _lastRotation = rotation;
        IsFacingRight = transform.rotation.y > 0;
    }

    // Update is called once per frame
    void Update() {
        UpdateBrain();
    }

    private void FixedUpdate() {
        if (JumpTimer > Time.time && OnGround) {
            SendMessageToBrain(tdMessageType.Jump);
        }

        tdPhysicsData.ModifyPhysics(OnGround, RgdBdy, CustomGravity, Drag, FallMultiplier, Velocity.x);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 currentPos = this.transform.position;
        Gizmos.DrawLine(currentPos + ColliderOffset, currentPos + ColliderOffset + Vector3.down * GroundLength);
    }
}

public enum tdEntityState {
    Unassigned,
    Navigation,
    Attack,

}