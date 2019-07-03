using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCall : MonoBehaviour
{
    //3, 2, 1 시작!
    //제한시간 15초
    //버튼을 눌러서 알맞은 번호를 누르자.
    //  //화면에 임의의 번호가 출력
    //  //숫자판 출력 - 배열이 임의의 형태로 되어있음
    //  //제대로 된 숫자를 누르면 알파값 = 1

    public GameObject[] numberToPush;
    public GameObject[] numberButton;
    public GameObject countDownNumber;
    public GameObject result;
    public Sprite[] numberImage;
    public Sprite[] countDownNumberImage;
    public Sprite[] resultImage;
    public Text remainTimeText;
    public float timeLimit = 15f;

    private enum State { ready, playing, success, fail };
    private State state;

    private int index = 0;
    private int nowNum;
    private float countDownTime = 4f;
    private float remainTime = 0;

    private void Start()
    {
        UIManager.instance.gameObject.SetActive(false);
        InitializePanel();
        state = State.ready;
        remainTime = timeLimit;
    }

    private void Update()
    {
        switch (state)
        {
            case State.ready :
                countDownTime -= Time.deltaTime;
                CountDown();

                if (countDownTime <= 0f) StartGame();
                break;
            case State.playing :
                remainTime -= Time.deltaTime;
                remainTimeText.text = remainTime.ToString("F2") + " s";
                if (remainTime <= 0)
                {
                    state = State.fail;
                    remainTimeText.text = "0.00 s";
                }
                break;
            case State.success:
                result.SetActive(true);
                result.GetComponent<Image>().sprite = resultImage[0];
                StartCoroutine(SuccessReaction());
                break;
            case State.fail:
                result.SetActive(true);
                result.GetComponent<Image>().sprite = resultImage[1];
                StartCoroutine(FailReaction());
                break;
            default:
                break;
        }
    }

    private void InitializePanel()
    {
        remainTimeText.text = "";
        result.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            numberToPush[i].GetComponent<Image>().color = new Color(1, 1, 1, 0f);
            numberButton[i].SetActive(false);
        }
    }

    private void CountDown()
    {
        if (countDownTime <= 0f) countDownNumber.SetActive(false);
        else if (countDownTime <= 1f) countDownNumber.GetComponent<Image>().sprite = countDownNumberImage[0];
        else if (countDownTime <= 2f) countDownNumber.GetComponent<Image>().sprite = countDownNumberImage[1];
        else if (countDownTime <= 3f) countDownNumber.GetComponent<Image>().sprite = countDownNumberImage[2];
    }

    private void StartGame()
    {
        for (int i = 0; i < 10; i++) numberButton[i].SetActive(true);

        state = State.playing;

        int num = Random.Range(0, 10);
        ShowNewNumberToPush();
        ShuffleButton();
    }

    private void ShuffleButton()
    {
        for (int i = 0; i < 10; i++)
        {
            int targetNum = Random.Range(0, 10);
            Vector3 temp = numberButton[i].transform.position;
            numberButton[i].transform.position = numberButton[targetNum].transform.position;
            numberButton[targetNum].transform.position = temp;
        }
    }

    private void ShowNewNumberToPush()
    {
        nowNum = Random.Range(0, 10);
        numberToPush[index].GetComponent<Image>().sprite = numberImage[nowNum];
        numberToPush[index].GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
    }

    public void PushNumber(int num)
    {
        if (nowNum == num) CorrectPushReaction();
        else WrongPushReaction();
    }

    private void CorrectPushReaction()
    {
        numberToPush[index].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        index++;
        if (index == 10)
        {
            state = State.success;
            return;
        }
        ShowNewNumberToPush();
        ShuffleButton();
    }

    private void WrongPushReaction()
    { }

    private IEnumerator SuccessReaction()
    {
        QuestManager questManager = QuestManager.instance;

        yield return new WaitForSeconds(1f);

        questManager.isSuccess[(int)Quest.PhoneCall] = true;
        questManager.BackToNoraml();
        Player.instance.gameObject.GetComponentInChildren<Talk>().Talking();
    }

    private IEnumerator FailReaction()
    {
        QuestManager questManager = QuestManager.instance;

        yield return new WaitForSeconds(1f);

        questManager.BackToNoraml();
        Player.instance.gameObject.GetComponentInChildren<Talk>().Talking();
    }
}
