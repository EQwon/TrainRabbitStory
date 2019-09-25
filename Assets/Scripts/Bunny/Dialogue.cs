using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextAsset fixDialogueAsset;
    public TextAsset normalDialogueAsset;
    public Quest quest;
    public GameObject TalkableIcon;
    [SerializeField] int nowTalkCnt = 0;

    private List<List<List<string>>> fixDialogues = new List<List<List<string>>>();
    private List<List<List<string>>> normalDialogues = new List<List<List<string>>>();

    private void Start()
    {
        normalDialogues = Parser.DialogParse(normalDialogueAsset);

        if (fixDialogueAsset == null) return;
        fixDialogues = Parser.DialogParse(fixDialogueAsset);
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

        if (GetComponent<Affinity>())
        {
            int bunnyNum = GetComponent<Affinity>().bunnyNum;
            int talkCnt = GameManager.instance.TalkCnt[bunnyNum];

            if (nowTalkCnt > GameManager.instance.MaxTalkCnt)
            {
                Debug.Log("최대 대화 횟수를 초과했습니다.");
                return dialog;
            }

            if (talkCnt == 0) return fixDialogues[0];
            if (nowTalkCnt == 1) return fixDialogues[GameManager.instance.Stage];
            dialogCnt = GameManager.instance.TalkCnt[GetComponent<Affinity>().bunnyNum];
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
}