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
    }

    public void Attacked(int damage)
    {
        hp = hp - damage;
        GameManager.instance.MP -= 10;
        Debug.Log(gameObject.name + "가 공격받음.");
    }

    public void FlipBunny(GameObject player)
    {
        float myPosX = transform.position.x;
        float playerPosX = player.transform.position.x;

        if(myPosX > playerPosX) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}