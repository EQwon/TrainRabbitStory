using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct ScoreCutline
{
    public enum Type { 이상, 미만 }

    public Type type;
    public float score;
    public int successNum;
    public int afterNum;
}

[RequireComponent(typeof(Quest))]
public class QuestDialogue : Dialogue
{
    [SerializeField] private List<ScoreCutline> cutline;

    private Quest myQuest;

    public Quest MyQuest { get { return myQuest; } }

    protected override void Start()
    {
        myQuest = GetComponent<Quest>();

        base.Start();

        QuestManager.instance.AddQuest(myQuest);
    }

    public override List<Dialog> DialogueForNow()
    {
        nowTalkCnt += 1;

        switch (myQuest.QuestState)
        {
            case QuestState.BeforeQuest:
                return dialogues[0];
            case QuestState.DoingQuest:
                return dialogues[1];
            case QuestState.FinishQuest:
                return FinishDialogue();
            case QuestState.AfterQuest:
                return AfterDialogue();
            default:
                return null;
        }
    }

    private List<Dialog> FinishDialogue()
    {
        int score = myQuest.Score;

        for (int i = 0; i < cutline.Count; i++)
        {
            if (score >= cutline[i].score)
                return dialogues[cutline[i].successNum];
        }

        Debug.LogError("점수에 해당하는 대화를 찾지 못했습니다.");
        return null;
    }

    private List<Dialog> AfterDialogue()
    {
        int score = myQuest.Score;

        for (int i = 0; i < cutline.Count; i++)
        {
            if (score >= cutline[i].score)
                return dialogues[cutline[i].afterNum];
        }

        Debug.LogError("점수에 해당하는 대화를 찾지 못했습니다.");
        return null;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScoreCutline))]
public class CutlineDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float totalWidth = position.width - 30f;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(new Rect(position.x, position.y, 0.3f * totalWidth, position.height), property.FindPropertyRelative("score"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + 0.3f * totalWidth, position.y, 0.3f * totalWidth, position.height), property.FindPropertyRelative("type"), GUIContent.none);
        EditorGUI.LabelField(new Rect(position.x + 0.6f * totalWidth, position.y, 30f, position.height), "이면");
        EditorGUI.PropertyField(new Rect(position.x + 0.6f * totalWidth + 30f, position.y, 0.2f * totalWidth, position.height), property.FindPropertyRelative("successNum"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + 0.8f * totalWidth + 30f, position.y, 0.2f * totalWidth, position.height), property.FindPropertyRelative("afterNum"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif