using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlayer : MonoBehaviour
{
    private bool isOverlapped;
    [SerializeField] private Player player;

    public void Start()
    {
        player = GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && isOverlapped)
        {
            teleportBack();
        }
    }

    private void teleportBack()
    {
        //player.position = new Vector3((float)-14.995, (float)-0.557, 0);
        Debug.Log("Teleporting");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isOverlapped = true;
            Debug.Log("Treuy");
        }

        else
        {
            isOverlapped = false;
        }
    }
}
