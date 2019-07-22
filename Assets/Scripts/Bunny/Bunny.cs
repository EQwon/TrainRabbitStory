using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public bool isInteractive; //상호작용 할 수 있는지
    public int hp;

    private void Update()
    {
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
}