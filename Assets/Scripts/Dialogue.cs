using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogType { BeforeQuest, AfterAccept, AfterRefuse, WhileQuest, SuccessQuest, AfterSuccess, Default1, Default2, Default3 };

[System.Serializable]
public class Dialog
{
    public DialogType type;
    public string Speaker;
    public string Text;
}

public class Dialogue : MonoBehaviour
{
    public List<Dialog> dialogue;
    public Quest quest;
    public List<int> reward;

    private int defaultDialogCnt;

    private void Start()
    {
        defaultDialogCnt = 6;
    }

    public List<Dialog> dialogueForNow()
    {
        if (quest == Quest.None) return DefaultDialog();

        bool isAccept = GameManager.instance.gameObject.GetComponent<QuestManager>().isAccept[(int)quest];
        bool isSuccess = GameManager.instance.gameObject.GetComponent<QuestManager>().isSuccess[(int)quest];

        if (isAccept == false)
        {
            if (isSuccess == false) return BeforeQuestDialog();
            else return AfterSuccessDialog();
        }
        else
        {
            if (isSuccess == false) return WhileQuestDialog();
            else return SuccessQuestDialog();
        }
    }

    private List<Dialog> DefaultDialog()
    {
        List<Dialog> dialog = new List<Dialog>();
        bool isThereNextDefaultDialog = false;

        for (int i = 0; i < dialogue.Count; i++)
        {
            if (dialogue[i].type == (DialogType)defaultDialogCnt)
            {
                dialog.Add(dialogue[i]);
            }
            if (dialogue[i].type == (DialogType)(defaultDialogCnt + 1))
            {
                isThereNextDefaultDialog = true;
            }
        }

        if (isThereNextDefaultDialog == true) defaultDialogCnt += 1;
        else defaultDialogCnt = 6;

        return dialog;
    }

    private List<Dialog> BeforeQuestDialog()
    {
        List<Dialog> dialog = new List<Dialog>();

        for (int i = 0; i < dialogue.Count; i++)
        {
            if (dialogue[i].type == DialogType.BeforeQuest)
            {
                dialog.Add(dialogue[i]);
            }
        }

        return dialog;
    }

    public List<Dialog> AfterAcceptDialog()
    {
        List<Dialog> dialog = new List<Dialog>();

        for (int i = 0; i < dialogue.Count; i++)
        {
            if (dialogue[i].type == DialogType.AfterAccept)
            {
                dialog.Add(dialogue[i]);
            }
        }

        return dialog;
    }

    public List<Dialog> AfterRefuseDialog()
    {
        List<Dialog> dialog = new List<Dialog>();

        for (int i = 0; i < dialogue.Count; i++)
        {
            if (dialogue[i].type == DialogType.AfterRefuse)
            {
                dialog.Add(dialogue[i]);
            }
        }

        return dialog;
    }

    private List<Dialog> WhileQuestDialog()
    {
        List<Dialog> dialog = new List<Dialog>();

        for (int i = 0; i < dialogue.Count; i++)
        {
            if (dialogue[i].type == DialogType.WhileQuest)
            {
                dialog.Add(dialogue[i]);
            }
        }

        return dialog;
    }

    private List<Dialog> SuccessQuestDialog()
    {
        List<Dialog> dialog = new List<Dialog>();

        for (int i = 0; i < dialogue.Count; i++)
        {
            if (dialogue[i].type == DialogType.SuccessQuest)
            {
                dialog.Add(dialogue[i]);
            }
        }

        if (reward.Count == 2)
        {
            Dialog rewardDialog = new Dialog();
            rewardDialog.type = DialogType.SuccessQuest;
            rewardDialog.Speaker = "";
            rewardDialog.Text = "체력 " + reward[0] + ", 토성 " + reward[1] + "이 회복되었다.";

            dialog.Add(rewardDialog);
        }

        GameManager.instance.gameObject.GetComponent<QuestManager>().isAccept[(int)quest] = false;

        return dialog;
    }

    private List<Dialog> AfterSuccessDialog()
    {
        List<Dialog> dialog = new List<Dialog>();

        for (int i = 0; i < dialogue.Count; i++)
        {
            if (dialogue[i].type == DialogType.AfterSuccess)
            {
                dialog.Add(dialogue[i]);
            }
        }

        return dialog;
    }

    public void GiveReward(List<Dialog> dialogs)
    {
        if (reward.Count != 2) return;

        if (dialogs[0].type == DialogType.SuccessQuest)
        {
            GameManager.instance.HP += reward[0];
            GameManager.instance.MP += reward[1];
        }
    }
}