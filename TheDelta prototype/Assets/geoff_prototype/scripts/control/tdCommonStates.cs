﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerNavigationState : tdIBaseState<tdBaseEntity> {
    public tdPlayerNavigationState(tdBaseEntity brain, int initConstruct) : base(brain) { }

    public override void OnReceiveMessage(tdMessageType msgType, object[] args) {
        switch (msgType) {
            case tdMessageType.Move:



                break;
            case tdMessageType.Jump:
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