using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThonosQuest : Quest
{
    [System.Serializable]
    public struct StoneInfo
    {
        public string stoneName;
        public Sprite stoneImage;
    }

    [Header("For Thonos")]
    [SerializeField] private float timeLimit;
    [SerializeField] private QuestItem trainStone;
    [SerializeField] private List<StoneInfo> stoneInfos;
    private List<GameObject> stones = new List<GameObject>();

    private float remainTime;
    private bool questStart;
    private int cnt;

    public float RemainTime { get { return remainTime; } }

    public override void StartQuest()
    {
        base.StartQuest();

        cnt = stoneInfos.Count;
        remainTime = timeLimit;

        CreateStones();

        questStart = true;
    }

    private void FixedUpdate()
    {
        if (!questStart) return;

        if (remainTime > 0)
            remainTime -= Time.fixedDeltaTime;
        else
        {
            remainTime = 0;

            foreach (GameObject stone in stones)
            {
                Destroy(stone);
            }
            EndQuest();
        }
    }

    private void CreateStones()
    {
        for (int i = 0; i < cnt; i++)
        {
            int cellNum = Random.Range(0, 10);
            float spawnPosX = cellNum * 20 + Random.Range(-7f, 7f);
            float spawnPosY = Random.Range(-3f, 1f);

            QuestItem stone = Instantiate(trainStone, new Vector2(spawnPosX, spawnPosY), Quaternion.identity);
            stones.Add(stone.gameObject);
            stone.GetComponent<SpriteRenderer>().sprite = stoneInfos[i].stoneImage;
            stone.name = stoneInfos[i].stoneName;
            stone.GetItem += GetStone;
        }
    }

    private void GetStone(string stoneName)
    {
        score += 1;
        CollectionManager.instance.AcquireCollection(stoneName);

        if (score == cnt) EndQuest();
    }

    protected override void Reward()
    {
        Debug.Log("현재 모은 돌 개수 : " + cnt + "에 따른 보상을 줘야합니다.");

        float rewardHp = (1000f - GameManager.instance.HP) / 2;
        GameManager.instance.HP += (int)rewardHp;
        if (score >= 6)
        {
            CollectionManager.instance.AcquireCollection("토노스의 건틀렛");
        }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ThonosQuest.StoneInfo))]
public class ThonosStoneDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float totalWidth = position.width - 10f;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(new Rect(position.x, position.y, 0.5f * totalWidth, position.height), property.FindPropertyRelative("stoneName"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + 0.5f * totalWidth + 10f, position.y, 0.5f * totalWidth, position.height), property.FindPropertyRelative("stoneImage"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif