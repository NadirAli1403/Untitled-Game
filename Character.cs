using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]


public abstract class Character : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb2D;
    public Animator myAnimator;
    public BoxCollider2D boxCollider;
    protected bool hasHit = false;

    [Header("Movement Variables")]
    [SerializeField] protected float speed = 1.0f;
    [SerializeField] protected float direction;
    [SerializeField] protected bool facingRight;

    [Header("Jump Variables")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float jumpTime;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float radOCircle;
    [SerializeField] protected LayerMask findGround;
    protected bool isGrounded;
    protected float jumpTimeCounter;
    protected bool stoppedJumping;

    [Header("Attack Variables")]
    [SerializeField] protected LayerMask enemyLayers;
    [SerializeField] protected float attackLength;
    [SerializeField] protected float attackRange = 0.5f;
    [SerializeField] protected Transform attackPoint;



    //[Header("Character Stats")]


    #region monos
    public virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;

    }

    public virtual void Update()
    {
        // handles input
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radOCircle, findGround);

        if (rb2D.velocity.y < 0)
        {
            myAnimator.SetBool("Falling", true);
            myAnimator.ResetTrigger("Jumped");
        }

        handleJumping();
    }

    public virtual void FixedUpdate()
    {
        // handles mechanics/physics
        handleMovement();
        layerHandler();
    }

    #endregion

    #region mechanics
    protected virtual void move()
    {
        rb2D.velocity = new Vector2(direction * speed, rb2D.velocity.y);

    }

    protected void jump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
    }

    protected virtual void attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy);
            hasHit = true;
        }
    }
    #endregion

    #region subMechanics
    protected virtual void handleMovement()
    {
        move();
        myAnimator.SetFloat("Speed", Mathf.Abs(direction));
        turnAround(direction);
    }

    protected virtual void handleJumping()
    {
        jump();
    }


    protected void turnAround(float horizontal)
    {
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    protected void layerHandler()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOCircle);
    }
    protected virtual void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

        #endregion
