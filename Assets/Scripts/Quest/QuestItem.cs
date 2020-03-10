using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public delegate void WhenPlayerGetItem(string itemName);

    public WhenPlayerGetItem GetItem;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GetItem(name);
            gameObject.SetActive(false);
        }
    }
}
