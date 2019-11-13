using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Problem
{
    private string question;
    private List<string> examples;
    private int answer;

    public string Question { get { return question; } }
    public List<string> Examples { get { return examples; } }
    public int Answer { get { return answer; } }

    public Problem(string question, List<string> examples, int answer)
    {
        this.question = question;
        this.examples = examples;
        this.answer = answer;
    }
}

public class Crammed : MonoBehaviour
{
    private enum State { Ready, Note, Test, Grading }
    private State state;

    [Header("Resources")]
    [SerializeField] private List<Sprite> countDownImage;
    [SerializeField] private int limitTime;
    [SerializeField] private List<Sprite> noteImage;
    [SerializeField] private TextAsset problemNote;
    [SerializeField] private List<Problem> problems;

    [Header("Holder")]
    [SerializeField] private GameObject countDown;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject test;
    [SerializeField] private List<Text> testText;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private GameObject gradeCard;
    [SerializeField] private List<Text> gradeText;
    
    private float nowTime;
    private Text timerText;
    private int noteNum = 0;
    private int problemNum = 0;
    private int answerCnt = 0;

    private void Start()
    {
        GameManager.instance.IsTalking = true;
        state = State.Ready;
        timerText = timer.GetComponentInChildren<Text>();
        timerText.text = "6.00 s";
        problems = Parser.CrammedParse(problemNote);
        nowTime = 0;

        countDown.SetActive(true);
        timer.SetActive(true);
        note.SetActive(true);
        test.SetActive(false);
        checkMark.SetActive(false);
        gradeCard.SetActive(false);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Ready:
                nowTime += Time.deltaTime;
                CountDown();
                break;
            case State.Note:
                nowTime += Time.deltaTime;
                NoteCheck();
                break;
            case State.Test:
                ShowTest();
                break;
            case State.Grading:
                ShowGrade();
                break;
        }
    }

    private void CountDown()
    {
        if (nowTime >= 3f)
        {
            state = State.Note;
            countDown.SetActive(false);
            nowTime = 0f;
        }

        countDown.GetComponent<Image>().sprite = countDownImage[Mathf.FloorToInt(nowTime)];
    }

    private void NoteCheck()
    {
        if (nowTime >= limitTime)
        {
            nowTime = 0;
            //노트 넘기기
            noteNum += 1;
            if (noteNum >= noteImage.Count)
            {
                state = State.Test;
                note.SetActive(false);
                timer.SetActive(false);

                test.SetActive(true);
                return;
            }

            note.GetComponent<Image>().sprite = noteImage[noteNum];
        }

        timerText.text = (limitTime - nowTime).ToString("F2") + " s";
    }

    private void ShowTest()
    {
        if (problemNum == problems.Count)
        {
            state = State.Grading;

            test.SetActive(false);
            gradeCard.SetActive(true);

            return;
        }

        Problem problem = problems[problemNum];

        testText[0].text = problem.Question;
        testText[1].text = problem.Examples[0];
        testText[2].text = problem.Examples[1];
        testText[3].text = problem.Examples[2];
        testText[4].text = problem.Examples[3];
    }

    public void NextTest(int answerNum)
    {
        StartCoroutine(CheckAnswer(answerNum));
    }

    private IEnumerator CheckAnswer(int answerNum)
    {
        for (int i = 1; i <= 4; i++)
        {
            testText[i].GetComponent<Button>().interactable = false;
        }

        if (answerNum == problems[problemNum].Answer)
        {
            answerCnt += 1;
            checkMark.SetActive(true);
            checkMark.GetComponent<RectTransform>().anchoredPosition = testText[answerNum].GetComponent<RectTransform>().anchoredPosition - new Vector2(250, 0);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 1; i <= 4; i++)
        {
            testText[i].GetComponent<Button>().interactable = true;
        }

        checkMark.SetActive(false);
        problemNum += 1;
    }

    private void ShowGrade()
    {
        string grade = "";

        if (answerCnt == 0) grade = "F";
        else if (1 <= answerCnt && answerCnt <= 3) grade = "C+";
        else if (4 <= answerCnt && answerCnt <= 6) grade = "B+";
        else if (7 <= answerCnt && answerCnt <= 9) grade = "A";
        else if (answerCnt == 10) grade = "A+";


        gradeText[0].text = (answerCnt * 10).ToString() + " 점";
        gradeText[1].text = grade;

        StartCoroutine(EndTest());
    }

    private IEnumerator EndTest()
    {
        yield return new WaitForSeconds(2.5f);

        QuestManager.instance.GetQuest(QuestName.Crammed).ChangeQuestState(true, true);
        QuestManager.instance.BackToNoraml();
        UIManager.instance.StartTalk();
    }
}
