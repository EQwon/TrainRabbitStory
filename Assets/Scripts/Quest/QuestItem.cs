using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public enum Name { handkerchief };

    public Name itemName;

    public void GetQuestItem()
    {
        switch (itemName)
        {
            case Name.handkerchief:
                QuestManager.instance.GetQuest(QuestName.PickUp).ChangeQuestState(true, true);
                break;
        }

        Destroy(gameObject);
    }
}
