using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThonosQuest : Quest
{
    [Header("For Thonos")]
    [SerializeField] private QuestItem trainStone;
    [SerializeField] private List<Sprite> stoneImages;
    private List<GameObject> stones = new List<GameObject>();

    [SerializeField] private float timeLimit;
    private float remainTime;
    private bool questStart;
    private int cnt;

    public float RemainTime { get { return remainTime; } }

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
            stone.GetComponent<SpriteRenderer>().sprite = stoneImages[i];
            stone.GetItem += GetStone;
        }
    }

    private void GetStone()
    {
        score += 1;

        if (score == cnt) EndQuest();
    }

    protected override void Reward()
    {
        Debug.Log("현재 모은 돌 개수 : " + cnt + "에 따른 보상을 줘야합니다.");
    }
}
