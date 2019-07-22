using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoving : MonoBehaviour
{
    public float speed;

    private float lastPosX;
    private float nowTime; //움직인 시간
    private float moveDelay; //움직이기로 한 시간
    private float restDelay; //가만히 있기로 한 시간
    private bool isMoving; //움직이고 있나?
    private Vector3 moveDir; //움직이려는 방향
    private Vector3 targetPos;

    private void Start()
    {
        lastPosX = transform.position.x;
        MovingInitialize();
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-100 * transform.position.y);
        Flip();
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

        if (isMoving == true)
        {
            if (nowTime >= moveDelay)
            {
                isMoving = false;
                nowTime = 0;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        }
        else
        {
            if (nowTime >= restDelay)
            {
                isMoving = true;
                nowTime = 0;
                moveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                targetPos = transform.position + moveDir * speed;

                int cellNum = (int)(transform.position.x + 10) / 20;
                int afterMoveCellNum = (int)(targetPos.x + 10) / 20;
                if (cellNum != afterMoveCellNum)
                {
                    targetPos = transform.position + new Vector3(-moveDir.x, moveDir.y, 0);
                }

                if (targetPos.y < -2.2f || targetPos.y > 2f)
                {
                    targetPos = transform.position + new Vector3(moveDir.x, -moveDir.y, 0);
                }
            }
        }
    }

    private void Flip()
    {
        float moveDelta = 0.1f;

        if (lastPosX + moveDelta <= transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            lastPosX = transform.position.x;
        }
        if (lastPosX - moveDelta >= transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            lastPosX = transform.position.x;
        }
    }

    public void FlipBunny()
    {
        float myPosX = transform.position.x;
        float playerPosX = Player.instance.gameObject.transform.position.x;

        if (myPosX > playerPosX) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
