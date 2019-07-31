using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject canvas;

    public enum Level { kinder, elementary, middle, high, university }
    public enum TrainState { normal, talking, normalQuest, instantQuest, cellChange }

    public Level level = Level.kinder;
    private TrainState state = TrainState.normal;
    public TrainState State { get { return state; } }
    private TrainState previousState = TrainState.normal;
    public int cellLength { get { return level == Level.kinder ? 10 : 20; } }
    private float hp;
    public float HP {
        get { return hp; }
        set {   if (value >= 100) hp = 100;
                else hp = value; } }
    private float mp;
    public float MP {
        get { return mp; }
        set {
            if (value >= 100) mp = 100;
            else if (value <= 0) mp = 0;
            else mp = value; } }

    public List<GameObject> playerPrefabs;

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
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Debug.Log("새로운 스테이지가 시작했습니다.");
            ChangeTrainState(TrainState.talking);

            HP = 100;
            MP = 100;

            InitStage();
            UIManager.instance.ShowOpeningStory();
            GetComponent<QuestManager>().enabled = true;
        }
    }

    public void InitStage()
    {
        Instantiate(canvas);
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Vector3 spawnPos = new Vector3(-8f, 0, 0);
        Instantiate(playerPrefabs[(int)level], spawnPos, Quaternion.identity);
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
