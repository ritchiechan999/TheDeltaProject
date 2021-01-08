﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class testComboManager : MonoBehaviour
{
    public static testComboManager instance { private set; get; }

    [Header("Combo")]
    public float inputResetLeeway = 0.2f;

    [Space]
    public bool canReceiveInput = false;
    public bool inputReceived = false;
    public KeyCode currentInputKey;

    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (Input.anyKeyDown)
        {
            if (canReceiveInput)
            {
                inputReceived = true;
                canReceiveInput = false;

                foreach (KeyCode keyCode in keyCodes)
                {
                    if (Input.GetKey(keyCode))
                    {
                        Debug.Log("KeyCode down: " + keyCode);
                        currentInputKey = keyCode;
                        Invoke(nameof(ResetCanReceivedInput), inputResetLeeway);
                        break;
                    }
                }
            }
        }
    }

    private void ResetCanReceivedInput()
    {
        currentInputKey = KeyCode.None;
        inputReceived = false;
        canReceiveInput = true;
    }

    public void InputManager()
    {
        canReceiveInput = !canReceiveInput;
    }
}

public enum tdComboAttack
{
    Default = 0,
    LightAttackOne = 1,
    LightAttackTwo = 2,
    
}
