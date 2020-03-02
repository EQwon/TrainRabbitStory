using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;

    private List<Quest> quests = new List<Quest>();

    public GameObject questCanvasPrefab;
    private QuestCanvasController questCanvas;

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

        questCanvas = Instantiate(questCanvasPrefab).GetComponent<QuestCanvasController>();
    }

    public void AddQuest(Quest myQuest)
    {
        quests.Add(myQuest);
    }

    public void AddQuestCanvas(QuestName questName, GameObject questCanvas)
    {
        this.questCanvas.AddCanvas(questName, questCanvas);
    }

    public void ActivateQuestCanvas(QuestName questName)
    {
        questCanvas.ActivateCanvas(questName);
    }

    public void DeactivateQuestCanvas(QuestName questName)
    {
        questCanvas.DeactivateCanvas(questName);
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

    public void UpdateQuest()
    {
        //퀘스트 패널의 카드 정렬
        UIManager.instance.ClearQuestCard();

        //성공한 퀘스트 카드 삽입
        foreach (Quest quest in SuccessQuestList())
        {
            UIManager.instance.AddQuestCard(quest, true);
        }

        //진행 중인 퀘스트 카드 삽입
        foreach (Quest quest in ProgressQuestList())
        {
            UIManager.instance.AddQuestCard(quest, false);
        }
    }

    private List<Quest> ProgressQuestList()
    {
        List<Quest> returnList = new List<Quest>();

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].GetState() == QuestState.DoingQuest) returnList.Add(quests[i]);
        }
        return returnList;
    }

    private List<Quest> SuccessQuestList()
    {
        List<Quest> returnList = new List<Quest>();

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].GetState() == QuestState.FinishQuest) returnList.Add(quests[i]);
        }
        return returnList;
    }

    private List<Quest> AfterQuestList()
    {
        List<Quest> returnList = new List<Quest>();

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].GetState() == QuestState.AfterQuest) returnList.Add(quests[i]);
        }
        return returnList;
    }

    public void QuestStart(QuestName questName)
    {
        Quest targetQuest = GetCorrespondQuest(questName);

        targetQuest.StartQuest();
    }

    public void QuestFinish(QuestName questName)
    {
        Quest targetQuest = GetCorrespondQuest(questName);

        targetQuest.FinishQuest();
    }

    public void BackToNoraml()
    {
        GameManager.instance.IsQuesting = false;
        Destroy(questCanvas);
        questCanvas = null;
        UIManager.instance.gameObject.SetActive(true);
    }

    private Quest GetCorrespondQuest(QuestName questName)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            Quest quest = quests[i];
            if (quest.QuestName == questName) return quest;
        }

        Debug.LogError(questName.ToString() + "에 해당하는 퀘스트를 찾지 못했습니다.");
        return null;
    }
}