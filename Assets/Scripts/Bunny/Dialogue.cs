using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BunnyName { Gotgam, Jadupudding, Kkingkkang,
                        Mango, Pulttegi, Ssookgat, Tomatotang,
                        Yanggaeng, Yuza, None }

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueAsset;
    /// <summary>
    /// 현재 스테이지가 시작되고 대화한 횟수
    /// </summary>
    [SerializeField] protected int nowTalkCnt;

    protected List<List<List<string>>> dialogues = new List<List<List<string>>>();

    protected virtual void Start()
    {
        dialogues = Parser.DialogParse(dialogueAsset);

        nowTalkCnt = 0;
    }

    public virtual List<List<string>> DialogForNow()
    {
        nowTalkCnt += 1;
        return dialogues[0];
    }
}