using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdPlayerController : MonoBehaviour {
    tdBaseEntity _tdBaseEntity;

    [Header("Key bindings")]
    public KeyCode LightKey = KeyCode.Alpha1;
    public KeyCode HeavyKey = KeyCode.Alpha2;
    public KeyCode MagicKey = KeyCode.Alpha3;

    ComboInput _currentComboInput = null;

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
        _currentComboInput = null;

        if (Input.GetKeyUp(HeavyKey))
            _currentComboInput = new ComboInput(AttackType.Heavy);
        if (Input.GetKeyUp(LightKey))
            _currentComboInput = new ComboInput(AttackType.Light);
        if (Input.GetKeyUp(MagicKey))
            _currentComboInput = new ComboInput(AttackType.Magic);

    }

    public ComboInput GetCurrentInput() {
        return _currentComboInput;
    }
}