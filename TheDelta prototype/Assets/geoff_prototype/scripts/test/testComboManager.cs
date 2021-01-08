using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testComboManager : MonoBehaviour
{
    public static testComboManager instance { private set; get; }

    public bool canReceiveInput = false;
    public bool inputReceived = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canReceiveInput)
            {
                inputReceived = true;
                canReceiveInput = false;

                Debug.Log("Button Smashed !");
            }
        }
    }

    public void InputManager()
    {
        canReceiveInput = !canReceiveInput;
    }
}
