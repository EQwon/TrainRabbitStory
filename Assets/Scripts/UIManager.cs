using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject talkPanel;
    public Image speakerImage;
    public Text speakerName;
    public Text speakerText;
    public GameObject finishDialogText;
    public GameObject acceptQuestButton;
    public GameObject rejectQuestButton;

    private GameObject mainCamera;    

    private List<Dialog> currentDialogue;
    private int currentDialogNum = -1;
    private int currentDialogSize = 0;
    private int currentQuestNum = 0;

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

        InitUI();
    }

    private void InitUI()
    {
        talkPanel.SetActive(false);
        finishDialogText.SetActive(false);
        acceptQuestButton.SetActive(false);
        rejectQuestButton.SetActive(false);

        currentDialogNum = -1;
        currentDialogSize = 0;

        mainCamera = GameObject.Find("Main Camera");

        mainCamera.transform.position = new Vector3(0, 0, -10f);
        mainCamera.GetComponent<Camera>().orthographicSize = 5.5f;
    }

    public void StartTalk(Vector2 playerPos, GameObject interactBunny)
    {
        Vector2 interactBunnyPos = interactBunny.transform.position;
        List<Dialog> dialogue = interactBunny.GetComponent<Dialogue>().dialogue;

        talkPanel.SetActive(true);
        mainCamera.transform.position = ZoomInCameraPos(playerPos, interactBunnyPos);
        mainCamera.GetComponent<Camera>().orthographicSize = 4f;
        currentDialogue = dialogue;
        currentDialogNum = -1;
        currentDialogSize = dialogue.Count;

        if (currentDialogue[currentDialogSize - 1].type == DialogType.Quest)
        {
            currentQuestNum = currentDialogue[currentDialogSize - 1].questNum;
        }

        NextDialog();
    }

    private Vector3 ZoomInCameraPos(Vector2 playerPos, Vector2 interactBunnyPos)
    {
        float cameraPos_X, cameraPos_Y;

        cameraPos_X = (playerPos.x + interactBunnyPos.x) / 2;
        cameraPos_Y = (playerPos.y + interactBunnyPos.y - 2f) / 2;

        if (cameraPos_X > 2.4f)
            cameraPos_X = 2.4f;
        else if (cameraPos_X < -2.4f)
            cameraPos_X = -2.4f;

        return new Vector3(cameraPos_X, cameraPos_Y, -10f);
    }

    public void NextDialog()
    {
        if(currentDialogNum >=0 && currentDialogue[currentDialogNum].type == DialogType.Quest)
        {
            return;
        }

        if(currentDialogNum >= currentDialogSize - 1)
        {
            InitUI();
            return;
        }

        currentDialogNum += 1;

        speakerName.text = currentDialogue[currentDialogNum].Speaker;
        speakerText.text = currentDialogue[currentDialogNum].Text;

        if(currentDialogNum == currentDialogSize - 1)
        {
            DialogueFinishNotice(currentDialogue[currentDialogNum].type);
        }
    }

    private void DialogueFinishNotice(DialogType type)
    {
        if(type == DialogType.Default)
        {
            finishDialogText.SetActive(true);
        }
        else if(type == DialogType.Quest)
        {
            acceptQuestButton.SetActive(true);
            rejectQuestButton.SetActive(true);
        }
    }

    public void AcceptQuest()
    {
        if(currentQuestNum == 0)
        {
            Debug.Log("퀘스트를 수락할 수 없습니다. 퀘스트가 지정 되지 않았습니다.");
            return;
        }

        InitUI();
        Debug.Log(currentQuestNum + "번의 퀘스트를 수락합니다.");
    }

    public void RejectQuest()
    {
        if (currentQuestNum == 0)
        {
            Debug.Log("퀘스트를 수락할 수 없습니다. 퀘스트가 지정 되지 않았습니다.");
            return;
        }

        InitUI();
        Debug.Log("퀘스트를 거절합니다.");
    }
}