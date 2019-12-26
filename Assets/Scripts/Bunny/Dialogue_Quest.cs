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
    [SerializeField] private Quest myQuest;

    public Quest MyQuest { get { return myQuest; } }

    protected override void Start()
    {
        base.Start();

        QuestManager.instance.AddQuest(myQuest);
    }

    public override List<List<string>> DialogForNow()
    {
        nowTalkCnt += 1;
        int dialogNum = (int)myQuest.State();

        if(myQuest.SuccessDialogues.ScoreLimit.Count != 0)
        {
            if (dialogNum == 2)
            {
                return SuccessDialogue();
            }
            if (dialogNum == 3)
            {
                return AfterDialogue();
            }
        }

        return dialogues[dialogNum];
    }

    private List<List<string>> SuccessDialogue()
    {
        ScoreDialogue successDialogues = myQuest.SuccessDialogues;
        int score = myQuest.Score;

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
        ScoreDialogue afterDialogues = myQuest.AfterDialogues;
        int score = myQuest.Score;

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
