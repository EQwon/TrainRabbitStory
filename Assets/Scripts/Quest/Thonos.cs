using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thonos : PickUpQuest
{
    [SerializeField] private QuestItem trainStone;
    [SerializeField] private int cnt;

    private int collectedCnt = 0;

    protected override void CreateItem()
    {
        for (int i = 0; i < cnt; i++)
        {
            float spawnPosX = Random.Range(-7f, 7f);
            float spawnPosY = Random.Range(-4f, 2f);

            QuestItem stone = Instantiate(trainStone, new Vector2(spawnPosX, spawnPosY), Quaternion.identity);
            stone.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            stone.QuestScript = this;
        }
    }

    public override void GetItem()
    {
        collectedCnt += 1;
        if (collectedCnt == cnt)
        {
            // 퀘스트 성공!
            // 성공 처리하면 됨.
            QuestManager.instance.GetQuest(QuestName.Thonos).ChangeQuestState(true, true);
        }
    }
}
