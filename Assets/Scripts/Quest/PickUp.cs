using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public Image handkerchiefIcon;
    public GameObject handkerchief;

    private void Start()
    {
        GameManager.instance.ChangeTrainState(GameManager.TrainState.normalQuest);

        float spawnPosX = (10 - 3) * 20 + Random.Range(-7f, 7f);
        float spawnPosY = Random.Range(-4f, 2f);

        Instantiate(handkerchief, new Vector3(spawnPosX, spawnPosY, 0), Quaternion.identity);
    }

    private void Update()
    {
        if (QuestManager.instance.isSuccess[(int)Quest.PickUp] == true && handkerchiefIcon.color.a != 1)
        {
            handkerchiefIcon.color = new Color(1, 1, 1, 1);
        }

        if (QuestManager.instance.isSuccess[(int)Quest.PickUp] == true && QuestManager.instance.isAccept[(int)Quest.PickUp] == false)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
