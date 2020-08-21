using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerController : MonoBehaviour {
    tdEntity _tdBaseEntity;

    [Header("Key bindings")]
    public KeyCode LightKey = KeyCode.Alpha1;
    public KeyCode HeavyKey = KeyCode.Alpha2;
    public KeyCode MagicKey = KeyCode.Alpha3;

    tdComboInput _currentComboInput = null;

    void Start() {
        _tdBaseEntity = this.GetComponent<tdEntity>();
    }

    void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        _tdBaseEntity.SendMessageToBrain(tdMessageType.Move, hAxis);

        _tdBaseEntity.OnGround = Physics.Raycast(transform.position + _tdBaseEntity.ColliderOffset,
                                                    Vector2.down, _tdBaseEntity.GroundLength, tdData.GroundLayer);
        if (Input.GetKeyDown(KeyCode.Space)) {
            _tdBaseEntity.JumpTimer = Time.time + _tdBaseEntity.JumpDelay;
        }

        _currentComboInput = null;

        if (Input.GetKeyUp(HeavyKey))
            _currentComboInput = new tdComboInput(tdAttackType.Heavy);
        if (Input.GetKeyUp(LightKey))
            _currentComboInput = new tdComboInput(tdAttackType.Light);
        if (Input.GetKeyUp(MagicKey))
            _currentComboInput = new tdComboInput(tdAttackType.Magic);

    }

    public tdComboInput GetCurrentInput() {
        return _currentComboInput;
    }
}