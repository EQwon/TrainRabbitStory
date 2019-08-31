using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Talk UI")]
    public GameObject talkPanel;
    public Image speakerImage;
    public Text speakerName;
    public Text speakerText;
    public GameObject finishDialogText;
    public GameObject acceptQuestButton;
    public GameObject rejectQuestButton;

    [Header("Basic UI")]
    public GameObject basicUI;
    public Text cellNumberText;
    public GameObject healthBar;
    public GameObject heart;
    public GameObject WarningPanel;
    public GameObject talkButton;

    [Header("For Tutorial")]
    public List<Dialog> tutoDialog;
    public GameObject darkPanel;

    private List<Dialog> currentDialogue;
    private GameObject currentInteractBunny;
    private int currentDialogNum = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitUI();
    }

    private void InitUI()
    {
        talkPanel.SetActive(false);
        finishDialogText.SetActive(false);
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);
        basicUI.SetActive(true);
        WarningPanel.SetActive(false);
        darkPanel.SetActive(false);

        TrainCellNumberUpdate(10);

        currentDialogNum = -1;

        GameObject mainCamera = Camera.main.gameObject;
        if (mainCamera.GetComponent<CameraWalk>().enabled == false)
            mainCamera.GetComponent<CameraWalk>().enabled = true;

        mainCamera.GetComponent<CameraWalk>().ZoomOutCamera();
    }

    public void StartTalk(Vector2 playerPos, GameObject interactBunny)
    {
        currentDialogue = interactBunny.GetComponent<Dialogue>().DialogueForNow();
        if (currentDialogue.Count == 0) return;

        GameManager.instance.IsTalking = true;
        basicUI.SetActive(false);
        currentInteractBunny = interactBunny;
        Vector2 interactBunnyPos = interactBunny.transform.position;

        talkPanel.SetActive(true);
        talkPanel.GetComponent<Button>().interactable = true;
        Camera.main.gameObject.GetComponent<CameraWalk>().ZoomInCamera(playerPos, interactBunny.transform.position);
        currentDialogNum = -1;

        NextDialog();
    }

    public void NextDialog()
    {
        currentDialogNum += 1;

        if (currentDialogNum >= currentDialogue.Count)
        {
            EndTalk();
            QuestManager.instance.StartUnperfomedQuest();
            QuestManager.instance.CheckStageClear();
            return;
        }

        speakerName.text = currentDialogue[currentDialogNum].Speaker;
        speakerImage.sprite = currentDialogue[currentDialogNum].SpeakerImage;
        if (currentDialogue[currentDialogNum].SpeakerImage != null) speakerImage.color = Color.white;
        else speakerImage.color = Color.clear;
        speakerText.text = currentDialogue[currentDialogNum].Text;
        SoundManager.instance.TalkSE();

        if(currentDialogNum == currentDialogue.Count - 1)
        {
            //Debug.Log("왜 안나옴?");
            DialogueFinishNotice(currentDialogue[currentDialogNum].type);
        }
    }

    private void DialogueFinishNotice(DialogType type)
    {
        if (type == DialogType.BeforeQuest)
        {
            talkPanel.GetComponent<Button>().interactable = false;
            acceptQuestButton.SetActive(true);
            rejectQuestButton.SetActive(true);
            finishDialogText.SetActive(false);
        }
        else
        {
            acceptQuestButton.SetActive(false);
            rejectQuestButton.SetActive(false);
            finishDialogText.SetActive(true);
        }
    }

    private void EndTalk()
    {
        if(currentInteractBunny != null) currentInteractBunny.GetComponent<Dialogue>().GiveReward(currentDialogue);
        InitUI();
        GameManager.instance.IsTalking = false;
    }

    public void AcceptQuest()
    {
        talkPanel.GetComponent<Button>().interactable = true;
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);
        currentDialogue.AddRange(currentInteractBunny.GetComponent<Dialogue>().AfterAcceptDialog());
        int questNum = (int)currentInteractBunny.GetComponent<Dialogue>().quest;
        GameManager.instance.gameObject.GetComponent<QuestManager>().isAccept[questNum] = true;
        Debug.Log(questNum + "번째 퀘스트를 수락합니다.");
        NextDialog();
    }

    public void RejectQuest()
    {
        talkPanel.GetComponent<Button>().interactable = true;
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);
        currentDialogue.AddRange(currentInteractBunny.GetComponent<Dialogue>().AfterRefuseDialog());
        Debug.Log("퀘스트를 거절합니다.");
        NextDialog();
    }

    public void TrainCellNumberUpdate(int cellNum)
    {
        //주어진 번호로 차량 번호 칸을 업데이트 한다.
        //cellNumberText.text = cellNum.ToString();
    }

    public void ShowOpeningStory()
    {
        darkPanel.SetActive(true);
        currentDialogue = tutoDialog;

        GameManager.instance.IsTalking = true;
        basicUI.SetActive(false);

        talkPanel.SetActive(true);
        talkPanel.GetComponent<Button>().interactable = true;
        currentDialogNum = -1;

        NextDialog();
    }

    public void AdjustStatusBar()
    {
        float hp = (float)GameManager.instance.HP / 1000;
        float mp = (float)GameManager.instance.MP / 100;
        healthBar.GetComponent<Slider>().value = hp;
        heart.GetComponent<Image>().color = new Color(mp, mp, mp);
    }

    public void Warning(string text)
    {
        Text warningText = WarningPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        warningText.text = text;

        WarningPanel.SetActive(true);
    }

    public void SetTalkButtonState(bool isActive)
    {
        if (isActive == true)
        {
            talkButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            talkButton.GetComponent<Button>().interactable = false;
        }
    }

    public void BackToStartScene()
    {
        SceneManager.LoadScene(0);
    }
}