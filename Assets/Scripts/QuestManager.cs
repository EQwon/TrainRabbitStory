using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Quest { None, Tutorial, PhoneCall, PickUp, Crammed, StageClear };

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

            if (i == (int)Quest.Tutorial && GameManager.instance.level == GameManager.Level.kinder)
            {
                isAccept[i] = true;
                isSuccess[i] = true;
            }
            if (i == (int)Quest.StageClear)
            {
                isAccept[i] = true;
                isSuccess[i] = true;
            }
        }
    }

    public void StartUnperfomedQuest()
    {
        if (GetUnperformQuest() == 0) return;

        int questNum = GetUnperformQuest();
        StartQuest(questNum);
    }

    public void CheckStageClear()
    {
        if (isAccept[(int)Quest.StageClear] == false && isSuccess[(int)Quest.StageClear] == true)
        {
            questCanvas = Instantiate(questCanvasPrefab);
            questCanvas.GetComponent<QuestCanvasController>().ActivateCanvas((int)Quest.StageClear);
        }
    }

    private int GetUnperformQuest()
    {
        for (int i = 0; i < isAccept.Count; i++)
        {
            if (i == (int)Quest.StageClear) continue;
            if (isAccept[i] == true) return i;
        }
        return 0;
    }

    private void StartQuest(int questNum)
    {
        questCanvas = Instantiate(questCanvasPrefab);
        questCanvas.GetComponent<QuestCanvasController>().ActivateCanvas(questNum);

        switch (questNum)
        {
            case 1:
                GameManager.instance.ChangeTrainState(GameManager.TrainState.normalQuest);
                break;
            case 2:
                GameManager.instance.ChangeTrainState(GameManager.TrainState.instantQuest);
                break;
            case 3:
                GameManager.instance.ChangeTrainState(GameManager.TrainState.normalQuest);
                break;
            case 4:
                GameManager.instance.ChangeTrainState(GameManager.TrainState.instantQuest);
                break;
        }
    }

    public void BackToNoraml()
    {
        GameManager.instance.ChangeTrainState(GameManager.TrainState.normal);
        Destroy(questCanvas);
        questCanvas = null;
        UIManager.instance.gameObject.SetActive(true);
    }
}