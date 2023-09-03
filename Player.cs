using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float runSpeed = 2.0f;
    private int attackCounter = 0;
    private float cooldown = 2f;

    public override void Start()
    {
        base.Start();
        speed = runSpeed;
    }
    public override void Update()
    {
        base.Update();
        direction = Input.GetAxis("Horizontal");
        attack();
    }

    protected override void handleJumping()
    {
        if (isGrounded)
        {
            jumpTimeCounter = jumpTime;
            myAnimator.SetBool("Falling", false);
            myAnimator.ResetTrigger("Jumped");

        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump();
            stoppedJumping = false;
            //tells animator to start jump anim
            myAnimator.SetTrigger("Jumped");
        }

        if (Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0))
        {
            jump();
            jumpTimeCounter -= Time.deltaTime;
            myAnimator.SetTrigger("Jumped");
        }

        if (Input.GetButtonUp("Jump"))
        {
            stoppedJumping = true;
            jumpTimeCounter = 0;
            myAnimator.SetBool("Falling", true);
            myAnimator.ResetTrigger("Jumped");
        }
    }

    protected override void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            base.attack();
            attackCounter += 1;
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);

            if (attackCounter == 1)
            {
                myAnimator.SetTrigger("Attack_1");
                attackLength = Time.time;
            }
            else if (attackCounter == 2)
            {
                myAnimator.ResetTrigger("Attack_1");
                myAnimator.SetTrigger("Attack_2");
                attackLength = Time.time;
            }
            else if (attackCounter == 3)
            {
                myAnimator.ResetTrigger("Attack_2");
                myAnimator.SetTrigger("Attack_3");
                attackLength = Time.time;   
            }
        }

        // Reset triggers and counter after the attack sequence finishes
        if (Input.GetMouseButtonUp(0))
        {
            myAnimator.ResetTrigger("Attack_1");
            myAnimator.ResetTrigger("Attack_2");
            myAnimator.ResetTrigger("Attack_3");
  
        }
        if (attackCounter > 3 || Time.time - attackLength > cooldown)
        {
            attackCounter = 0;
        }


    }

}
