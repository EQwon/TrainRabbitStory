using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThonosQuest : Quest
{
    [Header("For Thonos")]
    [SerializeField] private QuestItem trainStone;
    [SerializeField] private List<Sprite> stoneImages;
    private List<GameObject> stones = new List<GameObject>();

    [SerializeField] private int cnt;
    [SerializeField] private int collectedCnt = 0;

    [SerializeField] private float timeLimit;
    private float remainTime;
    private bool questStart;

    [Header("UI Element")]
    [SerializeField] private Text remainTimeText;

    public override void StartQuest()
    {
        base.StartQuest();

        cnt = stoneImages.Count;
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
            QuestManager.instance.QuestFinish(QuestName.Thonos);
        }

        remainTimeText.text = remainTime.ToString("0.0") + " 초";
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
            stone.GetComponent<SpriteRenderer>().sprite = stoneImages[i];
            stone.GetItem += GetStone;
        }
    }

    private void GetStone()
    {
        collectedCnt += 1;
        
        if (collectedCnt == cnt)
        {
            // 퀘스트 성공!
            // 성공 처리하면 됨.
            QuestManager.instance.QuestFinish(questName);
        }

        QuestManager.instance.UpdateQuest();
    }

    protected override void Reward()
    {
        Debug.Log("현재 모은 돌 개수 : " + cnt + "에 따른 보상을 줘야합니다.");
    }
}
