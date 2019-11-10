using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCanvasController : MonoBehaviour
{
    public GameObject[] canvas;

    public void ActivateCanvas(int canvasNum)
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
        Debug.Log(canvas[canvasNum].name + "를 활성화합니다.");
        canvas[canvasNum].SetActive(true);
    }
}
