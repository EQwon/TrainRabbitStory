using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TrainState { normal, talking, normalQuest, instantQuest, cellChange }

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    #region SaveLoad
    private Data data;
    private void SaveData() { SaveSystem.SaveData(data); }
    public void LoadData() { data = SaveSystem.LoadData();}
    #endregion

    #region GameStatus
    public int Stage
    {
        get { return data.stage; }
        set { data.stage = value; }
    }
    public int HP
    {
        get { return data.hp; }
        set { data.hp = value; }
    }
    public int MP
    {
        get { return data.mp; }
        set { data.mp = value; }
    }
    public int[] Affinity
    {
        get { return data.affinity; }
        set { data.affinity = value; }
    }
    public int[] TalkCnt
    {
        get { return data.talkCnt; }
        set { data.talkCnt = value; }
    }
    #endregion

    #region TrainState
    private bool isTalking = false;
    private bool isQuesting = false;
    private bool isCellChanging = false;

    public bool IsTalking { set { isTalking = value; ChangeTrainState(); } }
    public bool IsQuesting { set { isQuesting = value; ChangeTrainState(); } }
    public bool IsCellChanging { set { isCellChanging = value; ChangeTrainState(); } }

    private TrainState state = TrainState.normal;
    public TrainState State { get { return state; } }
    #endregion

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
        LoadData();

        //GetComponent<QuestManager>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SaveSystem.DeleteData();
        }
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

            TimeManager.timeScale = 1;
            GetComponent<QuestManager>().enabled = true;
            QuestManager.instance.Init();

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                UIManager.instance.ShowOpeningStory();
            }
        }
    }

    public void ChangeTrainState()
    {
        if (isCellChanging == true) state = TrainState.cellChange;
        else if (isTalking == true)
        {
            if (isQuesting == true) state = TrainState.instantQuest;
            else state = TrainState.talking;
        }
        else
        {
            if (isQuesting == true) state = TrainState.normalQuest;
            else state = TrainState.normal;
        }

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

    public void StageClear()
    {
        gameObject.GetComponent<QuestManager>().enabled = false;

        data.stage += 1;

        SaveData();
        SceneManager.LoadScene(0);
    }
}