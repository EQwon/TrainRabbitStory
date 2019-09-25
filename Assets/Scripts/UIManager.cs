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

    [Header("Inven Image Holder")]
    public List<Sprite> invenImage;
    public Image inven;

    [Header("For Opening")]
    public TextAsset openingAsset;
    public GameObject darkPanel;

    [Header("Stage Clear")]
    public GameObject stageClearPanel;
    public List<Sprite> clearMent;

    private List<List<string>> currentDialogue;
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
        stageClearPanel.SetActive(false);

        currentDialogNum = -1;

        GameObject mainCamera = Camera.main.gameObject;
        if (mainCamera.GetComponent<CameraWalk>().enabled == false)
            mainCamera.GetComponent<CameraWalk>().enabled = true;

        mainCamera.GetComponent<CameraWalk>().ZoomOutCamera();

        inven.sprite = invenImage[GameManager.instance.Stage];
    }

    public void StartTalk(Vector2 playerPos, GameObject interactBunny)
    {
        currentDialogue = interactBunny.GetComponent<Dialogue>().DialogForNow();
        if (currentDialogue.Count == 0) return;

        //게임 전체 상태 변환
        GameManager.instance.IsTalking = true;

        //UI 조정
        basicUI.SetActive(false);
        talkPanel.SetActive(true);
        talkPanel.GetComponent<Button>().interactable = true;

        //카메라 조정
        currentInteractBunny = interactBunny;        
        Camera.main.gameObject.GetComponent<CameraWalk>().ZoomInCamera(playerPos, currentInteractBunny);

        //대화 시작
        currentDialogNum = -1;
        NextDialog();
    }

    public void NextDialog()
    {
        currentDialogNum += 1;

        if (currentDialogNum >= currentDialogue.Count)
        {
            EndTalk();
            return;
        }

        speakerName.text = currentDialogue[currentDialogNum][0];
        speakerText.text = currentDialogue[currentDialogNum][1];
        if (currentDialogue[currentDialogNum].Count > 2)
        {
            string path = "SpeakerImage/" + currentDialogue[currentDialogNum][2];
            speakerImage.sprite = Resources.Load<Sprite>(path);
        }
        
        SoundManager.instance.TalkSE();

        if(currentDialogNum == currentDialogue.Count - 1)
        {
            //Debug.Log("왜 안나옴?");
            DialogueFinishNotice();
        }
    }

    private void DialogueFinishNotice()
    {
        finishDialogText.SetActive(true);
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);

        if (currentDialogue[currentDialogNum].Count < 4) return;

        if (currentDialogue[currentDialogNum][3] == "AR")           // 수락, 거절 선택일 경우
        {
            talkPanel.GetComponent<Button>().interactable = false;
            acceptQuestButton.SetActive(true);
            rejectQuestButton.SetActive(true);
            finishDialogText.SetActive(false);
        }
        else if (currentDialogue[currentDialogNum][3] == "CH")      // 선택지일 경우
        {

        }
        else if (currentDialogue[currentDialogNum][3] == "Quest")   // 퀘스트의 완료일 경우
        {
            int questNum = currentDialogue[currentDialogNum][4][0] - 48;

            QuestManager.instance.ChangeQuestState((Quest)questNum, false, true);
            GameManager.instance.IsQuesting = false;
        }
        else if (currentDialogue[currentDialogNum][3] == "Clear")    // 클리어일 경우
        {
            EndTalk();
            StageClear();
        }
    }

    private void EndTalk()
    {
        //currentInteractBunny.GetComponent<Dialogue>().GiveReward(currentDialogue);
        if (currentInteractBunny != null && currentInteractBunny.GetComponent<Affinity>() != null)
        {
            GameManager.instance.TalkCnt[currentInteractBunny.GetComponent<Affinity>().bunnyNum] += 1;
        }

        InitUI();
        GameManager.instance.IsTalking = false;
        QuestManager.instance.StartUnperfomedQuest();
    }

    public void AcceptQuest()
    {
        //UI 조정
        talkPanel.GetComponent<Button>().interactable = true;
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);

        //퀘스트 수락 전달
        int questNum = (int)currentInteractBunny.GetComponent<Dialogue>().quest;
        GameManager.instance.gameObject.GetComponent<QuestManager>().isAccept[questNum] = true;
        Debug.Log(questNum + "번째 퀘스트를 수락합니다.");

        //수락시 대사 출력
        //currentDialogue.AddRange(currentInteractBunny.GetComponent<Dialogue>().DialogForNow());
        NextDialog();
    }

    public void RejectQuest()
    {
        //UI 조정
        talkPanel.GetComponent<Button>().interactable = true;
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);

        //퀘스트 거절 - 전달할 필요 없음.
        Debug.Log("퀘스트를 거절합니다.");

        //거절시 대화 종료
        NextDialog();
    }

    public void TrainCellNumberUpdate(int cellNum)
    {
        //주어진 번호로 차량 번호 칸을 업데이트 한다.
        cellNumberText.text = (GameManager.instance.MaxCellNum - cellNum).ToString("00");
    }

    public void ShowOpeningStory()
    {
        //UI 조정
        darkPanel.SetActive(true);
        basicUI.SetActive(false);
        talkPanel.SetActive(true);
        talkPanel.GetComponent<Button>().interactable = true;

        //대화 로딩
        currentDialogue = Parser.DialogParse(openingAsset)[GameManager.instance.Stage];

        //게임 상태 변화
        GameManager.instance.IsTalking = true;
        
        //대화 시작
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

    public void StageClear()
    {
        stageClearPanel.transform.GetChild(0).GetComponent<Image>().sprite = clearMent[GameManager.instance.Stage];
        stageClearPanel.SetActive(true);
    }

    public void TellGameMangerStageClear()
    {
        GameManager.instance.StageClear();
    }

    public void BackToStartScene()
    {
        SceneManager.LoadScene(0);
    }
}