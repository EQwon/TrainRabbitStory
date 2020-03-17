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
    public enum Type
    {
        대화, 선택지, 클리어, 화면효과
    }

    public static Dialog ConvertToDialog(List<string> values, ref List<List<Dialog>> dialogues)
    {
        Dialog dialog = null;
        Type type = Type.대화;

        int num = int.Parse(values[0]);
        if (num >= dialogues.Count) dialogues.Add(new List<Dialog>());

        try
        {
            type = (Type)System.Enum.Parse(typeof(Type), values[1]);
        }
        catch
        {
            Debug.LogError(values[1] + "에 해당하는 Type Enum이 존재하지 않습니다. 대화로 대체합니다.");
        }

        switch (type)
        {
            case Type.대화:
                List<string> vs = values;
                vs.RemoveRange(0, 2);
                dialog = TalkDialog.ConvertToTalkDialog(vs);
                break;
            case Type.선택지:
                break;
            case Type.클리어:
                break;
            case Type.화면효과:
                dialog = new ScreenEffectDialog(values[2], values[3]);
                break;
        }

        dialogues[num].Add(dialog);

        return dialog;
    }

    public virtual void Run(UIManager UI)
    { 
    }
}

public class TalkDialog : Dialog
{
    public enum Command
    {
        None, 수락거절, 대답, 대답호감도, 퀘스트종료, 아이템, 호감도
    }

    protected BasicDialog dialog;

    protected TalkDialog()
    {
        dialog = new BasicDialog("EQ1", "버그가 발생했습니다. 뿌쓩빠쓩", Resources.Load<Sprite>("SpeakerImage/DarkBunny_Bust"));
    }

    protected TalkDialog(string speakerName, string speakerText, string speakerImg)
    {
        dialog = new BasicDialog(speakerName, speakerText, Resources.Load<Sprite>("SpeakerImage/" + speakerImg));
    }

    public override void Run(UIManager UI)
    {
        UI.ShowTalkDialog(dialog);
    }

    public static TalkDialog ConvertToTalkDialog(List<string> values)
    {
        TalkDialog talkDialog = new TalkDialog();
        Command command = Command.None;
        //  이름        대사         사진        비고
        //  values[0]   values[1]   values[2]   values[3]
        try
        {
            command = (Command)System.Enum.Parse(typeof(Command), values[3]);
        }
        catch
        {
            if (values.Count >= 4 && values[3] != "")
            {
                Debug.LogError(values[3] + "라는 Command Enum은 존재하지 않습니다.");
            }
        }

        switch (command)
        {
            case Command.None:
                talkDialog = new TalkDialog(values[0], values[1], values[2]);
                break;
            case Command.수락거절:
                talkDialog = new ARDialog(values[0], values[1], values[2]);
                break;
            case Command.대답:
                break;
            case Command.대답호감도:
                break;
            case Command.퀘스트종료:
                talkDialog = new QuestFinishDialog(values[0], values[1], values[2]);
                break;
            case Command.아이템:
                break;
            case Command.호감도:
                break;
        }

        return talkDialog;
    }
}

public class ARDialog : TalkDialog
{
    public ARDialog(string name, string text, string img) : base(name, text, img) { }

    public override void Run(UIManager UI)
    {
        base.Run(UI);
        UI.ShowARButton();
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

    public override void Run(UIManager UI)
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

    public override void Run(UIManager UI)
    {
        UI.ShowReactionDialog(dialogs);
    }
}

public class ReactionAffDialog : Dialog
{ }

public class QuestFinishDialog : TalkDialog
{
    public QuestFinishDialog(string name, string text, string img) : base(name, text, img) { }

    public override void Run(UIManager UI)
    {
        base.Run(UI);
        UI.QuestFinish();
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

    public override void Run(UIManager UI)
    {
        UI.ShowAffinityDialog(dialog, affinity);
    }
}

public class ClearDialog : Dialog
{
    public ClearDialog()
    { }

    public override void Run(UIManager UI)
    {
        UI.ShowClearDialog();
    }
}

public class ScreenEffectDialog : Dialog
{
    Color targetColor;
    float duration;

    public ScreenEffectDialog(string color, string duration)
    {
        string[] values = color.Split(',');
        targetColor = new Color(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        this.duration = float.Parse(duration);
    }

    public override void Run(UIManager UI)
    {
        UI.ShowScreenEffect(targetColor, duration);
    }
}