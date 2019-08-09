using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public int CellNum;
    public List<GameObject> AppearStandingBunnyList;
    public List<GameObject> AppearSittingBunnyList;

    private List<Vector2> sitPositions = new List<Vector2>();

    private void Start()
    {
        SetupTrain(GameManager.instance.level);
    }

    private void InitializeList()
    {
        sitPositions.Clear();  

        sitPositions.Add(new Vector2(-8.5f, 3.5f));
        sitPositions.Add(new Vector2(-7.5f, 3.5f));
        sitPositions.Add(new Vector2(-6.5f, 3.5f));
        sitPositions.Add(new Vector2(-2.8f, 3.5f));
        sitPositions.Add(new Vector2(-1.75f, 3.5f));
        sitPositions.Add(new Vector2(-0.65f, 3.5f));
        sitPositions.Add(new Vector2(0.45f, 3.5f));
        sitPositions.Add(new Vector2(1.5f, 3.5f));
        sitPositions.Add(new Vector2(2.6f, 3.5f));
        sitPositions.Add(new Vector2(6.2f, 3.5f));
        sitPositions.Add(new Vector2(7.3f, 3.5f));
        sitPositions.Add(new Vector2(8.4f, 3.5f));
    }

    public void SetupTrain(GameManager.Level level)
    {
        InitializeList();
        LayoutBunny(level);
    }

    private void LayoutBunny(GameManager.Level level)   //직접 토끼들을 레벨에 따라 배치시키는 함수
    {
        CollocateSittingBunny(AppearSittingBunnyList);
        CollocteStandingBunny(AppearStandingBunnyList);
    }

    private void CollocateSittingBunny(List<GameObject> bunnyList)  //앉아있는 일반 토끼들 배치하는 함수
    {
        for (int i = 0; i < bunnyList.Count; i++)
        {
            if (bunnyList[i] == null) continue;

            GameObject bunny = Instantiate(bunnyList[i], gameObject.transform);
            bunny.transform.localPosition = sitPositions[i];
            bunny.GetComponent<SpriteRenderer>().sortingOrder = (int)(sitPositions[i].x * 100);
        }
    }

    private void CollocteStandingBunny(List<GameObject> bunnyList)  //서 있는 일반 토끼들 배치하는 함수
    {
        for (int i = 0; i < bunnyList.Count; i++)
        {
            if (bunnyList[i] == null) continue;

            float xPos, yPos;
            xPos = Random.Range(-7f, 7f);
            yPos = Random.Range(-2f, 2f);

            Vector2 randomPos = new Vector2(xPos, yPos);

            GameObject bunny = Instantiate(bunnyList[i], gameObject.transform);
            bunny.transform.localPosition = randomPos;
        }
    }
}
