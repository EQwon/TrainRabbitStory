using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject canvas;

    public enum Level { kinder, elementary, middle, high, university }

    public Level level = Level.kinder;
    public bool canMove = true;
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
            ChangeMoveState(false);

            HP = 100;
            MP = 100;

            InitStage();
            UIManager.instance.ShowOpeningStory(level);
        }
    }

    public void InitStage()
    {
        //Debug.Log("현재 스테이지는 " + stage);
        Instantiate(canvas);
        trainController.SetupTrain(level);
        Debug.Log("Initialize Stage Finish");
    }

    public void NextStage()
    {
        stage++;
        //Debug.Log("스테이지를 " + stage + "로 올렸습니다.");
        trainController.CellNum = stage;
        trainController.SetupTrain(level);
        UIManager.instance.TrainCellNumberUpdate();
        GameManager.instance.ChangeMoveState(true);
    }

    public void ChangeMoveState(bool isTrue)
    {
        canMove = isTrue;

        if (isTrue == true) Time.timeScale = 1;
        else Time.timeScale = 0;
    }
}
