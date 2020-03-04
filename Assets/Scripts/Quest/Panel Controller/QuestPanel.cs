using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestPanel : MonoBehaviour
{
    public abstract void StartQuest(Quest quest);
    protected abstract void DuringQuest();

    private void FixedUpdate()
    {
        DuringQuest();
    }
}
