using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdMeleeAI : tdEntity
{
    [Header("AI details")]
    public float navTime = 2;
    private float resetNavTime = 0;
    private float direction = 0;

    [Space]
    public LayerMask targetMask;
    public float detectRadius = 2f;
    public Color detectColor = Color.green;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        resetNavTime = navTime;
        direction = 1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        OnGround = Physics.Raycast(transform.position + ColliderOffset, Vector2.down, GroundLength, tdData.GroundLayer);

        //jump ai calculation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpTimer = Time.time + JumpDelay;
        }

        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector3 currentPos = this.transform.position;
        Collider[] detected = Physics.OverlapSphere(currentPos, detectRadius, targetMask);
        if (detected.Length > 0)
        {
            detectColor = Color.red;

        }
        else
        {
            detectColor = Color.green;
        }
    }

    private void Patrolling()
    {
        SendMessageToBrain(tdMessageType.Move, direction);
        navTime -= Time.deltaTime;
        if (navTime <= 0)
        {
            //change direction
            direction = -direction;
            navTime = resetNavTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectColor;
        Gizmos.DrawWireSphere(this.transform.position, detectRadius);
    }
}