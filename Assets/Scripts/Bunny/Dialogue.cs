using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BunnyName { Gotgam, Jadupudding, Kkingkkang, Mango, Pulttegi, Ssookgat, Tomatotang, Yanggaeng, Yuza, None }

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextAsset fixDialogueAsset;
    [SerializeField] private TextAsset normalDialogueAsset;
    [SerializeField] private TextAsset presentDialogueAsset;
    public Quest quest;
    public GameObject TalkableIcon;
    [SerializeField] private int nowTalkCnt = 0;
    public BunnyName myName;

    private List<List<List<string>>> fixDialogues = new List<List<List<string>>>();
    private List<List<List<string>>> normalDialogues = new List<List<List<string>>>();
    private List<List<List<string>>> presentDialogues = new List<List<List<string>>>();

    private void Start()
    {
        normalDialogues = Parser.DialogParse(normalDialogueAsset);

        if (fixDialogueAsset != null) fixDialogues = Parser.DialogParse(fixDialogueAsset);
        if (presentDialogueAsset != null) presentDialogues = Parser.DialogParse(presentDialogueAsset);
    }

    public List<List<string>> DialogForNow()
    {
        List<List<string>> dialog = new List<List<string>>();

        nowTalkCnt += 1;
        if (nowTalkCnt >= GameManager.instance.MaxTalkCnt)
        {
            if (TalkableIcon == null) Debug.Log(gameObject.name + "에게 대화 가능 아이콘이 할당 되지 않았습니다.");
            else TalkableIcon.SetActive(false);
        }

        // 보여줄 대화 번호
        int dialogCnt = 0;

        if (myName != BunnyName.None)
        {
            int bunnyNum = (int)myName;
            int talkCnt = GameManager.instance.TalkCnt[bunnyNum];

            if (nowTalkCnt > GameManager.instance.MaxTalkCnt)
            {
                Debug.Log("최대 대화 횟수를 초과했습니다.");
                return dialog;
            }

            if (talkCnt == 0) return fixDialogues[0];
            if (nowTalkCnt == 1) return fixDialogues[GameManager.instance.Stage];
            dialogCnt = GameManager.instance.TalkCnt[(int)myName];
        }
        else
        {
            dialogCnt = QuestManager.instance.State(quest);
        }

        // 해당 대화 번호에 대화가 있으면
        if (normalDialogues.Count > dialogCnt)
        {
            dialog = new List<List<string>>(normalDialogues[dialogCnt]);
        }

        return dialog;
    }

    public List<List<string>> DialogForPresent(int presentNum)
    {
        List<List<string>> dialog = new List<List<string>>();

        if (presentDialogues.Count > presentNum) dialog = presentDialogues[presentNum];

        return dialog;
    }
}