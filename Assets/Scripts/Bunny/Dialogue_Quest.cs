using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Quest : Dialogue
{
    [SerializeField] private QuestName questName;

    private Quest myQuest;

    public Quest MyQuest { get { return myQuest; } }

    private void Start()
    {
        myQuest = new Quest(questName, true, false, false);
    }

    public override List<List<string>> DialogForNow()
    {
        nowTalkCnt += 1;
        int dialogNum = (int)QuestManager.instance.GetQuest(questName).State();

        return dialogues[dialogNum];
    }
}
