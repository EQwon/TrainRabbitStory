﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
    public GameObject effectPanel;

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

    [Header("Stage Clear")]
    public GameObject stageClearPanel;
    public List<Sprite> clearMent;

    private List<Dialog> currentDialogue;
    private GameObject currentInteractBunny;
    public GameObject CurrentInteractBunny { get { return currentInteractBunny; } set { currentInteractBunny = value; } }
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
        effectPanel.SetActive(false);
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
        currentDialogue = interactBunny.GetComponent<Dialogue>().DialogueForNow();
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
        currentDialogue = interactBunny.GetComponent<Dialogue>().DialogueForNow();
        Debug.Log(currentDialogue.Count);
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
        currentDialogue = interactBunny.GetComponent<StoryDialogue>().DialogForPresent(presentNum);
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

        talkPanel.SetActive(true);
        talkPanel.GetComponent<Button>().interactable = true;
        finishDialogText.SetActive(false);
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);
        SoundManager.instance.TalkSE();

        currentDialogue[currentDialogNum].Run(this);
    }

    #region 각 대화 클래스에 해당하는 함수
    public void ShowTalkDialog(BasicDialog dialog)
    {
        speakerName.text = dialog.speakerName;
        speakerText.text = dialog.speakerText;
        speakerImage.sprite = dialog.speakerImg;

        if (currentDialogNum == currentDialogue.Count - 1)
        {
            finishDialogText.SetActive(true);
        }
    }

    public void ShowARButton()
    {
        talkPanel.GetComponent<Button>().interactable = false;
        acceptQuestButton.SetActive(true);
        rejectQuestButton.SetActive(true);
        finishDialogText.SetActive(false);
    }

    public void ShowChoiceDialog(BasicDialog dialog, List<string> choiceTexts)
    {
        speakerName.text = dialog.speakerName;
        speakerText.text = dialog.speakerText;
        speakerImage.sprite = dialog.speakerImg;

        talkPanel.GetComponent<Button>().interactable = false;
        choicePanel.SetActive(true);
        for (int i = 0; i < choiceTexts.Count; i++)
        {
            GameObject choice = Instantiate(choiceCard, choicePanel.transform);
            choice.GetComponent<ChoiceCard>().SetChoice(choiceTexts, i);
        }
    }

    public void ShowReactionDialog(List<BasicDialog> dialogs)
    {
        BasicDialog dialog = dialogs[choosedNum];

        speakerName.text = dialog.speakerName;
        speakerText.text = dialog.speakerText;
        speakerImage.sprite = dialog.speakerImg;
    }

    public void ShowReactionAffDialog()
    {
        //else if (currentDialogue[currentDialogNum][3] == "RE_Affinity") // 대답에 따른 호감도 변화
        //{
        //    int changeAmount = int.Parse(currentDialogue[currentDialogNum][4 + choosedNum]);

        //    BunnyName bunnyName = currentInteractBunny.GetComponent<StoryDialogue>().myName;
        //    GameManager.instance.StoryBunny(bunnyName).ChangeAffinity(changeAmount);
        //}
    }

    public void QuestFinish()
    {
        currentInteractBunny.GetComponent<Quest>().FinishQuest();
        GameManager.instance.IsQuesting = false;
    }

    public void ShowItemDialog()
    {
        //int itemNum = int.Parse(currentDialogue[currentDialogNum][4]);

        //GameManager.instance.GetItem(itemNum);
    }

    public void ShowAffinityDialog(BasicDialog dialog, int affinityAmount)
    {
        speakerName.text = dialog.speakerName;
        speakerText.text = dialog.speakerText;
        speakerImage.sprite = dialog.speakerImg;

        BunnyName bunnyName = currentInteractBunny.GetComponent<StoryDialogue>().myName;
        GameManager.instance.StoryBunny(bunnyName).ChangeAffinity(affinityAmount);
    }

    public void ShowClearDialog()
    {
        EndTalk();
        StageClear();
    }

    public void ShowScreenEffect(Color color, float duration)
    {
        talkPanel.SetActive(false);
        effectPanel.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(effectPanel.GetComponent<Image>().DOColor(color, duration))
            .AppendCallback(() => ScreenEffect(color));
    }
    #endregion

    private void ScreenEffect(Color color)
    {
        if (color.a == 0) effectPanel.SetActive(false);
        NextDialog();
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
        Quest quest = currentInteractBunny.GetComponent<QuestDialogue>().MyQuest;
        QuestManager.instance.QuestStart(quest.QuestName);
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