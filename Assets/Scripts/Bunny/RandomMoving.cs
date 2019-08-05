using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoving : MonoBehaviour
{
    public float speed;
    public Sprite[] sprites;

    private float lastPosX;
    private float nowTime; //움직인 시간
    private float moveDelay; //움직이기로 한 시간
    private float restDelay; //가만히 있기로 한 시간
    private bool isMoving; //움직이고 있나?
    private Vector3 moveDir; //움직이려는 방향

    private void Start()
    {
        lastPosX = transform.position.x;
        MovingInitialize();
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-100 * transform.position.y);
        FlippedByPlayer();
        FlipByItself();
        Move();
    }

    private void MovingInitialize()
    {
        moveDelay = Random.Range(1f, 2f);
        restDelay = Random.Range(2f, 4f);
        nowTime = 0;
        isMoving = false;
    }

    private void Move()
    {
        nowTime += TimeManager.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir * speed, TimeManager.deltaTime * speed);

        if (GetComponent<Bunny>().isInvincible == true)
        {
            RunAway();
            return;
        }

        if (isMoving == true)
        {
            if (nowTime >= moveDelay)
            {
                isMoving = false;
                nowTime = 0;
                moveDir = Vector2.zero;
            }
        }
        else
        {
            if (nowTime >= restDelay)
            {
                isMoving = true;
                nowTime = 0;
                moveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            }
        }
    }

    private void RunAway()
    {
        GetComponent<Collider2D>().enabled = false;
        speed = 5f;

        Vector2 runDir = Vector2.zero;
        if (transform.position.x - Camera.main.transform.position.x >= 0) runDir = Vector2.right;
        else runDir = Vector2.left;

        moveDir = runDir;

        if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Door")
        {
            if (GetComponent<Bunny>().isInvincible == true) return;

            if (coll.gameObject.name.Contains("Left")) moveDir = new Vector2(0.6f, moveDir.y);
            if (coll.gameObject.name.Contains("Right")) moveDir = new Vector2(-0.6f, moveDir.y);
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Cell")
        {
            if (transform.position.y > 0) moveDir = new Vector2(moveDir.x, -0.6f);
            if (transform.position.y < 0) moveDir = new Vector2(moveDir.x, 0.6f);
        }
        if (coll.gameObject.tag == "Bunny")
        {
            if (GetComponent<Bunny>().isInvincible == true) return;

            moveDir *= (-1);
        }
    }

    public void StopByAttacked()
    {
        moveDir = Vector2.zero;
    }

    private void FlipByItself()
    {
        if (moveDir.x > 0) FlipBunny(true);
        else if (moveDir.x < 0) FlipBunny(false);
    }

    private void FlippedByPlayer()
    {
        float moveDelta = 0.1f;

        if (lastPosX + moveDelta <= transform.position.x)
        {
            FlipBunny(true);
            lastPosX = transform.position.x;
        }
        if (lastPosX - moveDelta >= transform.position.x)
        {
            FlipBunny(false);
            lastPosX = transform.position.x;
        }
    }

    public void FlipForTalk()
    {
        float myPosX = transform.position.x;
        float playerPosX = Player.instance.gameObject.transform.position.x;

        if (myPosX > playerPosX) FlipBunny(false);//transform.rotation = Quaternion.Euler(0, 180, 0);
        else FlipBunny(true);//transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void FlipBunny(bool isFacingRight)
    {
        if(isFacingRight == true) GetComponent<SpriteRenderer>().sprite = sprites[0];
        else GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
}
