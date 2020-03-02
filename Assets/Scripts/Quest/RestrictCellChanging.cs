﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RestrictCondition
{
    public QuestName questName;
    public bool isAccept;
    public bool isSuccess;
    public string warningText;
}

public class RestrictCellChanging : MonoBehaviour
{
    public List<RestrictCondition> cond;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag != "Player") return;

        for(int i = 0; i < cond.Count; i++)
        {
            if (QuestManager.instance.GetQuest(cond[i].questName).IsAccpet == cond[i].isAccept
                && QuestManager.instance.GetQuest(cond[i].questName).IsFinish == cond[i].isSuccess)
            {
                Player.instance.gameObject.transform.position -= new Vector3(0.5f, 0, 0);
                UIManager.instance.Warning(cond[i].warningText);

                return;
            }            
        }
    }
}
