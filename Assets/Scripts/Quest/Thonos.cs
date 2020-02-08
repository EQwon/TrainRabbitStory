using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thonos : PickUpQuest
{
    [SerializeField] private QuestItem trainStone;
    [SerializeField] private List<Sprite> stoneImages;
    private List<GameObject> stones = new List<GameObject>();

    [SerializeField] private int cnt;
    [SerializeField] private int collectedCnt = 0;

    [SerializeField] private float limitTime;
    private float remainTime;

    [Header("UI Element")]
    [SerializeField] private Text remainTimeText;

    protected override void Start()
    {
        cnt = stoneImages.Count;
        remainTime = limitTime;

        base.Start();
    }

    private void FixedUpdate()
    {
        if (remainTime > 0)
            remainTime -= Time.fixedDeltaTime;
        else
        {
            remainTime = 0;

            foreach (GameObject stone in stones)
            {
                Destroy(stone);
            }
        }

        remainTimeText.text = remainTime.ToString("0.0") + " 초";
    }

    protected override void CreateItem()
    {
        for (int i = 0; i < cnt; i++)
        {
            int cellNum = Random.Range(0, 10);
            float spawnPosX = cellNum * 20 + Random.Range(-7f, 7f);
            float spawnPosY = Random.Range(-3f, 1f);

            QuestItem stone = Instantiate(trainStone, new Vector2(spawnPosX, spawnPosY), Quaternion.identity);
            stones.Add(stone.gameObject);
            stone.GetComponent<SpriteRenderer>().sprite = stoneImages[i];
            stone.QuestScript = this;
        }
    }

    public override void GetItem()
    {
        collectedCnt += 1;
        QuestManager.instance.UpdateQuest();

        if (collectedCnt == cnt)
        {
            // 퀘스트 성공!
            // 성공 처리하면 됨.
            QuestManager.instance.GetQuest(QuestName.Thonos).ChangeQuestState(true, true);
        }
    }
}
