using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdNavigationState : tdIBaseState<tdBaseEntity> {
    public tdNavigationState(tdBaseEntity brain, int initConstruct) : base(brain) { }

    public override void OnReceiveMessage(tdMessageType msgType, object[] args) {
        switch (msgType) {
            case tdMessageType.Move:
                float horizontal = (float)args[0];
                MoveEntity(horizontal);
                Entity.RotateEntity(horizontal);
                if (Entity.OnGround) {
                    MovementAnimation(horizontal);
                }
                break;
            case tdMessageType.Jump:
                Entity.AnimCtrl.SetTrigger("jump");
                Entity.AnimCtrl.SetFloat("nav_speed", 0);
                Entity.Velocity = new Vector2(Entity.Velocity.x, 0);
                Entity.RgdBdy.AddForce(Vector2.up * Entity.JumpSpeed, ForceMode.Impulse);
                Entity.JumpTimer = 0;
                break;
            case tdMessageType.Attack:
                //go to attack state
                Entity.ChangeState(typeof(tdAttackState), args);
                break;
            case tdMessageType.Flinch:
                //chance to change state or idk to be discussed
                break;
            default:
                break;
        }
    }

    public override void OnStateEnter(object[] args) {
    }

    public override void OnStateExit(object[] args) {
    }

    public override void OnStateUpdate() {
        Entity.AnimCtrl.SetFloat("yVelocity", Entity.RgdBdy.velocity.y);
        Entity.AnimCtrl.SetBool("on_ground", Entity.OnGround);
    }

    public float SprintDownSpeed = 0.1f;
    float _sprintSpeed;
    public float SprintTimeTreshold = 1f;
    float _sprintTime;
    bool _isSprinting;

    void MovementAnimation(float xDir) {
        float moveSpeed = Mathf.Abs(xDir);
        float maxSpeed = Entity.AnimCtrl.GetFloat("nav_speed");
        if (maxSpeed >= 1 && moveSpeed == 1) {
            if (_sprintTime < SprintTimeTreshold) {
                _sprintTime += Time.deltaTime;
            } else {
                _isSprinting = true;
                _sprintSpeed += Time.deltaTime;
            }
        } else {
            _isSprinting = false;
            _sprintSpeed -= SprintDownSpeed / Time.deltaTime;
            _sprintTime = 0;
        }
        _sprintTime = Mathf.Clamp(_sprintTime, 0, SprintTimeTreshold);
        _sprintSpeed = Mathf.Clamp01(_sprintSpeed);
        moveSpeed += _sprintSpeed;
        Entity.AnimCtrl.SetFloat("nav_speed", moveSpeed);
    }

    //TODO decide later where to put below function
    void MoveEntity(float xDir) {
        float sprintSpeed = Mathf.Lerp(Entity.MinMaxMoveSpeed.y, Entity.MinMaxMoveSpeed.x, Entity.MoveSmoothSpeed);
        float runSpeed = Mathf.Lerp(Entity.MinMaxMoveSpeed.x, Entity.MinMaxMoveSpeed.y, Entity.MoveSmoothSpeed);
        Entity.CurrentSpeed = _isSprinting && Entity.OnGround ? sprintSpeed : runSpeed;
        Entity.RgdBdy.velocity = new Vector2(xDir * Entity.CurrentSpeed, Entity.RgdBdy.velocity.y);
    }
}

public class tdAttackState : tdIBaseState<tdBaseEntity> {
    public tdAttackState(tdBaseEntity brain, int initConstruct) : base(brain) { }
    public override void OnReceiveMessage(tdMessageType msgType, object[] args) {
        switch (msgType) {
            //TODO continuous combo
            case tdMessageType.Attack:
                Attack currentAtk = (Attack)args[0];
                Entity.AnimCtrl.SetInteger("anim_state", (int)currentAtk.AnimState);
                break;
            case tdMessageType.Flinch:
                break;
        }
    }

    public override void OnStateEnter(object[] args) {
        if (args != null) {
            Attack currentAtk = (Attack)args[0];
            Entity.AnimCtrl.SetInteger("anim_state", (int)currentAtk.AnimState);
        }
    }

    public override void OnStateExit(object[] args) {
    }

    public override void OnStateUpdate() {
    }
}