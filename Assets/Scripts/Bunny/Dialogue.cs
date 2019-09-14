using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Dialogue : MonoBehaviour
{
    public TextAsset dialogueAsset;
    public Quest quest;

    private List<List<List<string>>> dialogues = new List<List<List<string>>>();

    private void Start()
    {
        if (dialogueAsset == null) return;
        dialogues = Parser.DialogParse(dialogueAsset);
    }

    public List<List<string>> DialogForNow()
    {
        List<List<string>> dialog = new List<List<string>>();
        int dialogCnt = 0;

        if (GetComponent<Affinity>())
        {
            dialogCnt = GameManager.instance.TalkCnt[GetComponent<Affinity>().bunnyNum];
        }
        else
        {
            dialogCnt = QuestManager.instance.State(quest);
        }

        if (dialogues.Count > dialogCnt)
        {
            dialog = new List<List<string>>(dialogues[dialogCnt]);
        }

        return dialog;
    }
}