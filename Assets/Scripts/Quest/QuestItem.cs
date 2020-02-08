using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private PickUpQuest questScript;
    public PickUpQuest QuestScript { set { questScript = value; } }

    public void GetQuestItem()
    {
        questScript.GetItem();

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GetQuestItem();
        }
    }
}
