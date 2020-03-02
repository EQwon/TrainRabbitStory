using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCanvasController : MonoBehaviour
{
    private Dictionary<QuestName, GameObject> canvasDict = new Dictionary<QuestName, GameObject>();

    public void AddCanvas(QuestName questName, GameObject questCanvas)
    {
        questCanvas.transform.parent = gameObject.transform;
        canvasDict.Add(questName, questCanvas);
    }

    public void ActivateCanvas(QuestName questName)
    {
        canvasDict[questName].SetActive(true);
    }

    public void DeactivateCanvas(QuestName questName)
    {
        canvasDict[questName].SetActive(false);
    }
}
