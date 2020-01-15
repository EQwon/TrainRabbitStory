using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TrainState { normal, talking, normalQuest, instantQuest, cellChange }

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public TextAsset itemAsset;
    private List<ItemInfo> itemIndexList = new List<ItemInfo>();
    public List<Item> itemList = new List<Item>();

    #region SaveLoad
    private Data data;
    private void SaveData()
    {
        SaveSystem.SaveData(data);
    }
    public void LoadData()
    {
        data = SaveSystem.LoadData();
    }
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
        set
        {
            data.hp = value;
            if (data.hp > 1000) data.hp = 1000;
        }
    }
    public int MP
    {
        get { return data.mp; }
        set
        {
            data.mp = value;
            if (data.mp > 100) data.mp = 100;
        }
    }
    public StoryBunny StoryBunny(BunnyName name)
    {
        return data.storyBunnies[(int)name];
    }
    public int MaxStoryTalkCnt
    {
        get
        {
            if (Stage == 1)
                return 1;
            else
            {
                if (MP >= 50) return 5;
                else return 3;
            }
        }
    }
    public int MaxNormalTalkCnt
    {
        get { return 3; }
    }
    public int MaxCellNum
    {
        get { return Stage >= 1 ? 16 : 11; }
    }
    #endregion

    #region TrainState
    [SerializeField] private bool isTalking = false;
    [SerializeField] private bool isQuesting = false;
    private bool isCellChanging = false;

    public bool IsTalking { set { isTalking = value; ChangeTrainState(); } }
    public bool IsQuesting { get { return isQuesting; } set { isQuesting = value; ChangeTrainState(); } }
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

            UIManager.instance.ShowOpeningStory();
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

    public void UseItem(Item usingItem, bool forMe)
    {
        Item targetItem = itemList.Find(x => x == usingItem);

        if (forMe)
        {
            // 왜 10배냐면 최대 체력을 1000이라고 뒀기 때문!
            HP += targetItem.info.hpChange * 10;
            MP += targetItem.info.mpChange;
        }

        targetItem.amount -= 1;
        if (targetItem.amount == 0) itemList.Remove(targetItem);
    }

    public void StageClear()
    {
        gameObject.GetComponent<QuestManager>().enabled = false;

        data.stage += 1;

        SaveData();
        SceneManager.LoadScene(0);
    }
}