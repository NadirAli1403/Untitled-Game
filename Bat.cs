using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radOCircle, findGround);
        checkAlive();
        cdTimer += Time.deltaTime;
        PlayerInSight();

        if (cdTimer >= attackCd)
        {
            attack();
        }
        if (isInvincible)
        {
            iFrames();
        }
    }
    protected override void layerHandler() //the bat is flying so we want the layer weight to be consistent
    {
        myAnimator.SetLayerWeight(0, 0);
    }

}