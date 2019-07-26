using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public int hp;
    public bool isInteractable;

    private void Start()
    {
        if (isInteractable == true) hp = 1000000;
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        GameManager.instance.MP -= 10;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}