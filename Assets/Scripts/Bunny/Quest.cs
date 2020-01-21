using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestName { Tutorial, PhoneCall, PickUp, Crammed, Skipping, Thonos };
public enum QuestState { BeforeQuest, AcceptQuest, SuccessQuest, AfterQuest };

[System.Serializable]
public class Quest
{
    [SerializeField] private QuestName questName;
    [SerializeField] private bool isAccept;
    [SerializeField] private bool isSuccess;
    [SerializeField] private bool isInstant;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private ScoreDialogue successDialogues;
    [SerializeField] private ScoreDialogue afterDialogues;
    [SerializeField] private int score = 0;

    public QuestName QuestName { get { return questName; } }
    public bool IsAccpet { get { return isAccept; } }
    public bool IsSuccess { get { return isSuccess; } }
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
    public ScoreDialogue SuccessDialogues { get { return successDialogues; } }
    public ScoreDialogue AfterDialogues { get { return afterDialogues; } }

    public void ChangeQuestState(bool accept, bool success)
    {
        isAccept = accept;
        isSuccess = success;
    }

    public QuestState State()
    {
        if (isAccept == false)
        {
            if (isSuccess == false) return QuestState.BeforeQuest;
            else return QuestState.AfterQuest;
        }
        else
        {
            if (isSuccess == false) return QuestState.AcceptQuest;
            else return QuestState.SuccessQuest;
        }
    }
}
