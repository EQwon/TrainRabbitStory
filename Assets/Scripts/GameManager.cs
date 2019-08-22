using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public enum Level { kinder, elementary, middle, high, university }
    public enum TrainState { normal, talking, normalQuest, instantQuest, cellChange }

    public Level level = Level.kinder;
    private TrainState state = TrainState.normal;
    public TrainState State { get { return state; } }
    private TrainState previousState = TrainState.normal;

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
        GetComponent<QuestManager>().enabled = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Debug.Log("새로운 스테이지가 시작했습니다.");

            InitStage();
            GetComponent<QuestManager>().enabled = true;
            QuestManager.instance.Init();

            if(SceneManager.GetActiveScene().buildIndex == 1) UIManager.instance.ShowOpeningStory();
        }
    }

    public void InitStage()
    {
        TimeManager.timeScale = 1;
    }

    public void ChangeTrainState(TrainState trainState)
    {
        previousState = state;
        state = trainState;

        switch (state)
        {
            case TrainState.normal:
                TimeManager.timeScale = 1;
                break;
            case TrainState.talking:
                TimeManager.timeScale = 0;
                break;
            case TrainState.cellChange:
                TimeManager.timeScale = 1;
                break;
            case TrainState.normalQuest:
                TimeManager.timeScale = 1;
                break;
            case TrainState.instantQuest:
                TimeManager.timeScale = 0;
                break;
        }
    }

    public void BackToPreviousState()
    {
        ChangeTrainState(previousState);
        previousState = state;
    }
}
