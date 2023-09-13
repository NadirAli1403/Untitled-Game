using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]


public abstract class Character : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb2D;
    public Animator myAnimator;
    public BoxCollider2D boxCollider;
    protected internal bool hasHit = false;
    public Transform characterTransform;

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
    [SerializeField] protected internal Transform attackPoint;



    [Header("Character Stats")]
    [SerializeField] protected int hitPoints;
    [SerializeField] protected int damage;
    protected bool isPlayer = false;

    #region monos
    public virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        characterTransform = GetComponent<Transform>();
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
        myAnimator.ResetTrigger("Hurt");
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


            hasHit = true;

            Character character = enemy.GetComponent<Character>();

            if(character!=null)
            {
                if(character.isPlayer && character.hasHit)
                {
                    Debug.Log("Test");
                }
                else
                {
                    character.myAnimator.SetTrigger("Hurt");
                    character.hitPoints -= damage;
                    Debug.Log(enemy + " " + character.hitPoints);

                }
                if(character.hitPoints <= 0 && !character.isPlayer)
                {
                    Destroy(character);
                }
            }

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

            //GameObject player = enemy.gameObject;
            //Debug.Log("Player is here");
            //Transform playerTransform = player.transform;
            //Vector2 playerPos = playerTransform.position;
            //Vector2 enemyAttackPos = attackPoint.position;
            //float distance = Vector2.Distance(playerPos, enemyAttackPos);
            //float nearMissTolerance = 1.5f;

            //if (distance < attackRange * nearMissTolerance)
            //{
            //    // The player is within the attack range, so it's a hit
            //    Debug.Log("Hit player");
            //    // Handle the hit here, e.g., reduce player health
            //}
            //else
            //{
            //    // The player dodged the attack at the last second
            //    Debug.Log("Missed player");
            //    // Handle the miss here, e.g., play a dodge animation
            //}