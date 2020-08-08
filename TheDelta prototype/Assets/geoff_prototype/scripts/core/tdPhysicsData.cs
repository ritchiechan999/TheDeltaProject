using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class tdPhysicsData 
{
    public const float GlobalGravity = -9.8f;
    static float _gravityScale;

    public static void ModifyPhysics(bool onGround, Rigidbody rb, float customGravity, 
                                        float dragScale, float fallMultiplier, float horizontalMovement) {
        if (onGround) {
            rb.drag = Mathf.Abs(horizontalMovement) < 0.4f ? dragScale : 0;
            _gravityScale = 0;
        } else {
            _gravityScale = customGravity;
            rb.drag = dragScale * 0.15f;
            float rbVelocityY = rb.velocity.y;
            Vector3 newGravity = GlobalGravity * _gravityScale * Vector3.up;
            if (rbVelocityY < 0) {
                newGravity *= fallMultiplier;
            } else if (rbVelocityY > 0 && !Input.GetButton("Jump")) {
                newGravity *= fallMultiplier / 2;
            }
            rb.AddForce(newGravity, ForceMode.Acceleration);
        }
    }
}
