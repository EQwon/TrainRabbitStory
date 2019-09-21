using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Quest { None, Tutorial, PhoneCall, PickUp, Crammed };

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;

    public List<bool> isAccept = new List<bool>();
    public List<bool> isSuccess = new List<bool>();

    public GameObject questCanvasPrefab;
    private GameObject questCanvas;

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

    public void Init()
    {
        isAccept.Clear();
        isSuccess.Clear();
        int n = Enum.GetNames(typeof(Quest)).Length;

        for (int i = 0; i < n; i++)
        {
            isAccept.Add(false);
            isSuccess.Add(false);

            if (i == (int)Quest.Tutorial && GameManager.instance.Stage == 0)
            {
                isAccept[i] = true;
                isSuccess[i] = true;
            }
        }
    }

    public int State(Quest quest)
    {
        if (isAccept[(int)quest] == false)
        {
            if (isSuccess[(int)quest] == false) return 0;
            else return 3;
        }
        else
        {
            if (isSuccess[(int)quest] == false) return 1;
            else return 2;
        }
    }

    public void ChangeQuestState(Quest quest, bool accept, bool success)
    {
        isAccept[(int)quest] = accept;
        isSuccess[(int)quest] = success;
    }

    public void StartUnperfomedQuest()
    {
        int questNum = GetUnperformQuest();

        if (GameManager.instance.IsQuesting == true) return;
        if (questNum == 0) return;
        
        if (questNum != 1) // 튜토리얼은 캔버스 생성 안함.
        {
            questCanvas = Instantiate(questCanvasPrefab);
            questCanvas.GetComponent<QuestCanvasController>().ActivateCanvas(questNum);
        }

        GameManager.instance.IsQuesting = true;
    }

    private int GetUnperformQuest()
    {
        for (int i = 0; i < isAccept.Count; i++)
        {
            if (isAccept[i] == true) return i;
        }
        return 0;
    }

    public void BackToNoraml()
    {
        GameManager.instance.IsQuesting = false;
        Destroy(questCanvas);
        questCanvas = null;
        UIManager.instance.gameObject.SetActive(true);
    }
}