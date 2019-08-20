using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public Sprite[] tutoImage;
    public Image tuto;

    private int num = 0;

    private void Update()
    {
        tuto.sprite = tutoImage[num];
    }

    public void NextTuto()
    {
        num += 1;

        if (num == tutoImage.Length)
        {
            Destroy(gameObject);
        }
    }
}
