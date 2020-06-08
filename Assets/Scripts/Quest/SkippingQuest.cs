using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotTalkDialogue
{
    private string question;
    private List<string> answers;
    private List<int> points;
    private List<string> reactions;

    public string Question { get { return question; } }
    public string Answer(int i) { return answers[i]; }
    public int Point(int i) { return points[i]; }
    public string Reaction(int i) { return reactions[i]; }

    public CarrotTalkDialogue(string question, List<string> answers, List<int> points, List<string> reactions)
    {
        this.question = question;
        this.answers = answers;
        this.points = points;
        this.reactions = reactions;
    }
}

public class SkippingQuest : QuestPanel
{
    [Header("Object Holder")]
    [SerializeField] private GameObject phoneScreen;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private List<Text> answerTexts;

    [Header("Resource Holder")]
    [SerializeField] private GameObject teacherTextPrefab;
    [SerializeField] private GameObject myTextPrefab;
    [SerializeField] private TextAsset skippingTextAsset;

    private QuestName questName;
    private List<CarrotTalkDialogue> dialogues;
    private int nowDialogue;
    private List<GameObject> SpeakLists;
    private int points;

    public override void StartQuest(Quest quest)
    {
        questName = quest.QuestName;

        dialogues = Parser.SkippingParse(skippingTextAsset);
        nowDialogue = 0;
        points = 0;
        SpeakLists = new List<GameObject>();

        CleanAnswerText();
        TeacherSpeak();
    }

    protected override void DuringQuest()
    {
        for (int i = 0; i < SpeakLists.Count; i++)
        {
            GameObject target = SpeakLists[SpeakLists.Count - 1 - i];
            Vector2 spawnPos;

            if (i == 0)
            {
                spawnPos = Vector2.zero;
            }
            else
            {
                spawnPos = SpeakLists[SpeakLists.Count - i].GetComponent<RectTransform>().anchoredPosition;
                spawnPos.y += SpeakLists[SpeakLists.Count - i].GetComponent<RectTransform>().sizeDelta.y + 10f;
            }

            target.GetComponent<RectTransform>().anchoredPosition = spawnPos;
        }
    }

    private void TeacherSpeak()
    {
        GameObject teacherText = Instantiate(teacherTextPrefab, phoneScreen.transform);
        SpeakLists.Add(teacherText);

        string question = dialogues[nowDialogue].Question;
        teacherText.GetComponent<CarrotTalkSizeFitter>().AssignText(question);

        ShowMyAnswers();
    }

    private void ShowMyAnswers()
    {
        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].text = dialogues[nowDialogue].Answer(i);
            answerButtons[i].interactable = true;
        }
    }

    private void MySpeak(int num)
    {
        GameObject myText = Instantiate(myTextPrefab, phoneScreen.transform);
        SpeakLists.Add(myText);

        string answer = dialogues[nowDialogue].Answer(num);
        myText.GetComponent<CarrotTalkSizeFitter>().AssignText(answer);
    }

    private void TeacherReaction(int num)
    {
        GameObject teacherText = Instantiate(teacherTextPrefab, phoneScreen.transform);
        SpeakLists.Add(teacherText);

        string reaction = dialogues[nowDialogue].Reaction(num);
        teacherText.GetComponent<CarrotTalkSizeFitter>().AssignText(reaction);
    }

    public void Answer(int i)
    {
        StartCoroutine(AnswerRoutine(i));
        points += dialogues[nowDialogue].Point(i);
    }

    private IEnumerator AnswerRoutine(int i)
    {
        MySpeak(i);
        CleanAnswerText();

        yield return new WaitForSeconds(2f);

        TeacherReaction(i);

        yield return new WaitForSeconds(2f);

        nowDialogue += 1;
        if (dialogues.Count == nowDialogue)
        {
            yield return new WaitForSeconds(2f);

            UIManager.instance.CurrentInteractBunny.GetComponent<QuestDialogue>().MyQuest.Score = points;
            QuestManager.instance.QuestFinish(questName);
            QuestManager.instance.BackToNoraml();
            UIManager.instance.StartTalk();
            gameObject.SetActive(false);

            yield break;
        }

        TeacherSpeak();
    }

    private void CleanAnswerText()
    {
        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].text = "";
            answerButtons[i].interactable = false;
        }
    }
}
