using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public int hp;
    public bool isInteractable;
    public bool isInvincible = false;

    private int startHp;

    private void Start()
    {
        if (isInteractable == true) hp = 1000000;

        startHp = hp;
    }

    public void TakeDamage(int hpAmount, int mpAmount)
    {
        if (isInvincible == true) return;

        GetComponent<RandomMoving>().StopByAttacked();
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float)hp / startHp);

        hp -= hpAmount;
        GameManager.instance.MP -= mpAmount;

        if (hp <= 20)
        {
            isInvincible = true;
        }
    }
}