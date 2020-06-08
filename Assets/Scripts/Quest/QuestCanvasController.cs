using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCanvasController : MonoBehaviour
{
    private Dictionary<QuestName, GameObject> canvasDict = new Dictionary<QuestName, GameObject>();

    public void AddCanvas(QuestName questName, GameObject questCanvas)
    {
        questCanvas.transform.SetParent(gameObject.transform);
        RectTransform rect = questCanvas.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
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
