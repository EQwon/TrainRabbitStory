using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestName { Tutorial, PhoneCall, PickUp, Crammed, Skipping, Thonos };
public enum QuestState { BeforeQuest, DoingQuest, FinishQuest, AfterQuest };

[System.Serializable]
public class Quest : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected QuestName questName;
    [SerializeField] private GameObject questCanvas;
    [SerializeField] private bool isAccept;
    [SerializeField] private bool isFinish;
    [SerializeField] private bool isInstant;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] protected int score = 0;

    protected GameObject myQuestCanvas;

    public QuestName QuestName { get { return questName; } }
    public bool IsAccpet { get { return isAccept; } }
    public bool IsFinish { get { return isFinish; } }
    public bool IsInstant { get { return isInstant; } }
    public int Score { get { return score; } set { score = value; } }
    public string Title
    {
        get
        {
            if (isInstant)
            {
                Debug.Log("즉시 퀘스트는 타이틀이 없습니다.");
                return null;
            }
            else return title;
        }
    }
    public string Description
    {
        get
        {
            if (isInstant)
            {
                Debug.Log("즉시 퀘스트는 설명이 없습니다.");
                return null;
            }
            else return description;
        }
    }

    private void SetState(QuestState state)
    {
        if (state == QuestState.BeforeQuest) isAccept = false; isFinish = false;
        if (state == QuestState.DoingQuest) isAccept = true; isFinish = false;
        if (state == QuestState.FinishQuest) isAccept = true; isFinish = true;
        if (state == QuestState.AfterQuest) isAccept = false; isFinish = true;
    }

    public QuestState GetState()
    {
        if (!isAccept && !isFinish) return QuestState.BeforeQuest;
        if (isAccept && !isFinish) return QuestState.DoingQuest;
        if (isAccept && isFinish) return QuestState.FinishQuest;
        if (!isAccept && isFinish) return QuestState.AfterQuest;

        Debug.LogError("????? 어떻게 왔어...?");
        return QuestState.BeforeQuest;
    }

    private void Start()
    {
        InitializeQuest();
    }

    private void InitializeQuest()
    {
        // QuestManager에 자신을 등록
        QuestManager.instance.AddQuest(this);
        // 자신 전용 Quest Canvas를 생성
        myQuestCanvas = Instantiate(questCanvas);
        myQuestCanvas.GetComponent<QuestPanel>().StartQuest(this);
        // 자신 전용 Quest Canvas를 QuestCanvas에 등록
        QuestManager.instance.AddQuestCanvas(questName, myQuestCanvas);
        myQuestCanvas.SetActive(false);
    }

    /// <summary>
    /// 퀘스트를 시작하는 함수
    /// </summary>
    public virtual void StartQuest()
    {
        // 퀘스트의 상태를 DoingQuest로 바꿔주고
        SetState(QuestState.DoingQuest);
        // Quest Canvas On!
        QuestManager.instance.ActivateQuestCanvas(questName);
    }

    /// <summary>
    /// 퀘스트의 결과를 확정짓는 함수.
    /// </summary>
    public void EndQuest()
    {
        SetState(QuestState.FinishQuest);
        QuestManager.instance.DeactivateQuestCanvas(questName);
    }

    /// <summary>
    /// 퀘스트를 종료하는 함수. 퀘스트 토끼에게 대화를 걺으로써 불린다.
    /// </summary>
    public void FinishQuest()
    {
        SetState(QuestState.AfterQuest);
        // 퀘스트 완료에 대한 이벤트를 실행
        Reward();
    }

    protected virtual void Reward()
    { }
}
