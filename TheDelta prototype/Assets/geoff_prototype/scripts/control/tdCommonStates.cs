using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerNavigationState : tdIBaseState<tdBaseEntity> {
    public tdPlayerNavigationState(tdBaseEntity brain, int initConstruct) : base(brain) { }

    public override void OnReceiveMessage(tdMessageType msgType, object[] args) {
        switch (msgType) {
            case tdMessageType.Move:
                float horizontal = (float)args[0];
                Entity.RgdBdy.velocity = new Vector2(horizontal * Entity.CurrentSpeed, Entity.RgdBdy.velocity.y);
                Entity.RotateEntity(horizontal);
                if (Entity.OnGround) {
                    MovementAnimation(horizontal);
                }
                break;
            case tdMessageType.Jump:
                Entity.Velocity = new Vector2(Entity.Velocity.x, 0);
                Entity.RgdBdy.AddForce(Vector2.up * Entity.JumpSpeed, ForceMode.Impulse);
                Entity.JumpTimer = 0;
                Entity.AnimCtrl.SetTrigger("jump");
                break;
            case tdMessageType.Attack:
                //go to attack state
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

    //animation side
    void MovementAnimation(float xDir) {
        float moveDir = Mathf.Abs(xDir);
        Entity.AnimCtrl.SetFloat("nav_speed", moveDir);
    }
}