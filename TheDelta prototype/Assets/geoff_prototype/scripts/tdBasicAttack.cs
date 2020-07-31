using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdBasicAttack : MonoBehaviour {
    private Animator _animCtrl;
    [Header("Combos")]
    public string[] ComboList;
    public int ComboNumber { private set; get; }
    int _maxCombo = 3;
    public float ResetTime;
    float _timeBeforeReset;
    tdPlayerMovement _movement;

    // Start is called before the first frame update
    void Start() {
        _animCtrl = GetComponent<Animator>();
        _movement = GetComponent<tdPlayerMovement>();
    }

    // Update is called once per frame
    void Update() {
        //subject to change
        BasicAttack();
    }

    void BasicAttack() {
        if (Input.GetButtonDown("Fire1") && ComboNumber < _maxCombo 
            && _movement.PlayerState != PlayerState.Jumping) {
            //subject to change
            _movement.PlayerState = PlayerState.Attacking;
            _animCtrl.SetTrigger(ComboList[ComboNumber]);
            ComboNumber++;
            _timeBeforeReset = 0;
        }
        if (ComboNumber > 0) {
            _timeBeforeReset += Time.deltaTime;
            if (_timeBeforeReset > ResetTime) {
                ComboNumber = 0;
                foreach (string cn in ComboList) {
                    _animCtrl.ResetTrigger(cn);
                }

                _movement.PlayerState = PlayerState.Idle;
            }
        }
        ResetTime = ComboNumber == _maxCombo ? 2f : 1f;
    }
}
