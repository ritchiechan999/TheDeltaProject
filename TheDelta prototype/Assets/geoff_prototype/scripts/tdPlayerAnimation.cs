using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerAnimation : MonoBehaviour {
    [Header("Components")]
    public Animator AnimCtrl;
    tdPlayerMovement _movement;
    //tdBasicAttack _basicAttack;
    [Header("Sprint Animation")]
    public float SprintDownSpeed = 0.1f;
    float _sprintSpeed;
    public float SprintTimeTreshold = 1f;
    float _sprintTime;
    public bool IsSprinting { private set; get; }
    public bool IsOnGround = false;

    //for now based on combo number
    [Header("Effects")]
    public AttackEffects[] AtkFx;

    // Start is called before the first frame update
    void Start() {
        AnimCtrl = GetComponent<Animator>();
        _movement = this.GetComponent<tdPlayerMovement>();
        //_basicAttack = this.GetComponent<tdBasicAttack>();
    }

    // Update is called once per frame
    void Update() {
        //ok for prototype
        IsOnGround = _movement.OnGround;
        if (IsOnGround)
            MovementAnimation();

        JumpAnimation();
    }

    public void AttackFx(int comboNb) {
        GameObject fx = Instantiate(AtkFx[comboNb].Prefab);
        Vector3 posOffset;
        Quaternion rotOffset;
        if (_movement.IsFacingRight) {
            posOffset = AtkFx[comboNb].PosRightOffset;
            rotOffset = Quaternion.Euler(AtkFx[comboNb].RotRightOffset);
        } else {
            posOffset = AtkFx[comboNb].PosLeftOffset;
            rotOffset = Quaternion.Euler(AtkFx[comboNb].RotLeftOffset);
        }
        fx.transform.position = this.transform.position + posOffset;
        fx.transform.rotation = this.transform.rotation * rotOffset;
    }

    void JumpAnimation() {
        float ySpeed = Mathf.Clamp(_movement.Velocity.y, -1, 1);
        AnimCtrl.SetFloat("yVelocity", ySpeed);
        AnimCtrl.SetBool("on_ground", IsOnGround);
    }

    public void JumpTrigger() {
        AnimCtrl.SetTrigger("jump");
    }

    void MovementAnimation() {
        float moveSpeed = Mathf.Abs(_movement._inputDir.x);
        float maxSpeed = AnimCtrl.GetFloat("nav_speed");
        if (maxSpeed >= 1 && moveSpeed == 1) {
            if (_sprintTime < SprintTimeTreshold) {
                _sprintTime += Time.deltaTime;
            } else {
                IsSprinting = true;
                _sprintSpeed += Time.deltaTime;
            }
        } else {
            IsSprinting = false;
            _sprintSpeed -= SprintDownSpeed / Time.deltaTime;
            _sprintTime = 0;
        }
        _sprintTime = Mathf.Clamp(_sprintTime, 0, SprintTimeTreshold);
        _sprintSpeed = Mathf.Clamp01(_sprintSpeed);
        moveSpeed += _sprintSpeed;
        AnimCtrl.SetFloat("nav_speed", moveSpeed);
    }
}

[System.Serializable]
public class AttackEffects {
    public string Name;
    public GameObject Prefab;
    public Vector3 PosRightOffset;
    public Vector3 PosLeftOffset;
    public Vector3 RotRightOffset;
    public Vector3 RotLeftOffset;
}