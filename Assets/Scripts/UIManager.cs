using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject talkPanel;
    public Image speakerImage;
    public Text speakerName;
    public Text speakerText;
    public GameObject finishDialogText;
    public GameObject acceptQuestButton;
    public GameObject rejectQuestButton;
    public GameObject basicUI;
    public Text cellNumberText;
    public List<Sprite> storyOpeningImage;
    public List<Sprite> storyEndingImage;
    public GameObject storyPanel;
    public GameObject healthBar;
    public GameObject heart;

    private GameObject mainCamera;

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

        TrainCellNumberUpdate();

        currentDialogNum = -1;

        mainCamera = GameObject.Find("Main Camera");

        mainCamera.transform.position = new Vector3(0, 0, -10f);
        mainCamera.GetComponent<Camera>().orthographicSize = 5.5f;
    }

    public void StartTalk(Vector2 playerPos, GameObject interactBunny)
    {
        basicUI.SetActive(false);
        currentInteractBunny = interactBunny;
        Vector2 interactBunnyPos = interactBunny.transform.position;

        currentDialogue = interactBunny.GetComponent<Dialogue>().dialogueForNow();

        talkPanel.SetActive(true);
        mainCamera.transform.position = ZoomInCameraPos(playerPos, interactBunnyPos);
        mainCamera.GetComponent<Camera>().orthographicSize = 4f;
        currentDialogNum = -1;

        NextDialog();
    }

    private Vector3 ZoomInCameraPos(Vector2 playerPos, Vector2 interactBunnyPos)
    {
        float cameraPos_X, cameraPos_Y;

        cameraPos_X = (playerPos.x + interactBunnyPos.x) / 2;
        cameraPos_Y = (playerPos.y + interactBunnyPos.y - 2f) / 2;

        if (cameraPos_X > 2.4f)
            cameraPos_X = 2.4f;
        else if (cameraPos_X < -2.4f)
            cameraPos_X = -2.4f;

        return new Vector3(cameraPos_X, cameraPos_Y, -10f);
    }

    public void NextDialog()
    {
        currentDialogNum += 1;

        if (currentDialogNum >= currentDialogue.Count)
        {
            currentInteractBunny.GetComponent<Dialogue>().GiveReward(currentDialogue);
            InitUI();
            GameManager.instance.ChangeMoveState(true);
            currentInteractBunny.transform.GetChild(1).GetComponent<TalkBubble>().ChangeBubbleState();
            return;
        }

        speakerName.text = currentDialogue[currentDialogNum].Speaker;
        speakerText.text = currentDialogue[currentDialogNum].Text;
        SoundManager.instance.talkSE();

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

    public void AcceptQuest()
    {
        currentDialogue.AddRange(currentInteractBunny.GetComponent<Dialogue>().AfterAcceptDialog());
        Debug.Log("퀘스트를 수락합니다.");
        int questNum = (int)currentInteractBunny.GetComponent<Dialogue>().quest;
        GameManager.instance.gameObject.GetComponent<QuestManager>().isAccept[questNum] = true;
        NextDialog();
    }

    public void RejectQuest()
    {
        currentDialogue.AddRange(currentInteractBunny.GetComponent<Dialogue>().AfterRefuseDialog());
        Debug.Log("퀘스트를 거절합니다.");
        NextDialog();
    }

    public void TrainCellNumberUpdate()
    {
        cellNumberText.text = GameManager.instance.gameObject.GetComponent<TrainController>().CellNum.ToString();
    }

    public void ShowOpeningStory(GameManager.Level level)
    {
        storyPanel.GetComponent<Image>().sprite = storyOpeningImage[(int)level];
        storyPanel.SetActive(true);
    }

    public void ShowEndingStory(GameManager.Level level)
    {
        storyPanel.GetComponent<Image>().sprite = storyEndingImage[(int)level];
        storyPanel.SetActive(true);
    }

    public void AdjustStatusBar()
    {
        float hp = GameManager.instance.HP / 100;
        float mp = GameManager.instance.MP / 100;
        healthBar.GetComponent<Slider>().value = hp;
        heart.GetComponent<Image>().color = new Color(mp, mp, mp);
    }

    public void CloseStory()
    {
        storyPanel.SetActive(false);
        GameManager.instance.ChangeMoveState(true);
    }
}