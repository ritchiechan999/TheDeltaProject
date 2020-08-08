using System.Collections;
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
        if (hAxis != 0) {
            _tdBaseEntity.SendMessageToBrain(tdMessageType.Move, hAxis);
        }
    }
}