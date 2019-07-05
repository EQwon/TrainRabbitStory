using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    //public string bunnyName;    //토끼의 이름
    public bool isSitting; //토끼가 앉아있는지
    public bool isInteractive; //상호작용 할 수 있는지
    public int hp;
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
        if(isSitting == false)
        {
            GetComponent<SpriteRenderer>().sortingOrder = (int)(-100 * transform.position.y);
        }

        if (hp <= 0)
        {
            Debug.Log(gameObject.name + "가 죽었습니다.");
            Destroy(gameObject);
        }

        Flip();
        if(isSitting == false) Move();
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
                targetPos = transform.position + moveDir;

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

    public void Attacked(int damage)
    {
        hp = hp - damage;
        GameManager.instance.MP -= 10;
        Debug.Log(gameObject.name + "가 공격받음.");
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

    public void FlipBunny(GameObject player)
    {
        if (isSitting) return;

        float myPosX = transform.position.x;
        float playerPosX = player.transform.position.x;

        if(myPosX > playerPosX) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}