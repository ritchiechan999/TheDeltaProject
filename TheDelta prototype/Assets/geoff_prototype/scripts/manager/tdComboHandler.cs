using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO a system/manager idk hahaha this is the best for now
public class tdComboHandler : MonoBehaviour {
    [Header("Components and stuff")]
    Animator _anim;
    tdPlayerController _playerController;
    tdEntity _entity;

    [Header("Attacks")]
    public tdAttack HeavyAttack;
    public tdAttack LightAttack;
    public tdAttack MagicAttack;
    public List<tdCombo> Combos;
    [Space]
    public float ComboLeeway = 1f;
    tdAttack _currentAttack = null;
    tdComboInput _lastInput = null;
    List<int> _currentCombos = new List<int>();

    float _attackTimer = 0;
    float _leeway = 0;
    bool _skip = false;
    //
    public bool IsOnCombo;


    //temp mouse logic
    int numberOfClicks;
    // Start is called before the first frame update
    void Start() {
        _anim = this.GetComponent<Animator>();
        _playerController = this.GetComponent<tdPlayerController>();
        _entity = this.GetComponent<tdEntity>();
        PrimeCombos();
    }

    void PrimeCombos() {
        for (int i = 0; i < Combos.Count; i++) {
            tdCombo c = Combos[i];
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
        MouseComboLogic();
    }

    void MouseComboLogic()
    {
        if(_currentCombos.Count > 0)
        {
            Debug.Log(_currentAttack);
            

           
        }
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
        tdComboInput input = null;
        if(_playerController != null) {
            input = _playerController.GetCurrentInput();
            Debug.Log(input);
        }

        if (input == null)
            return;

        _lastInput = input;

        //continue combo reference
        List<int> removeCombo = new List<int>();
        for (int i = 0; i < _currentCombos.Count; i++) {
            tdCombo c = Combos[_currentCombos[i]];
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
            tdCombo c = Combos[_currentCombos[i]];
            c.ResetCombo();
        }
        _currentCombos.Clear();
        _anim.SetInteger("anim_state", 0);
        IsOnComboReset();
    }

    void FinalAttack(tdAttack atk) {
        IsOnCombo = true;
        _currentAttack = atk;
        _attackTimer = atk.LengthDuration;
        Debug.Log(_currentAttack.Name + " ");// + _currentAttack.LengthDuration);
        _entity.SendMessageToBrain(tdMessageType.Attack, atk);
        Invoke(nameof(IsOnComboReset), _attackTimer);
    }

    //do something regards to this haha
    void ChainAttack(tdAttack chainAtk) {
        IsOnCombo = true;
        //Debug.Log(chainAtk.Name);
        _entity.SendMessageToBrain(tdMessageType.Attack, chainAtk);
    }

    void IsOnComboReset() {
        IsOnCombo = false;
    }

    tdAttack GetAttackFromType(tdAttackType type) {
        switch (type) {
            case tdAttackType.Heavy:
                return HeavyAttack;
            case tdAttackType.Light:
                return LightAttack;
            case tdAttackType.Magic:
                return MagicAttack;
            default:
                return null;
        }
    }
}