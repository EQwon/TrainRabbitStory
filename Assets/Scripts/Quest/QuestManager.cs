using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            if (quest.QuestName == name) return quest;
        }

        Debug.LogError("해당하는 퀘스트를 찾을 수 없습니다.");
        return null;
    }

    public bool StartInstantQuest()
    {
        if(questCanvas == null) questCanvas = Instantiate(questCanvasPrefab);

        Quest targetQuest = null;

        foreach (Quest quest in ProgressQuestList())
        {
            if (quest.IsInstant) targetQuest = quest;
        }

        if (targetQuest == null) return false;

        questCanvas.GetComponent<QuestCanvasController>().ActivateCanvas((int)targetQuest.QuestName);
        GameManager.instance.IsQuesting = true;
        return true;
    }

    public void UpdateQuestList()
    {
        UIManager.instance.ClearQuestCard();

        for (int i = 0; i < quests.Count; i++)
        {
            Quest quest = quests[i];
            if(quest.IsAccpet && !quest.IsInstant) UIManager.instance.AddQuestCard(quest);
        }
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