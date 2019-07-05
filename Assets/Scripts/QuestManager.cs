using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Quest { None, Tutorial, PhoneCall, PickUp };

[System.Serializable]
public class QuestRestrict
{
    public int maxCellNum;
    public string warningText;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;

    public List<bool> isAccept = new List<bool>();
    public List<bool> isSuccess = new List<bool>();
    public List<QuestRestrict> restrict;

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

        int n = Enum.GetNames(typeof(Quest)).Length;

        for (int i = 0; i < n; i++)
        {
            isAccept.Add(false);
            isSuccess.Add(false);
            if (i == 1)
            {
                isAccept[1] = true;
                isSuccess[1] = true;
            }
        }
    }

    public void StartUnperfomedQuest()
    {
        if (GetUnperformQuest() == 0) return;

        int questNum = GetUnperformQuest();
        StartQuest(questNum);
    }

    private int GetUnperformQuest()
    {
        for (int i = 0; i < isAccept.Count; i++)
        {
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
        }
    }

    public void BackToNoraml()
    {
        GameManager.instance.ChangeTrainState(GameManager.TrainState.normal);
        Destroy(questCanvas);
        questCanvas = null;
        UIManager.instance.gameObject.SetActive(true);
    }

    public void RestrictCellMove()
    {
        int questNum = GetUnperformQuest();
        float maxPosX = (GameManager.instance.cellLength - restrict[questNum].maxCellNum) * 20 + 9f;

        if (Player.instance.gameObject.transform.position.x >= maxPosX)
        {
            UIManager.instance.Warning(restrict[questNum].warningText);

            float posY = Player.instance.gameObject.transform.position.y;
            Player.instance.gameObject.transform.position = new Vector3(maxPosX - 0.5f, posY, 0);
        }
    }
}