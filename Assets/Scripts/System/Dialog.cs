using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BasicDialog
{
    public string speakerName;
    public string speakerText;
    public Sprite speakerImg;

    public BasicDialog(string speakerName, string speakerText, Sprite speakerImg)
    {
        this.speakerName = speakerName;
        this.speakerText = speakerText;
        this.speakerImg = speakerImg;
    }
}

public class Dialog
{
    public enum Command
    {
        수락거절, 선택지, 대답, 대답호감도, 퀘스트종료, 아이템,
        호감도, 클리어, 화면효과
    }

    public static Dialog ConvertToDialog(List<string> line, ref List<List<Dialog>> dialogue)
    {
        Dialog dialog = null;

        int num = int.Parse(line[0]);
        if (num >= dialogue.Count) dialogue.Add(new List<Dialog>());

        try
        {
            Command command = (Command)System.Enum.Parse(typeof(Command), line[4]);

            switch (command)
            {
                case Command.수락거절:
                    dialog = new ARDialog(line[1], line[2], line[3]);
                    break;
                case Command.선택지:
                    List<string> choiceTexts = new List<string>(line);
                    choiceTexts.RemoveRange(0, 5);
                    dialog = new ChoiceDialog(line[1], line[2], line[3], choiceTexts);
                    break;
                case Command.대답:
                    break;
                case Command.대답호감도:
                    break;
                case Command.퀘스트종료:
                    dialog = new QuestFinishDialog(line[1], line[2], line[3]);
                    break;
                case Command.아이템:
                    break;
                case Command.호감도:
                    break;
                case Command.클리어:
                    break;
                case Command.화면효과:
                    dialog = new ScreenEffectDialog(line[1], line[2], line[3], line[5], line[6]);
                    break;
            }
        }
        catch
        {
            if (line.Count >= 5)
            {
                if (line[4] != "") Debug.LogError(line[4] + "라는 Command Enum은 존재하지 않습니다.");
            }

            dialog = new TalkDialog(line[1], line[2], line[3]);
        }

        dialogue[num].Add(dialog);

        return dialog;
    }

    public virtual void Show(UIManager UI)
    { 
    }
}

public class TalkDialog : Dialog
{
    private BasicDialog dialog;

    public TalkDialog(string speakerName, string speakerText, string speakerImg)
    {
        dialog = new BasicDialog(speakerName, speakerText, Resources.Load<Sprite>("SpeakerImage/" + speakerImg));
    }

    public override void Show(UIManager UI)
    {
        UI.ShowTalkDialog(dialog);
    }
}

public class ARDialog : Dialog
{
    private BasicDialog dialog;

    public ARDialog(string speakerName, string speakerText, string speakerImg)
    {
        dialog = new BasicDialog(speakerName, speakerText, Resources.Load<Sprite>("SpeakerImage/" + speakerImg));
    }

    public override void Show(UIManager UI)
    {
        UI.ShowARDialog(dialog);
    }
}

public class ChoiceDialog : Dialog
{
    private BasicDialog dialog;
    private List<string> choiceTexts = new List<string>();

    public ChoiceDialog(string speakerName, string speakerText, string speakerImg, List<string> choiceTexts)
    {
        dialog = new BasicDialog(speakerName, speakerText, Resources.Load<Sprite>("SpeakerImage/" + speakerImg));
        this.choiceTexts = choiceTexts;
    }

    public override void Show(UIManager UI)
    {
        UI.ShowChoiceDialog(dialog, choiceTexts);
    }
}

public class ReactionDialog : Dialog
{
    List<BasicDialog> dialogs = new List<BasicDialog>();

    public ReactionDialog(List<string> line)
    {
        string speaker = line[1];
        Sprite img = Resources.Load<Sprite>("SpeakerImage/" + line[3]);
        for (int i = 4; i < line.Count; i++)
        {
            BasicDialog dialog = new BasicDialog(speaker, line[i], img);
            dialogs.Add(dialog);
        }
    }

    public override void Show(UIManager UI)
    {
        UI.ShowReactionDialog(dialogs);
    }
}

public class ReactionAffDialog : Dialog
{ }

public class QuestFinishDialog : Dialog
{
    BasicDialog dialog;

    public QuestFinishDialog(string speakerName, string speakerText, string speakerImg)
    {
        dialog = new BasicDialog(speakerName, speakerText, Resources.Load<Sprite>("SpeakerImage/" + speakerImg));
    }

    public override void Show(UIManager UI)
    {
        UI.ShowQuestFinishDialog(dialog);
    }
}

public class ItemDialog : Dialog
{ }

public class AffinityDialog : Dialog
{
    BasicDialog dialog;
    int affinity;

    public AffinityDialog(string speakerName, string speakerText, string speakerImg, string affinityAmount)
    {
        dialog = new BasicDialog(speakerName, speakerText, Resources.Load<Sprite>("SpeakerImage/" + speakerImg));
        affinity = int.Parse(affinityAmount);
    }

    public override void Show(UIManager UI)
    {
        UI.ShowAffinityDialog(dialog, affinity);
    }
}

public class ClearDialog : Dialog
{
    public ClearDialog()
    { }

    public override void Show(UIManager UI)
    {
        UI.ShowClearDialog();
    }
}

public class ScreenEffectDialog : Dialog
{
    BasicDialog dialog;
    Color targetColor;
    float duration;

    public ScreenEffectDialog(string speakerName, string speakerText, string speakerImg
        , string color, string duration)
    {
        dialog = new BasicDialog(speakerName, speakerText, Resources.Load<Sprite>("SpeakerImage/" + speakerImg));

        string[] values = color.Split(',');
        targetColor = new Color(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        this.duration = float.Parse(duration);
    }

    public override void Show(UIManager UI)
    {
        UI.ShowScreenEffectDialog(dialog, targetColor, duration);
    }
}