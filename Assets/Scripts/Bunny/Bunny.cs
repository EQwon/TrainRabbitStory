using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public int hp;
    public bool isInteractable;
    public bool isInvincible = false;

    private int startHp;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (isInteractable == true) hp = 1000000;

        startHp = hp;
    }

    private void Update()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (float)hp / startHp);
    }

    public void TakeDamage(int hpAmount, int mpAmount)
    {
        if (isInvincible == true) return;

        GetComponent<RandomMoving>().StopByAttacked();

        hp -= hpAmount;
        Player.instance.MP -= mpAmount;

        if (hp <= 20)
        {
            isInvincible = true;
        }
    }
}