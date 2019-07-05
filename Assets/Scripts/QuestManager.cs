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

    private void Update()
    {
        switch (GameManager.instance.State)
        {
            case GameManager.TrainState.normal:
                if (GetUnperformQuest() == 0) return;

                GameManager.instance.ChangeTrainState(GameManager.TrainState.normalQuest);

                if (questCanvas == null) questCanvas = Instantiate(questCanvasPrefab);
                int questNum = GetUnperformQuest();
                
                questCanvas.GetComponent<QuestCanvasController>().ActivateCanvas(questNum);
                break;
            case GameManager.TrainState.talking:
                if (questCanvas != null) questCanvas.SetActive(false);
                break;
            case GameManager.TrainState.normalQuest:
                if (questCanvas != null) questCanvas.SetActive(true);
                RestrictCellMove();

                if (GetUnperformQuest() == 0)
                {
                    GameManager.instance.ChangeTrainState(GameManager.TrainState.normal);
                    Debug.Log("모든 퀘스트가 수행되어서 다시 정상 상태로 돌아옵니다.");
                }
                break;
        }
    }

    public int GetUnperformQuest()
    {
        for (int i = 0; i < isAccept.Count; i++)
        {
            if (isAccept[i] == true) return i;
        }
        return 0;
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