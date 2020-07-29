using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //temporary skill effects
    
    public EffectInfoDic[] EI;
    [System.Serializable]
    public class EffectInfoDic
    {
        public GameObject skillSlash;
        public Transform skillPos;
    }

    #region stats
    //public adjustable stat
    public float IdleLimit;
    public float Speed;
    public float JumpHeight;
    public float JumpForce;
    public bool isGrounded = true;
    #endregion
    public GameObject playerObj;
    public PlayerMoveSet moveset;

    #region private conditions
    //private
    bool idleTimerSwitch;
    float idleTimer;
    bool stillAttacking;
    int attackComboCount;
    int leftMouseButtonPressed;
    Animator anim;
    Rigidbody rb;
    float holdDownTime;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        //PlayerHorizontalMovement();
        
        Movements();
        Jump();
        SideScrollerControls();
        //PlayerHorizontalMovement();
        //rb.AddForce(movement * Speed);

        

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("isJumping", true);
        }

        if(isGrounded == true)
        {
            anim.SetBool("isJumping", false);
        }

    }

    public void Movements()
    {

        switch (moveset)
        {
            case PlayerMoveSet.HorizontalMovement:
                PlayerHorizontalMovement();
                break;
            case PlayerMoveSet.Jump:
                Jump();
                break;
        }

    }
    
    public void Jump()
    {
        //float y = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Debug.Log("jump");
            isGrounded = false;
            rb.AddForce((new Vector3(0, JumpHeight, 0)) * JumpForce);
        }
    }
    public void PlayerHorizontalMovement()
    {
        float x = Input.GetAxis("Horizontal");
        
        //rb.velocity = new Vector3(x, this.transform.position.y, 0) * Speed * Time.deltaTime;

        Vector3 movement = new Vector3(x, 0, 0);


        //Vector3 newPosition = new Vector3(x, 0.0f, 0.0f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        transform.LookAt(movement + transform.position);

        transform.Translate(movement * Speed * Time.deltaTime, Space.World);

        //
        //if(Input.GetKeyDown(KeyCode.D))
        //{
        //    playerObj.transform.rotation = Quaternion.RotateTowards(playerObj.transform.rotation, Quaternion.LookRotation(new Vector3(90, 0, 0)), 900* Time.deltaTime);
        //}
    }
    public void ThirdPersonControl()
    {

    }
    public void SideScrollerControls()
    {
        if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A)))
        {
            stillAttacking = false;
            attackComboCount = 0;
            anim.SetInteger("attackCombo", attackComboCount);
            leftMouseButtonPressed = 0;
            anim.SetBool("isAttacking", false);
            holdDownTime += Time.deltaTime;
            anim.SetFloat("hold", holdDownTime);
            anim.SetBool("isRunning", true);
            print("running");
            //if (holdDownTime >= 0.5f)
            //{
            //    anim.SetBool("isRunning", false);
            //    anim.SetBool("isSprinting", true);
            //}

        }
        else if ((Input.GetKeyUp(KeyCode.D)) || (Input.GetKeyUp(KeyCode.A)))
        {
            print("not running");
            anim.SetBool("isRunning", false);
            anim.SetBool("isSprinting", false);
            holdDownTime = 0f;
        }
        if (idleTimerSwitch)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > IdleLimit)
            {
                anim.SetBool("idleTime", true);
                stillAttacking = false;
                attackComboCount = 0;
                anim.SetInteger("attackCombo", attackComboCount);
                leftMouseButtonPressed = 0;
                anim.SetBool("isAttacking", false);
            }
            //if(idleTimer > )
        }

        //basic attacks = 3 combo slash 
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("idleTime", false);
            idleTimer = 0;
            //stillAttacking = !stillAttacking;
            anim.SetBool("isAttacking", true);
            leftMouseButtonPressed++;

            if (leftMouseButtonPressed > 0 && stillAttacking == false)
            {
                Debug.Log("isattack");
                if (attackComboCount > 3)
                {
                    attackComboCount = 0;
                }
                if (attackComboCount <= 3)
                {
                    Debug.Log("isAttacking");
                    attackComboCount++;
                    leftMouseButtonPressed = 0;
                }
                anim.SetInteger("attackCombo", attackComboCount);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            idleTimerSwitch = true;
        }
    }
    public void SkillEffect(int skillNumber)
    {

        GameObject Instance = Instantiate(EI[skillNumber].skillSlash, EI[skillNumber].skillPos.transform.position, EI[skillNumber].skillPos.rotation) as GameObject;
        Destroy(Instance, 5);
    }
    public void CheckIfAttacking()
    {
        stillAttacking = !stillAttacking;
        //if(stillAttacking == false)
        //{
        //    leftMouseButtonPressed = 0;
        //}
        

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            //anim.SetTrigger("landing");
            isGrounded = true;
            
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            
            isGrounded = false;

        }
    }
}

public enum PlayerMoveSet
{
    HorizontalMovement,
    Jump,
    Downfast,
    

    
}
