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

        QuestManager.instance.UpdateQuest();
        Destroy(gameObject);
    }
}
