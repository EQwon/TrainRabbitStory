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

    private void Start()
    {
        lastPosX = transform.position.x;
        //StartCoroutine(Move());
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
    }

    /*private IEnumerator Move()
    {
        float time = Random.Range(2f, 4f);
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

        gameObject.GetComponent<Rigidbody2D>().velocity = dir * speed;

        yield return new WaitForSeconds(time);

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        time = Random.Range(4f, 8f);

        yield return new WaitForSeconds(time);

        yield return StartCoroutine(Move());
    }*/

    public void Attacked(int damage)
    {
        hp = hp - damage;
        GameManager.instance.MP -= 10;
        Debug.Log(gameObject.name + "가 공격받음.");
    }

    private void Flip()
    {
        if (lastPosX < transform.position.x) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (lastPosX > transform.position.x) transform.rotation = Quaternion.Euler(0, 180, 0);

        lastPosX = transform.position.x;
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