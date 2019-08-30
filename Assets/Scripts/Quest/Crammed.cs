using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Question
{
    public string question;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;
}

public class Crammed : MonoBehaviour
{
    private enum State { Ready, Note, Test, Grading }
    private State state;

    [Header("Resources")]
    public List<Sprite> countDownImage;
    public int limitTime;
    public List<Sprite> noteImage;
    public List<Question> questions;

    [Header("Holder")]
    public GameObject countDown;
    public GameObject timer;
    public GameObject note;
    public GameObject test;
    public List<Text> testText;
    public GameObject checkMark;
    public GameObject gradeCard;
    public List<Text> gradeText;
    
    private float nowTime;
    private Text timerText;
    private int noteNum = 0;
    private int testNum = 0;

    private void Start()
    {
        GameManager.instance.IsTalking = true;
        state = State.Ready;
        timerText = timer.GetComponentInChildren<Text>();
        timerText.text = "6.00 s";
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
        if (testNum == questions.Count)
        {
            state = State.Grading;

            test.SetActive(false);
            gradeCard.SetActive(true);

            return;
        }

        string[] temp;
        temp = questions[testNum].question.Split('!');
        if (temp.Length <= 1) testText[0].text = (testNum + 1) + ". " + questions[testNum].question;
        else testText[0].text = (testNum + 1) + ". " + temp[0] + "\n <size=45>" + temp[1] + "</size>";

        testText[1].text = "가. " + questions[testNum].answer1.Replace("<>", "\n");
        testText[2].text = "나. " + questions[testNum].answer2.Replace("<>", "\n");
        testText[3].text = "다. " + questions[testNum].answer3.Replace("<>", "\n");
        testText[4].text = "라. " + questions[testNum].answer4.Replace("<>", "\n");
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

        checkMark.SetActive(true);
        checkMark.GetComponent<RectTransform>().anchoredPosition = testText[answerNum].GetComponent<RectTransform>().anchoredPosition - new Vector2(250, 0);

        yield return new WaitForSeconds(1f);

        for (int i = 1; i <= 4; i++)
        {
            testText[i].GetComponent<Button>().interactable = true;
        }

        checkMark.SetActive(false);
        testNum += 1;
    }

    private void ShowGrade()
    {
        gradeText[0].text = "테슽";
        gradeText[1].text = "S+";

        StartCoroutine(EndTest());
    }

    private IEnumerator EndTest()
    {
        yield return new WaitForSeconds(1.5f);

        QuestManager.instance.isSuccess[(int)Quest.Crammed] = true;
        QuestManager.instance.BackToNoraml();
        Player.instance.gameObject.GetComponentInChildren<Talk>().Talking();
    }
}
