using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f,0);
    public LayerMask enemyLayers;

    Rigidbody2D rb2D;


    public void Awake()
    {
        
        rb2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2D.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null && collision.gameObject.layer==enemyLayers)
        {
            character.damaged(damage);
            Destroy(gameObject);
        }
    }
}
