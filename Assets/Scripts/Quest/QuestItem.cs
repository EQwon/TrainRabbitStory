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
                QuestManager.instance.isSuccess[(int)Quest.PickUp] = true;
                break;
        }

        Destroy(gameObject);
    }
}
