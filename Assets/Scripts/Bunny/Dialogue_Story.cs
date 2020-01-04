using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Story : Dialogue
{
    [SerializeField] private TextAsset fixDialogueAsset;
    [SerializeField] private TextAsset presentDialogueAsset;
    public BunnyName myName;

    private List<List<List<string>>> fixDialogues = new List<List<List<string>>>();
    private List<List<List<string>>> presentDialogues = new List<List<List<string>>>();

    protected override void Start()
    {
        base.Start();

        fixDialogues = Parser.DialogParse(fixDialogueAsset);
        presentDialogues = Parser.DialogParse(presentDialogueAsset);
    }

    public override List<List<string>> DialogForNow()
    {
        List<List<string>> dialog = new List<List<string>>();

        if (nowTalkCnt > GameManager.instance.MaxTalkCnt)
        {
            Debug.Log("최대 대화 횟수를 초과했습니다.");
            return dialog;
        }

        // 대화할 때마다 호감도 증가
        GameManager.instance.StoryBunny(myName).ChangeAffinity(1);

        if (nowTalkCnt == 0)    // 이번 스테이지의 최초 대화라면 고정 대화를 출력하고 종료
        {
            nowTalkCnt += 1;
            return fixDialogues[GameManager.instance.Stage];
        }

        // 보여줄 대화 번호
        int dialogCnt = GameManager.instance.StoryBunny(myName).TalkCnt;

        // 해당 대화 번호에 대화가 있으면
        if (dialogues.Count > dialogCnt)
        {
            dialog = new List<List<string>>(dialogues[dialogCnt]);
        }
        else Debug.LogError(myName.ToString() + "에게 더 이상 할당되어 있는 대화가 없습니다.");

        nowTalkCnt += 1;
        return dialog;
    }

    public List<List<string>> DialogForPresent(int presentNum)
    {
        List<List<string>> dialog = new List<List<string>>();

        if (presentDialogues.Count > presentNum) dialog = presentDialogues[presentNum];

        return dialog;
    }
}
