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

        //퀘스트 캔버스 오브젝트 활성화, 비활성화
        if (questCanvas == null) questCanvas = Instantiate(questCanvasPrefab);
        QuestCanvasController controller = questCanvas.GetComponent<QuestCanvasController>();

        foreach (Quest quest in quests)
        {
            if (quest.IsAccpet)
            {
                controller.ActivateCanvas((int)quest.QuestName);
                GameManager.instance.IsQuesting = true;
            }
            else controller.DeactivateCanvas((int)quest.QuestName);
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

    private List<Quest> AfterQuestList()
    {
        List<Quest> returnList = new List<Quest>();

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].State() == QuestState.AfterQuest) returnList.Add(quests[i]);
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