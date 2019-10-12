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

    [Header("Inven")]
    public List<Sprite> invenImage;
    public Image inven;
    public GameObject InventoryPanel;

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
        InventoryPanel.SetActive(false);
        stageClearPanel.SetActive(false);

        currentDialogNum = -1;

        GameObject mainCamera = Camera.main.gameObject;
        if (mainCamera.GetComponent<CameraWalk>().enabled == false)
            mainCamera.GetComponent<CameraWalk>().enabled = true;

        mainCamera.GetComponent<CameraWalk>().ZoomOutCamera();

        inven.sprite = invenImage[GameManager.instance.Stage];
    }
    
    /// <summary>
    /// 방금 전 토끼와 다시 대화
    /// </summary>
    public void StartTalk()
    {
        GameObject interactBunny = currentInteractBunny;

        //1. 현재 할 대화를 가져오는 기능
        currentDialogue = interactBunny.GetComponent<Dialogue>().DialogForNow();
        if (currentDialogue.Count == 0) return;

        //2. 대화 상태로 UI 조정하는 기능
        AdjustUIAndStart(interactBunny);
    }
    
    /// <summary>
    /// 대화할 토끼 지정해서 대화
    /// </summary>
    /// <param name="interactBunny">대화할 토끼</param>
    public void StartTalk(GameObject interactBunny)
    {
        //1. 현재 할 대화를 가져오는 기능
        currentDialogue = interactBunny.GetComponent<Dialogue>().DialogForNow();
        if (currentDialogue.Count == 0) return;

        //2. 대화 상태로 UI 조정하는 기능
        AdjustUIAndStart(interactBunny);
    }

    /// <summary>
    /// 선물하기 대화
    /// </summary>
    /// <param name="interactBunny">선물할 토끼</param>
    /// <param name="presentNum">선물 index 번호</param>
    public void StartPresentTalk(GameObject interactBunny, Item present)
    {
        // 먼저 선물에 대한 처리를 합니다.
        GameManager.instance.UseItem(present, false);       // 선물한 아이템 사용
        int presentNum = present.info.indexNum;

        // presentNum - 1인 이유는 아이템은 1부터 시작하고 대화는 0부터 시작해서입니다.
        currentDialogue = interactBunny.GetComponent<Dialogue>().DialogForPresent(presentNum - 1);
        if (currentDialogue.Count == 0) return;

        // 마지막으로 대화 상태로 UI를 조정합니다.
        AdjustUIAndStart(interactBunny);
    }

    private void AdjustUIAndStart(GameObject interactBunny)
    {
        //게임 전체 상태 변환
        GameManager.instance.IsTalking = true;

        //플레이어 정지
        Player.instance.GetComponent<Animator>().SetBool("playerWalk", false);
        Player.instance.PlayerStop();

        //대화 토끼랑 바라보게 조정
        if (interactBunny.GetComponent<RandomMoving>() != null)
            interactBunny.GetComponent<RandomMoving>().FlipForTalk();

        //UI 조정
        basicUI.SetActive(false);
        talkPanel.SetActive(true);
        talkPanel.GetComponent<Button>().interactable = true;

        //카메라 조정
        Vector2 playerPos = Player.instance.transform.position;
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

        SpecialDialogueAction();
    }

    private void SpecialDialogueAction()
    {
        finishDialogText.SetActive(false);
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);

        if (currentDialogNum == currentDialogue.Count - 1)
        {
            finishDialogText.SetActive(true);
        }

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
            int questNum = int.Parse(currentDialogue[currentDialogNum][4]);

            QuestManager.instance.ChangeQuestState((Quest)questNum, false, true);
            GameManager.instance.IsQuesting = false;
        }
        else if (currentDialogue[currentDialogNum][3] == "Item")    // 아이템일 경우
        {
            int itemNum = int.Parse(currentDialogue[currentDialogNum][4]);

            GameManager.instance.GetItem(itemNum);
        }
        else if (currentDialogue[currentDialogNum][3] == "Affinity")// 호감도일 경우
        {
            int changeAmount = int.Parse(currentDialogue[currentDialogNum][4]);

            GameManager.instance.AffinityChange(currentInteractBunny.GetComponent<Dialogue>().myName, changeAmount);
        }
        else if (currentDialogue[currentDialogNum][3] == "Clear")   // 클리어일 경우
        {
            EndTalk();
            StageClear();
        }
    }

    private void EndTalk()
    {
        //currentInteractBunny.GetComponent<Dialogue>().GiveReward(currentDialogue);

        InitUI();
        Player.instance.joystick.StopMoving();
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