using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO a system/manager idk hahaha this is the best for now
public class tdComboHandler : MonoBehaviour {
    [Header("Components and stuff")]
    Animator _anim;
    tdPlayerController _playerController;
    tdBaseEntity _entity;

    [Header("Attacks")]
    public Attack HeavyAttack;
    public Attack LightAttack;
    public Attack MagicAttack;
    public List<Combo> Combos;
    [Space]
    public float ComboLeeway = 1f;
    Attack _currentAttack = null;
    ComboInput _lastInput = null;
    List<int> _currentCombos = new List<int>();

    float _attackTimer = 0;
    float _leeway = 0;
    bool _skip = false;

    // Start is called before the first frame update
    void Start() {
        _anim = this.GetComponent<Animator>();
        _playerController = this.GetComponent<tdPlayerController>();
        _entity = this.GetComponent<tdBaseEntity>();

        PrimeCombos();
    }

    void PrimeCombos() {
        for (int i = 0; i < Combos.Count; i++) {
            Combo c = Combos[i];
            c.OnInputted = () => {
                _skip = true;
                FinalAttack(c.ComboAttack);
                Invoke(nameof(ResetCombos), c.ComboAttack.LengthDuration);
            };
        }
    }

    // Update is called once per frame
    void Update() {
        ComboLogic();
    }

    void ComboLogic() {
        //when final atk is happenning
        if (_currentAttack != null) {
            if (_attackTimer > 0)
                _attackTimer -= Time.deltaTime;
            else
                _currentAttack = null;
            return;
        }

        //check if player is beyond leeway time
        if (_currentCombos.Count > 0) {
            _leeway += Time.deltaTime;
            if (_leeway >= ComboLeeway) {
                if (_lastInput != null) {
                    // FinalAttack(GetAttackFromType(_lastInput.Type));
                    _lastInput = null;
                }
                ResetCombos();
            }
        } else
            _leeway = 0;

        //inputs.. TODO separate the one on player and AI
        ComboInput input = null;
        if(_playerController != null) {
            input = _playerController.GetCurrentInput();
        }

        if (input == null)
            return;

        _lastInput = input;

        //continue combo reference
        List<int> removeCombo = new List<int>();
        for (int i = 0; i < _currentCombos.Count; i++) {
            Combo c = Combos[_currentCombos[i]];
            if (c.ContinueCombo(input) && !c.DoComboAtk) {
                //TODO send message to brain
                ChainAttack(GetAttackFromType(_lastInput.Type));
                _leeway = 0;
            } else {
                removeCombo.Add(i);
            }
        }

        if (_skip) {
            _skip = false;
            return;
        }

        //checking combo list
        for (int i = 0; i < Combos.Count; i++) {
            if (_currentCombos.Contains(i))
                continue;
            if (Combos[i].ContinueCombo(input)) {
                //TODO send message to entity brain
                ChainAttack(GetAttackFromType(_lastInput.Type));
                _currentCombos.Add(i);
                _leeway = 0;
            }
        }

        //this will cause bugs later...
        foreach (int i in removeCombo) {
            _currentCombos.RemoveAt(i);
        }

        //get the combo
        if (_currentCombos.Count <= 0) {
            //TODO send message to brain
            FinalAttack(GetAttackFromType(input.Type));
        }
    }


    void ResetCombos() {
        _leeway = 0;
        for (int i = 0; i < _currentCombos.Count; i++) {
            Combo c = Combos[_currentCombos[i]];
            c.ResetCombo();
        }
        _currentCombos.Clear();
        _anim.SetInteger("anim_state", 0);
    }

    void FinalAttack(Attack atk) {
        _currentAttack = atk;
        _attackTimer = atk.LengthDuration;
        Debug.Log(_currentAttack.Name + " ");// + _currentAttack.LengthDuration);
        //_anim.SetInteger("anim_state", (int)atk.AnimState);
        _entity.SendMessageToBrain(tdMessageType.Attack, atk);
    }

    //do something regards to this haha
    void ChainAttack(Attack chainAtk) {
        Debug.Log(chainAtk.Name);
        //_anim.SetInteger("anim_state", (int)chainAtk.AnimState);
        _entity.SendMessageToBrain(tdMessageType.Attack, chainAtk);
    }

    Attack GetAttackFromType(AttackType type) {
        switch (type) {
            case AttackType.Heavy:
                return HeavyAttack;
            case AttackType.Light:
                return LightAttack;
            case AttackType.Magic:
                return MagicAttack;
            default:
                return null;
        }
    }
}