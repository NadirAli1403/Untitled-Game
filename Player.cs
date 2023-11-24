using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Components")]
    [SerializeField] public HealthUI healthUI;
    [SerializeField] protected LayerMask findDoor;



    [Header("Slide Attributes")]
    [SerializeField] protected float slideVelocityMultiplier = 2f;
    public bool isSliding = false;
    [SerializeField] protected float slideDuration = 0.5f;
    protected float slideTimer = 0f;
    private float runSpeed = 2.0f;
    private int attackCounter = 0;
    private float cooldown = 2f;

    private float _fallSpeedYDampingChangeThreshold;


    public override void Start()
    {
        base.Start();
        speed = runSpeed;
        isPlayer = true;
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
    }
    public override void Update()
    {
        base.Update();
        direction = Input.GetAxis("Horizontal"); //this is for player
        attack();
        handleSliding();
        healthUI.UpdateHealthUI(hitPoints, maxHealth);
        //if we are falling past a speed threshold

        if (rb2D.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.isLerypingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
            
        }

        //if we are still moving up or stood still
        if (rb2D.velocity.y >=0f && !CameraManager.instance.isLerypingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            //reset so it can be called again
            CameraManager.instance.lerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
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

    protected virtual void handleSliding()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.C) && isGrounded && !isSliding) // check if the player is not already sliding, grounded and pressed the correct key
        {
            StartSlide();
        }

        // Check if sliding
        if (isSliding)
        {
            slideTimer -= Time.deltaTime; //slide timer counts down
            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }
    }
    protected virtual void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        speed = speed * slideVelocityMultiplier;
        myAnimator.SetBool("Sliding", true); // Set a trigger in your animator to play the sliding animation
    }

    protected virtual void StopSlide()
    {
        speed = 2;
        isSliding = false;
        myAnimator.SetBool("Sliding", false); // Reset the sliding animation trigger
    }

    protected override void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            base.attack();
            attackCounter ++;
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);

            switch(attackCounter)
            {

            case 1:
                myAnimator.SetTrigger("Attack_1");
                attackLength = Time.time;
                break;
            
            case 2:
                myAnimator.ResetTrigger("Attack_1");
                myAnimator.SetTrigger("Attack_2");
                attackLength = Time.time;
                break;

            case 3:
                myAnimator.ResetTrigger("Attack_2");
                myAnimator.SetTrigger("Attack_3");
                attackLength = Time.time;
                break;
            }
        }

        // Reset triggers and counter after the attack sequence finishes
        if (Input.GetMouseButtonUp(0))
        {
            myAnimator.ResetTrigger("Attack_1");
            myAnimator.ResetTrigger("Attack_2");
            myAnimator.ResetTrigger("Attack_3");
            hasHit = false;
  
        }
        if (attackCounter > 3 || Time.time - attackLength > cooldown)
        {
            attackCounter = 0;
        }


    }

}
