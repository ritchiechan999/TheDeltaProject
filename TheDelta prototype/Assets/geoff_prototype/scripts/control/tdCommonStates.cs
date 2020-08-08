using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerNavigationState : tdIBaseState<tdBaseEntity> {
    public tdPlayerNavigationState(tdBaseEntity brain, int initConstruct) : base(brain) { }

    public override void OnReceiveMessage(tdMessageType msgType, object[] args) {
        switch (msgType) {
            case tdMessageType.Move:
                float horizontal = (float)args[0];
                Entity.RigidBody.velocity = new Vector2(horizontal * Entity.CurrentSpeed, Entity.RigidBody.velocity.y);
                break;
            case tdMessageType.Jump:
                Entity.Velocity = new Vector2(Entity.Velocity.x, 0);
                Entity.RigidBody.AddForce(Vector2.up * Entity.JumpSpeed, ForceMode.Impulse);
                Entity.JumpTimer = 0;
                break;
            case tdMessageType.Attack:
                break;
            case tdMessageType.Flinch:
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
    }
}