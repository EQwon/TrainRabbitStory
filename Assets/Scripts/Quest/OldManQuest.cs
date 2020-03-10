using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldManQuest : Quest
{
    [SerializeField] private QuestItem handkerchief;

    public override void StartQuest()
    {
        base.StartQuest();

        CreateHandkerchief();
    }

    private void CreateHandkerchief()
    {
        float spawnPosX = (8 - 3) * 20 + Random.Range(-7f, 7f);
        float spawnPosY = Random.Range(-4f, 2f);

        QuestItem item = Instantiate(handkerchief, new Vector3(spawnPosX, spawnPosY, 0), Quaternion.identity);
        item.GetItem += GetHandkerchief;
    }

    private void GetHandkerchief(string itemName)
    {
        QuestManager.instance.QuestFinish(questName);
    }

    protected override void Reward()
    {
        Debug.Log("할아버지 퀘스트를 수행했으니 보상을 줘야합니다.");
    }
}
