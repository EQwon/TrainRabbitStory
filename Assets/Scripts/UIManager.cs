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
    public GameObject choicePanel;
    public GameObject choiceCard;

    [Header("Basic UI")]
    public GameObject basicUI;
    public Text cellNumberText;
    public GameObject healthBar;
    public GameObject heart;
    public GameObject WarningPanel;
    public GameObject talkButton;

    [Header("Bag")]
    public List<Sprite> bagImage;
    public Image bag;
    public GameObject bagPanel;

    [Header("Quest")]
    public GameObject questPanel;
    public GameObject questCard;
    private List<GameObject> questCards = new List<GameObject>();

    [Header("For Opening")]
    public TextAsset openingAsset;
    public GameObject darkPanel;

    [Header("Stage Clear")]
    public GameObject stageClearPanel;
    public List<Sprite> clearMent;

    private List<List<string>> currentDialogue;
    private GameObject currentInteractBunny;
    public GameObject CurrentInteractBunny { get { return currentInteractBunny; } }
    private int currentDialogNum = -1;
    private int choosedNum = 0;

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
        choicePanel.SetActive(false);
        basicUI.SetActive(true);
        WarningPanel.SetActive(false);
        darkPanel.SetActive(false);
        bagPanel.SetActive(false);
        stageClearPanel.SetActive(false);

        currentDialogNum = -1;

        GameObject mainCamera = Camera.main.gameObject;
        if (mainCamera.GetComponent<CameraWalk>().enabled == false)
            mainCamera.GetComponent<CameraWalk>().enabled = true;

        mainCamera.GetComponent<CameraWalk>().ZoomOutCamera();

        bag.sprite = bagImage[GameManager.instance.Stage];
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

        // 해당하는 번호에 대한 대사를 가져온다.
        currentDialogue = interactBunny.GetComponent<Dialogue_Story>().DialogForPresent(presentNum);
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
        if (currentDialogue[currentDialogNum].Count > 2 && currentDialogue[currentDialogNum][2] != "")
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
            List<string> nowDialog = currentDialogue[currentDialogNum];

            talkPanel.GetComponent<Button>().interactable = false;
            choicePanel.SetActive(true);
            for (int i = 4; i < nowDialog.Count; i++)
            {
                GameObject choice = Instantiate(choiceCard, choicePanel.transform);
                choice.GetComponent<ChoiceCard>().SetChoice(nowDialog[i], nowDialog.Count - 3, i - 4);
            }
            finishDialogText.SetActive(false);
        }
        else if (currentDialogue[currentDialogNum][3] == "RE")      // 대답일 경우
        {
            speakerText.text = currentDialogue[currentDialogNum][4 + choosedNum];
        }
        else if (currentDialogue[currentDialogNum][3] == "Quest")   // 퀘스트의 완료일 경우
        {
            int questNum = int.Parse(currentDialogue[currentDialogNum][4]);

            QuestManager.instance.GetQuest((QuestName)questNum).ChangeQuestState(false, true);
            GameManager.instance.IsQuesting = false;
        }
        else if (currentDialogue[currentDialogNum][3] == "Item")    // 아이템일 경우
        {
            int itemNum = int.Parse(currentDialogue[currentDialogNum][4]);

            //GameManager.instance.GetItem(itemNum);
        }
        else if (currentDialogue[currentDialogNum][3] == "Affinity")// 호감도일 경우
        {
            int changeAmount = int.Parse(currentDialogue[currentDialogNum][4]);

            BunnyName bunnyName = currentInteractBunny.GetComponent<Dialogue_Story>().myName;
            GameManager.instance.StoryBunny(bunnyName).ChangeAffinity(changeAmount);
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

        QuestManager.instance.UpdateQuest();
    }

    public void AcceptQuest()
    {
        //UI 조정
        talkPanel.GetComponent<Button>().interactable = true;
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);

        //퀘스트 수락 전달
        Quest quest = currentInteractBunny.GetComponent<Dialogue_Quest>().MyQuest;
        QuestManager.instance.GetQuest(quest.QuestName).ChangeQuestState(true, false);
        Debug.Log(quest.QuestName.ToString() + " 퀘스트를 수락합니다.");

        //수락시 대사 출력
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

    public void Choose(int i)
    {
        //UI 조정
        talkPanel.GetComponent<Button>().interactable = true;
        choicePanel.SetActive(false);

        //선택한 정보 저장
        choosedNum = i;

        //선택한 선택지에 대한 대사 출력
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

    public void ClearQuestCard()
    {
        for (int i = questCards.Count - 1; i >= 0; i--)
        {
            Destroy(questCards[i]);
        }

        questCards = new List<GameObject>();
    }

    public void AddQuestCard(Quest quest, bool isSuccess = false)
    {
        GameObject card = Instantiate(questCard, questPanel.transform);
        questCards.Add(card);
        card.GetComponent<QuestCardSizeFitter>().SetCard(quest.Title, quest.Description, isSuccess);

        float posY = -50f;

        if (questCards.Count != 1)
        {
            RectTransform previousRect = questCards[questCards.Count - 2].GetComponent<RectTransform>();
            float previousPosY = previousRect.anchoredPosition.y;
            float previousHeight = previousRect.sizeDelta.y;

            posY = previousPosY - previousHeight;
        }

        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
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