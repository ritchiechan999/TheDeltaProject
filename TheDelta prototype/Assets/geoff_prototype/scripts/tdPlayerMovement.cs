using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerMovement : MonoBehaviour {
    [Header("Components")]
    public Rigidbody RigidBody;
    tdPlayerAnimation _animCtrl;

    [Header("Horizontal Movement + Rotation")]
    public Vector2 MinMaxMoveSpeed;
    public float MoveSmoothSpeed = 0.15f;
    float _currentSpeed;
    public Vector2 _inputDir { private set; get; }
    public bool IsFacingRight = true;
    Quaternion _lastRotation;
    public float RotateSmoothSpeed = 0.15f;

    [Header("Vertical Movement")]
    public float JumpSpeed = 15f;
    public float JumpDelay = 0.25f;
    float _jumpTimer;

    [Header("Collision")]
    public float GroundLength = 0.6f;
    public Vector3 ColliderOffset;
    public bool OnGround { private set; get; }
    public string GroundLayerName = "ground";
    public int GroundLayer => 1 << LayerMask.NameToLayer(GroundLayerName);

    [Header("Player State")] //have this on another script to hold the player/make fsm design?
    public PlayerState PlayerState;

    [Header("Physics")]
    [Range(1,10)]
    public float CustomGravity = 1f;
    float _gravityScale = 1f;
    public float FallMultiplier = 5f;
    public float Drag = 4f;
    public static float GlobalGravity = -9.8f;

    [Header("Debug")]
    public Vector3 Velocity;
    // Start is called before the first frame update
    void Start() {
        RigidBody = this.GetComponent<Rigidbody>();
        _animCtrl = this.GetComponent<tdPlayerAnimation>();

        //test 
        _lastRotation = new Quaternion(0, 0.7f, 0, 0.7f);
        RigidBody.useGravity = false;
    }

    // Update is called once per frame
    void Update() {
        //inputs
        Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), 0);
        _inputDir = inputs;

        //jump inputs
        OnGround = Physics.Raycast(transform.position + ColliderOffset, Vector2.down, GroundLength, GroundLayer);
        if (Input.GetKeyDown(KeyCode.Space)) {
            _jumpTimer = Time.time + JumpDelay;
        }

        RotateCharacter();
        Velocity = RigidBody.velocity;
    }

    private void FixedUpdate() {
        if (PlayerState == PlayerState.Moving || PlayerState == PlayerState.Idle
            || PlayerState == PlayerState.Jumping) {

            PlayerState = PlayerState == PlayerState.Jumping ? PlayerState.Jumping : PlayerState.Moving;
            MoveCharacter(_inputDir.x);
        }

        if (PlayerState == PlayerState.Attacking)
            RigidBody.velocity = Vector2.zero;

        if (PlayerState == PlayerState.Jumping && Velocity.y == 0)
            PlayerState = PlayerState.Idle;

        if (_jumpTimer > Time.time && OnGround) {
            _animCtrl.JumpTrigger();
            Jump();
        }

        ModifyPhysics();
    }

    void ModifyPhysics() {
        if (OnGround) {
            RigidBody.drag = Mathf.Abs(_inputDir.x) < 0.4f ? Drag : 0;
            _gravityScale = 0;
        } else {
            _gravityScale = CustomGravity;
            RigidBody.drag = Drag * 0.15f;
            float rbVelocityY = RigidBody.velocity.y;
            Vector3 newGravity = GlobalGravity * _gravityScale * Vector3.up;
            if (rbVelocityY < 0) {
                newGravity *= FallMultiplier;
            } else if (rbVelocityY > 0 && !Input.GetButton("Jump")) {
                newGravity *= FallMultiplier / 2;
            }
            //newtons 2nd law of motion lol
            RigidBody.AddForce(newGravity, ForceMode.Acceleration);
        }
    }
    void Jump() {
        RigidBody.velocity = new Vector2(RigidBody.velocity.x, 0);
        RigidBody.AddForce(Vector2.up * JumpSpeed, ForceMode.Impulse);
        _jumpTimer = 0;

        PlayerState = PlayerState.Jumping;
    }

    void MoveCharacter(float horizontal) {
        float sprintSpeed = Mathf.Lerp(MinMaxMoveSpeed.y, MinMaxMoveSpeed.x, MoveSmoothSpeed);
        float runSpeed = Mathf.Lerp(MinMaxMoveSpeed.x, MinMaxMoveSpeed.y, MoveSmoothSpeed);
        _currentSpeed = _animCtrl.IsSprinting && PlayerState != PlayerState.Jumping? sprintSpeed : runSpeed;
        RigidBody.velocity = new Vector2(horizontal * _currentSpeed, RigidBody.velocity.y);
    }

    void RotateCharacter() {
        if (_inputDir.x == 0) {
            transform.rotation = _lastRotation;
            return;
        }
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inputDir), RotateSmoothSpeed);
        transform.rotation = rotation;
        _lastRotation = transform.rotation;
        IsFacingRight = transform.rotation.y > 0;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 currentPos = this.transform.position;
        Gizmos.DrawLine(currentPos + ColliderOffset, currentPos + ColliderOffset + Vector3.down * GroundLength);
    }
}

public enum PlayerState {
    Idle,
    Moving,
    Jumping,
    Attacking,
    SkillCasting,
    LightCombo,
    HeavyCombo,
}