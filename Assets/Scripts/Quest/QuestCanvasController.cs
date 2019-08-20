using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestCanvasController : MonoBehaviour
{
    public GameObject[] canvas;

    public void ActivateCanvas(int canvasNum)
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
        Debug.Log(canvas[canvasNum - 1].name + "를 활성화합니다.");
        canvas[canvasNum - 1].SetActive(true);
    }

    public void GoToTitleScene()
    {
        SceneManager.LoadScene(0);
        GameManager.instance.gameObject.GetComponent<QuestManager>().enabled = false;
    }
}
