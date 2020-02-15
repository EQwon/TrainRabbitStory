using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldMan : PickUpQuest
{
    [SerializeField] private QuestItem handkerchief;

    public override void GetItem()
    {
        QuestManager.instance.GetQuest(QuestName.PickUp).ChangeQuestState(true, true);
    }

    protected override void CreateItem()
    {
        float spawnPosX = (8 - 3) * 20 + Random.Range(-7f, 7f);
        float spawnPosY = Random.Range(-4f, 2f);

        QuestItem item = Instantiate(handkerchief, new Vector3(spawnPosX, spawnPosY, 0), Quaternion.identity);
        item.QuestScript = this;
    }
}
