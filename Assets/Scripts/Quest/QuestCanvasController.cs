using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCanvasController : MonoBehaviour
{
    public GameObject[] canvas;

    public void ActivateCanvas(int canvasNum)
    {
        if (canvas[canvasNum].activeInHierarchy) return;

        Debug.Log(canvas[canvasNum].name + "를 활성화합니다.");
        canvas[canvasNum].SetActive(true);
    }

    public void DeactivateCanvas(int canvasNum)
    {
        if (!canvas[canvasNum].activeInHierarchy) return;

        Debug.Log(canvas[canvasNum].name + "를 비활성화합니다.");
        canvas[canvasNum].SetActive(false);
    }
}
