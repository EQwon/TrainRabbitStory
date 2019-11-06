using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public enum Name { handkerchief };

    public Name itemName;

    public void GetItem()
    {
        switch (itemName)
        {
            case Name.handkerchief:
                QuestManager.instance.ChangeQuestState(Quest.PickUp, true, true);
                break;
        }

        Destroy(gameObject);
    }
}
