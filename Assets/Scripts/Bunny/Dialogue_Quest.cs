using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreDialogue
{
    [SerializeField] private List<int> scoreLimit;
    [SerializeField] private TextAsset dialogue;

    public List<int> ScoreLimit { get { return scoreLimit; } }
    public TextAsset Dialogue { get { return dialogue; } }
}

public class Dialogue_Quest : Dialogue
{
    [SerializeField] private QuestName questName;
    [Tooltip("0 : isAccept, 1 : isSuccess, 2 : isInstant")]
    [SerializeField] private List<bool> questState;
    [SerializeField] private ScoreDialogue successDialogues;
    [SerializeField] private ScoreDialogue afterDialogues;

    private Quest myQuest;
    private int score = 0;

    public Quest MyQuest { get { return myQuest; } }
    public int Score { set { score = value; } }

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

        if (dialogNum == 2)
        {
            return SuccessDialogue();
        }
        if (dialogNum == 3)
        {
            return AfterDialogue();
        }

        return dialogues[dialogNum];
    }

    private List<List<string>> SuccessDialogue()
    {
        List<int> scoreLimit = successDialogues.ScoreLimit;
        for (int i = 0; i < scoreLimit.Count; i++)
        {
            if (score >= scoreLimit[i])
                return Parser.DialogParse(successDialogues.Dialogue)[i];
        }

        Debug.LogError("점수에 해당하는 대화를 찾지 못했습니다.");
        return null;
    }

    private List<List<string>> AfterDialogue()
    {
        List<int> scoreLimit = afterDialogues.ScoreLimit;
        for (int i = 0; i < scoreLimit.Count; i++)
        {
            if (score >= scoreLimit[i])
                return Parser.DialogParse(afterDialogues.Dialogue)[i];
        }

        Debug.LogError("점수에 해당하는 대화를 찾지 못했습니다.");
        return null;
    }
}
