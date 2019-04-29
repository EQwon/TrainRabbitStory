using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject canvas;

    public enum Level { kinder, elementary, middle, high, university }

    private Level level = Level.kinder;
    private int stage = 1;

    private TrainController trainController;

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

        DontDestroyOnLoad(gameObject);

        trainController = GetComponent<TrainController>();

        InitStage();
    }

    public void InitStage()
    {
        //Debug.Log("현재 스테이지는 " + stage);
        Instantiate(canvas);
        trainController.SetupTrain(level);
        //Debug.Log("Initialize Stage Finish");
    }

    public void NextStage()
    {
        stage++;
        //Debug.Log("스테이지를 " + stage + "로 올렸습니다.");
        trainController.stageNum = stage;
        trainController.SetupTrain(level);
    }
}
