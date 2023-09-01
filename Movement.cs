using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class Movement : MonoBehaviour
{
    //attributes necessary for animation and physics
    [Header("Components")]
    private Rigidbody2D rb2D;
    private Animator myanimator;
    private CircleCollider2D playerCollider;


    private bool facingRight = true;

    [Header("Public Variables")]
    public float speed = 1.0f; //f means that the number is a floating point number
    public float horizontalMovement; //= 1, -1 or 0 based on if it is going left or right or no direction
    public bool isGrounded;

    [Header("Private Variables")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radOCircle;
    [SerializeField] private LayerMask findGround;


    [Header("Jump Details")]
    public float jumpForce;
    public float jumpTime;
    public float jumpTimeCounter;
    private bool stoppedJumping;
    public float lastJumped;
    public float jumpCooldown = 10f;


    // Start is called before the first frame update
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        myanimator = GetComponent<Animator>();
        playerCollider = GetComponent<CircleCollider2D>();
        jumpTimeCounter = jumpTime;
    }





    // Update is called once per frame
    //handles physics inputs
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,radOCircle, findGround);

        if(isGrounded)
        {
            jumpTimeCounter = jumpTime;
            myanimator.SetBool("Falling", false);
            myanimator.ResetTrigger("Jumped");

        }

        Run();
        Jump();
    }

    #region Run
    private void Run()
    {
        //check for inputs
        horizontalMovement = Input.GetAxis("Horizontal");
        rb2D.velocity = new Vector2(horizontalMovement * speed, rb2D.velocity.y);
        Flip(horizontalMovement);
        myanimator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
    }
    #endregion


    #region Jump
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && Time.time - lastJumped > jumpCooldown)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            stoppedJumping = false;
            //tells animator to start jump anim
            myanimator.SetTrigger("Jumped");
            lastJumped = Time.time;
        }

        if (Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0))
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
            myanimator.SetTrigger("Jumped");
        }

        if (Input.GetButtonUp("Jump"))
        {
            stoppedJumping = true;
            jumpTimeCounter = 0;
            myanimator.SetBool("Falling", true);
            myanimator.ResetTrigger("Jumped");
        }


        if (rb2D.velocity.y < 0)
        {
            myanimator.SetBool("Falling", true);
            myanimator.ResetTrigger("Jumped");
        } 
        
        #endregion
    }

    private void Flip(float horizontal)
    {
        if(horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void FixedUpdate()
    {
        LayerHandler();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOCircle);
    }
    private void LayerHandler()
    {
        if (!isGrounded)
        {
            myanimator.SetLayerWeight(1, 1);
        }
        else
        {
            myanimator.SetLayerWeight(1, 0);
        }
    }
}