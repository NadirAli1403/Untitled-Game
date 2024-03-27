using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("Components")]
    public BoxCollider2D boxCollider;



    [Header("Enemy Specific")]
    [SerializeField] protected float attackCd;
    protected float cdTimer = 0f;
    protected float attackAnimationDuration;



    public override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        PlayerInSight();
        cdTimer += Time.deltaTime;


        if (cdTimer >= attackCd)
        {
            attack();
        }
    }

    protected override void checkAlive()
    {
        base.checkAlive();
        if (hitPoints <= 0)
        {
            //remove the rigid body component to allow player to pass through
            boxCollider.enabled = false;
            this.enabled = false;
        }
    }
    private IEnumerator AttackCoroutine()
    {
        myAnimator.SetBool("Attack", true);
        yield return new WaitForSeconds(attackAnimationDuration); // Wait for the attack animation to finish
        myAnimator.SetBool("Attack", false); // Reset the attack animation state
    }

    protected override void attack()
    {
        base.attack();
        cdTimer = 0f;
        if (hasHit)
        {
            Debug.Log("Starting Coroutine");
            StartCoroutine(AttackCoroutine());
            hasHit = false;
        }
    }

    protected bool PlayerInSight() //this is for enemy
    {
        RaycastHit2D hitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 3, enemyLayers);
        RaycastHit2D hitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 3, enemyLayers);

        if (hitLeft.collider != null)
        {
            direction = -1;
        }

        else if (hitRight.collider != null)
        {
            direction = 1;
        }

        else
        {
            direction = 0;
        }

        return hitLeft.collider != null || hitRight.collider != null;


    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Calculate the end point of the BoxCast
        Vector2 boxCastEnd = (Vector2)transform.position + Vector2.left * 3;

        // Draw the start and end points
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawSphere(boxCastEnd, 0.1f);

        // Draw the BoxCast
        Gizmos.DrawLine(transform.position, boxCastEnd);
        Gizmos.DrawWireCube(boxCastEnd, boxCollider.bounds.size);

        Vector2 boxCastEndRight = (Vector2)transform.position + Vector2.right * 3;

        // Draw the start and end points
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawSphere(boxCastEndRight, 0.1f);

        // Draw the BoxCast
        Gizmos.DrawLine(transform.position, boxCastEndRight);
        Gizmos.DrawWireCube(boxCastEndRight, boxCollider.bounds.size);

    }
}
