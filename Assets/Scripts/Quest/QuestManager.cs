using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum QuestName{ Tutorial, PhoneCall, PickUp, Crammed };
public enum QuestState { BeforeQuest, AcceptQuest, SuccessQuest, AfterQuest };

public class Quest
{
    private QuestName name;
    private bool isAccept;
    private bool isSuccess;
    private bool isInstant;

    Quest(QuestName name, bool isAccept = false, bool isSuccess = false, bool isInstant = false)
    {
        this.name = name;
        this.isAccept = isAccept;
        this.isSuccess = isSuccess;
        this.isInstant = isInstant;
    }

    public QuestName Name { get { return name; } }
    public bool IsAccpet { get { return isAccept; } }
    public bool IsSuccess { get { return isSuccess; } }
    public bool IsInstant { get { return isInstant; } }

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

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;

    private List<Quest> quests = new List<Quest>();

    public GameObject questCanvasPrefab;
    private GameObject questCanvas = null;

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
    }

    public void AddQuest(Quest myQuest)
    {
        quests.Add(myQuest);
    }

    public Quest GetQuest(QuestName name)
    {
        foreach (Quest quest in quests)
        {
            if (quest.Name == name) return quest;
        }

        Debug.LogError("해당하는 퀘스트를 찾을 수 없습니다.");
        return null;
    }

    public void StartInstantQuest()
    {
        if(questCanvas == null) questCanvas = Instantiate(questCanvasPrefab);

        Quest targetQuest = null;

        foreach (Quest quest in ProgressQuestList())
        {
            if (quest.IsInstant) targetQuest = quest;
        }

        if (targetQuest == null) return;

        questCanvas.GetComponent<QuestCanvasController>().ActivateCanvas((int)targetQuest.Name);
        GameManager.instance.IsQuesting = true;
    }

    private List<Quest> ProgressQuestList()
    {
        List<Quest> returnList = new List<Quest>();

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].State() == QuestState.AcceptQuest) returnList.Add(quests[i]);
        }
        return returnList;
    }

    private List<Quest> SuccessQuestList()
    {
        List<Quest> returnList = new List<Quest>();

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].State() == QuestState.SuccessQuest) returnList.Add(quests[i]);
        }
        return returnList;
    }

    public void BackToNoraml()
    {
        GameManager.instance.IsQuesting = false;
        Destroy(questCanvas);
        questCanvas = null;
        UIManager.instance.gameObject.SetActive(true);
    }
}