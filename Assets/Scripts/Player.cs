using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        FlippingPlayer(horizontal);

        Move(horizontal, vertical);//움직여!
    }

    private void Move(int xDir, int yDir)
    {
        Vector2 newPosition = rb2D.position + new Vector2(xDir, yDir) * speed * Time.deltaTime;
        rb2D.MovePosition(newPosition);
    }

    private void FlippingPlayer(int horizontal)
    {
        if(horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Door")
        {
            Invoke("Restart", 1f);
        }
    }

    private void Restart()
    {
        GameManager.instance.NextStage();
    }
}