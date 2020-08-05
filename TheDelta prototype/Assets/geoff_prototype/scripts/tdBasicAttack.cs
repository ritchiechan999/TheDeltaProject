using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdBasicAttack : MonoBehaviour {
    private Animator _animCtrl;
    [Header("Combos")]
    public string[] ComboList;
    public int ComboNumber; /*{ private set; get; }*/
    public bool CanAttack;
    int _maxCombo = 3;
    public float ResetTime;
    float _timeBeforeReset;
    tdPlayerMovement _movement;
    float _attackEndCountDownTime = 0.3f; //hardcoded for test-ritchie
    public bool attackEndCountDownSwitch;

    public GameObject ShowBranchAttack; //temporary


    public Event lastKeyPressed; //temporary to prevent spam
    public KeyCode keyPressed;

    public AnimComboStrings[] AnimNames;

    
    // Start is called before the first frame update
    void Start() {
        _animCtrl = GetComponent<Animator>();
        _movement = GetComponent<tdPlayerMovement>();
        

        //_attackEndCountDownTime = _timeBeforeReset;
        //ShowBranchAttack.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        //subject to change
        BasicAttack();

        //need find a better way for this
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            keyPressed = KeyCode.Mouse0;
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            keyPressed = KeyCode.Mouse0;
        }
        

    }
    void OnGUI()
    {
        lastKeyPressed = Event.current;
        if (lastKeyPressed.isKey)
        {
            keyPressed = lastKeyPressed.keyCode;
        }
        
        //if (lastKeyPressed.isKey)
        //{
        //    //lastKeyPressed.keyCode = keyPressed;
        //    Debug.Log("Detected key code: " + lastKeyPressed.keyCode);
        //}
    }
    void BasicAttack() {
        if (Input.GetButtonDown("Fire1") && ComboNumber < _maxCombo 
            && _movement.PlayerState != PlayerState.Jumping) 
        {
            //subject to change
            


            _movement.PlayerState = PlayerState.Attacking;
            //_animCtrl.SetTrigger(ComboList[ComboNumber]);
            _animCtrl.SetTrigger(AnimNames[ComboNumber].NormalAttackName);
            
            _timeBeforeReset = 0;
            
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; AnimNames[ComboNumber].branchAttackName.Length > i; i++)
            {

                Debug.Log(AnimNames[ComboNumber].branchAttackName[0]);
                _animCtrl.SetTrigger(AnimNames[ComboNumber].branchAttackName[0]);
            }
            
        }
        else
        {
            _animCtrl.ResetTrigger(AnimNames[ComboNumber].branchAttackName[0]);
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

    public void ComboAttack()
    {
        if(keyPressed == KeyCode.Mouse0)
        {
            ComboNumber++;
        }

        if(keyPressed == KeyCode.Alpha1)
        {

        }
    }
    
    //temp couroutine
    public void SlowTimeEndAttack()
    {
        //Maybe have some slow down time in attack
        //StartCoroutine(SlowDownTimeForFewSeconds());
        //attackEndCountDownSwitch = true;
        //ComboNumber++;

    }
    public void CanBranchAttack()
    {
        CanAttack = !CanAttack;
    }
    //slow down attack to joint combo
    IEnumerator SlowDownTimeForFewSeconds()
    {
        Time.timeScale = 0.3f;       
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1f;
    }
}

[System.Serializable]
public class AnimComboStrings
{
    public string NormalAttackName;
    public string[] branchAttackName;
}