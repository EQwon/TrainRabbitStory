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

public class Skipping : MonoBehaviour
{
    [Header("Object Holder")]
    [SerializeField] private GameObject phoneScreen;
    [SerializeField] private List<Text> answerTexts;

    [Header("Resource Holder")]
    [SerializeField] private GameObject teacherTextPrefab;
    [SerializeField] private GameObject myTextPrefab;
    [SerializeField] private TextAsset skippingTextAsset;

    private List<CarrotTalkDialogue> dialogues;
    private int nowDialogue;
    private List<GameObject> SpeakLists = new List<GameObject>();

    private void Start()
    {
        dialogues = Parser.SkippingParse(skippingTextAsset);
        nowDialogue = 0;

        CleanAnswerText();
        TeacherSpeak();
    }

    private void TeacherSpeak()
    {
        GameObject teacherText = Instantiate(teacherTextPrefab, phoneScreen.transform);

        string question = dialogues[nowDialogue].Question;
        teacherText.GetComponent<CarrotTalkSizeFitter>().AssignText(question);

        ShowMyAnswers();
    }

    private void ShowMyAnswers()
    {
        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].text = dialogues[nowDialogue].Answer(i);
        }
    }

    private void MySpeak(int num)
    {
        GameObject myText = Instantiate(myTextPrefab, phoneScreen.transform);

        string answer = dialogues[nowDialogue].Answer(num);
        myText.GetComponent<CarrotTalkSizeFitter>().AssignText(answer);
    }

    private void TeacherReaction(int num)
    {
        GameObject teacherText = Instantiate(teacherTextPrefab, phoneScreen.transform);

        string reaction = dialogues[nowDialogue].Reaction(num);
        teacherText.GetComponent<CarrotTalkSizeFitter>().AssignText(reaction);
    }

    public void Answer(int i)
    {
        MySpeak(i);
        TeacherReaction(i);
    }

    private void CleanAnswerText()
    {
        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].text = "";
        }
    }

    private void AddSpeak(GameObject speak)
    {
        SpeakLists.Add(speak);

        //거리 계산해서 배치해야함
    }
}
