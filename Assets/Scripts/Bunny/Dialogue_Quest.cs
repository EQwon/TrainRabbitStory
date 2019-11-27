using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Quest : Dialogue
{
    [SerializeField] private QuestName questName;
    [Tooltip("0 : isAccept, 1 : isSuccess, 2 : isInstant")]
    [SerializeField] private List<bool> questState;

    private Quest myQuest;

    public Quest MyQuest { get { return myQuest; } }

    protected override void Start()
    {
        base.Start();

        if (questState.Count == 3)
            myQuest = new Quest(questName, questState[0], questState[1], questState[2]);
        else myQuest = new Quest(questName);
    }

    public override List<List<string>> DialogForNow()
    {
        nowTalkCnt += 1;
        int dialogNum = (int)QuestManager.instance.GetQuest(questName).State();

        return dialogues[dialogNum];
    }
}
