using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public delegate void WhenPlayerGetItem();

    public WhenPlayerGetItem GetItem;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GetItem();
            gameObject.SetActive(false);
        }
    }
}
