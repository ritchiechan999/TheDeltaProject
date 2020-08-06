using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdBasicAttack : MonoBehaviour {
    private Animator _animCtrl;
    [Header("Combos")]
    public string[] ComboList;
    public int ComboNumber; /*{ private set; get; }*/
    int _comboNumberArray;
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
        //get mouse click as last pressed button
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            keyPressed = KeyCode.Mouse0;
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            keyPressed = KeyCode.Mouse0;
        }



        Debug.Log(_comboNumberArray);

    }
    void OnGUI()
    {
        //this is to find ways to prevent mouse input spam;
        //get last key pressed on the keyboard
        lastKeyPressed = Event.current;
        if (lastKeyPressed.isKey)
        {
            keyPressed = lastKeyPressed.keyCode;
        }
        
        
    }
    void BasicAttack() {

        if (ComboNumber > 0)
        {
            _comboNumberArray = ComboNumber - 1;
        }

        if (Input.GetButtonDown("Fire1") && ComboNumber < _maxCombo 
            && _movement.PlayerState != PlayerState.Jumping) 
        {
            //subject to change


            ComboNumber++;
            _movement.PlayerState = PlayerState.Attacking;
            //_animCtrl.SetTrigger(ComboList[ComboNumber]);
            _animCtrl.SetTrigger(AnimNames[_comboNumberArray].NormalAttackName);
            
            _timeBeforeReset = 0;
            
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; AnimNames[_comboNumberArray].branchAttackName.Length > i; i++)
            {

                Debug.Log(AnimNames[_comboNumberArray].branchAttackName[0]);
                _animCtrl.SetTrigger(AnimNames[_comboNumberArray].branchAttackName[0]);
            }
            
        }
        else
        {
            _animCtrl.ResetTrigger(AnimNames[_comboNumberArray].branchAttackName[0]);
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


    //at the end of the animation if the last button press is mouse it will ++ , else if " 1 " , it will go to branch skill

    public void ComboAttack()
    {
        
        if(keyPressed == KeyCode.Mouse0)
        {
            //ComboNumber++;
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