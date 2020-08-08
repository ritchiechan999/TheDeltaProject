﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerController : MonoBehaviour {
    tdBaseEntity _tdBaseEntity;
    // Start is called before the first frame update
    void Start() {
        _tdBaseEntity = this.GetComponent<tdBaseEntity>();
    }

    // Update is called once per frame
    void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        _tdBaseEntity.SendMessageToBrain(tdMessageType.Move, hAxis);

        _tdBaseEntity.OnGround = Physics.Raycast(transform.position + _tdBaseEntity.ColliderOffset,
                                                    Vector2.down, _tdBaseEntity.GroundLength, _tdBaseEntity.GroundLayer);
        if (Input.GetKeyDown(KeyCode.Space)) {
            _tdBaseEntity.JumpTimer = Time.time + _tdBaseEntity.JumpDelay;
        }
    }
}