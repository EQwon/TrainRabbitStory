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
[RequireComponent(typeof(Quest))]
public class QuestDialogue : Dialogue
{
    [SerializeField] private ScoreDialogue successDialogues;
    [SerializeField] private ScoreDialogue afterDialogues;

    private Quest myQuest;

    public Quest MyQuest { get { return myQuest; } }

    protected override void Start()
    {
        myQuest = GetComponent<Quest>();

        base.Start();

        QuestManager.instance.AddQuest(myQuest);
    }

    public override List<List<string>> DialogueForNow()
    {
        nowTalkCnt += 1;

        switch (myQuest.GetState())
        {
            case QuestState.BeforeQuest:
                return dialogues[0];
            case QuestState.DoingQuest:
                return dialogues[1];
            case QuestState.FinishQuest:
                return FinishDialogue();
            case QuestState.AfterQuest:
                return AfterDialogue();
            default:
                return null;
        }
    }

    private List<List<string>> FinishDialogue()
    {
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
