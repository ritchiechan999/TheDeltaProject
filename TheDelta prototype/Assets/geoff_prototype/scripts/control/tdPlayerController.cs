using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerController : MonoBehaviour {
    tdBaseEntity _tdBaseEntity;

    void Start() {
        _tdBaseEntity = this.GetComponent<tdBaseEntity>();
    }

    void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        _tdBaseEntity.SendMessageToBrain(tdMessageType.Move, hAxis);

        _tdBaseEntity.OnGround = Physics.Raycast(transform.position + _tdBaseEntity.ColliderOffset,
                                                    Vector2.down, _tdBaseEntity.GroundLength, tdData.GroundLayer);
        if (Input.GetKeyDown(KeyCode.Space)) {
            _tdBaseEntity.JumpTimer = Time.time + _tdBaseEntity.JumpDelay;
        }

        //TODO inputs for combo
    }
}