using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThonosQuestPanel : QuestPanel
{
    [Header("UI Element")]
    [SerializeField] private Text remainTimeText;

    private ThonosQuest quest;

    public override void StartQuest(Quest quest)
    {
        this.quest = quest.GetComponent<ThonosQuest>();
    }

    protected override void DuringQuest()
    {
        remainTimeText.text = quest.RemainTime.ToString("0.0 초");
    }
}
