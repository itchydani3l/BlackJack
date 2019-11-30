using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public Sprite jsp;
    public Sprite isp;
    public Sprite wsp1;
    public Sprite wsp2;
    private int wspind = 0;

    private SpriteRenderer spriteRenderer;
 //   private Animator animator;

    // Use this for initialization
    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();
        //        animator = GetComponent<Animator> ();
        player = true;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f)); // edited line to include -0.1f  to prevent flipping while stationary
        if (flipSprite) 
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if (velocity.y != 0.00f) spriteRenderer.sprite = jsp;
        else if (move.x != 0.0f)
        {
            if (wspind == 12) { spriteRenderer.sprite = wsp1; wspind = 0; }
            else if (wspind == 6) { spriteRenderer.sprite = wsp2; wspind++; }
            else wspind++;
        }
        else spriteRenderer.sprite = isp;

        //        animator.SetBool ("grounded", grounded);
        //        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}