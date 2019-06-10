using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private GameObject player;
    private List<GameObject> attackedBunnies;
    private int damage;

    private void Awake()
    {
        player = gameObject.transform.parent.gameObject;
        attackedBunnies = new List<GameObject>();
        damage = player.GetComponent<Player>().damage;
    }

    public void ApplyDamage()
    {
        for (int i = 0; i < attackedBunnies.Count; i++)
        {
            attackedBunnies[i].GetComponent<Bunny>().Attacked(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Bunny")
        {
            attackedBunnies.Add(coll.gameObject);
            coll.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Bunny")
        {
            if (attackedBunnies.Contains(coll.gameObject))
            {
                attackedBunnies.Remove(coll.gameObject);
                coll.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            }
        }
    }
}
