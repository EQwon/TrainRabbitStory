using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialBunny
{
    public int apearCellNum;
    public GameObject bunnyPrefab;
}

public class TrainController : MonoBehaviour
{
    public List<GameObject> playerBunny;

    public List<GameObject> normalSittingBunny;
    public List<SpecialBunny> kinderStageSittingBunny;
    public List<SpecialBunny> elementaryStageSittingBunny;
    public List<SpecialBunny> middleStageSittingBunny;
    public List<SpecialBunny> highStageSittingBunny;
    public List<SpecialBunny> universityStageSittingBunny;

    public List<GameObject> normalStandingBunny;
    public List<SpecialBunny> kinderStageStandingBunny;
    public List<SpecialBunny> elementaryStageStandingBunny;
    public List<SpecialBunny> middleStageStandingBunny;
    public List<SpecialBunny> highStageStandingBunny;
    public List<SpecialBunny> universityStageStandingBunny;

    public int CellNum = 1;

    private List<GameObject> sittingBunnyList = new List<GameObject>();
    private List<GameObject> standingBunnyList = new List<GameObject>();
    private List<Vector2> sitPositions = new List<Vector2>();

    private void InitializeList()
    {
        DestroyBunny();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);

        sitPositions.Clear();
        sittingBunnyList.Clear();
        standingBunnyList.Clear();    

        sitPositions.Add(new Vector2(-8.5f, 3.5f));
        sitPositions.Add(new Vector2(-7.5f, 3.5f));
        sitPositions.Add(new Vector2(-6.5f, 3.5f));
        sitPositions.Add(new Vector2(-2.8f, 3.5f));
        sitPositions.Add(new Vector2(-1.75f, 3.5f));
        sitPositions.Add(new Vector2(-0.7f, 3.5f));
        sitPositions.Add(new Vector2(0.35f, 3.5f));
        sitPositions.Add(new Vector2(1.4f, 3.5f));
        sitPositions.Add(new Vector2(2.5f, 3.5f));
        sitPositions.Add(new Vector2(6.2f, 3.5f));
        sitPositions.Add(new Vector2(7.2f, 3.5f));
        sitPositions.Add(new Vector2(8.2f, 3.5f));
    }

    private void DestroyBunny() //다음 스테이지를 위해 모든 토끼 제거
    {
        GameObject[] bunnyList = GameObject.FindGameObjectsWithTag("Bunny");

        for(int i = 0; i < bunnyList.Length; i++)
        {
            Destroy(bunnyList[i]);
        }
    }

    private void CollocateSittingBunny(List<GameObject> bunnyList)  //앉아있는 일반 토끼들 배치하는 함수
    {
        GameObject train = GameObject.Find("Train");

        for (int i = 0; i < sitPositions.Count; i++)
        {
            int randomIndex = Random.Range(0, bunnyList.Count);

            GameObject bunny = Instantiate(bunnyList[randomIndex], sitPositions[i], Quaternion.identity);
            bunny.transform.SetParent(train.transform);
            sittingBunnyList.Add(bunny);
            bunny.GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    private void CollocteStandingBunny(List<GameObject> bunnyList)  //서 있는 일반 토끼들 배치하는 함수
    {
        int standingBunnyNum = 10;   //생성하는 숫자는 나중에 바꾸도록 하자!
        GameObject train = GameObject.Find("Train");

        for (int i = 0; i < standingBunnyNum; i++)
        {
            float xPos, yPos;
            xPos = Random.Range(-8f, 8f);
            yPos = Random.Range(-2f, 2f);

            Vector2 randomPos = new Vector2(xPos, yPos);
            int randomIndex = Random.Range(0, bunnyList.Count);

            GameObject bunny = Instantiate(bunnyList[randomIndex], randomPos, Quaternion.identity);
            bunny.transform.SetParent(train.transform);
            standingBunnyList.Add(bunny);
        }
    }

    public void SetupTrain(GameManager.Level level)
    {
        InitializeList();
        LayoutBunny(level);
        CreatePlayer(level);
    }

    private void LayoutBunny(GameManager.Level level)   //직접 토끼들을 레벨에 따라 배치시키는 함수
    {
        switch (level)
        {
            case GameManager.Level.kinder:
                CollocateSpecialStandingBunny(kinderStageStandingBunny);
                CollocateSpecialSittingBunny(kinderStageSittingBunny);
                break;
            case GameManager.Level.elementary:
                CollocateSpecialStandingBunny(elementaryStageStandingBunny);
                CollocateSpecialSittingBunny(elementaryStageSittingBunny);
                break;
            case GameManager.Level.middle:
                CollocateSpecialStandingBunny(middleStageStandingBunny);
                CollocateSpecialSittingBunny(middleStageSittingBunny);
                break;
            case GameManager.Level.high:
                CollocateSpecialStandingBunny(highStageStandingBunny);
                CollocateSpecialSittingBunny(highStageSittingBunny);
                break;
            case GameManager.Level.university:
                CollocateSpecialStandingBunny(universityStageStandingBunny);
                CollocateSpecialSittingBunny(universityStageSittingBunny);
                break;
            default:
                Debug.Log("Something is Wrong at Layount Bunny!!");
                break;
        }
        CollocateSittingBunny(normalSittingBunny);
        CollocteStandingBunny(normalStandingBunny);
    }

    private void CollocateSpecialStandingBunny(List<SpecialBunny> bunnyList)    //특수한 서 있는 토끼들을 배치하는 함수
    {
        GameObject train = GameObject.Find("Train");

        for (int i = 0; i < bunnyList.Count; i++)
        {
            if(CellNum == bunnyList[i].apearCellNum)
            {
                float xPos, yPos;
                xPos = Random.Range(-8f, 8f);
                yPos = Random.Range(-2f, 2f);

                Vector2 randomPos = new Vector2(xPos, yPos);

                GameObject bunny = Instantiate(bunnyList[i].bunnyPrefab, randomPos, Quaternion.identity);
                bunny.transform.SetParent(train.transform);
                standingBunnyList.Add(bunny);

                //Debug.Log("현재 스테이지는" + stageNum + "이여서" + bunny.name + "을 만들었다.");
            }
        }
    }

    private void CollocateSpecialSittingBunny(List<SpecialBunny> bunnyList) //특수한 앉아있는 토끼들 배치하는 함수
    {
        GameObject train = GameObject.Find("Train");

        for (int i = 0; i < bunnyList.Count; i++)
        {
            if(CellNum == bunnyList[i].apearCellNum)
            {
                int randomPos = Random.Range(0, sitPositions.Count);

                GameObject bunny = Instantiate(bunnyList[i].bunnyPrefab, sitPositions[randomPos], Quaternion.identity);
                bunny.transform.SetParent(train.transform);
                sittingBunnyList.Add(bunny);
                bunny.GetComponent<SpriteRenderer>().sortingOrder = i;
                sitPositions.RemoveAt(randomPos);
            }
        }
    }

    private List<GameObject> SittingBunnyList(List<GameObject> sittingBunnyList)    //이거 잘못만든 것 같은데 일단 내비 둠
    {
        List<GameObject> bunnyList = normalSittingBunny;

        for (int i = 0; i < sittingBunnyList.Count; i++)
        {
            bunnyList.Add(sittingBunnyList[i]);
        }

        return bunnyList;
    }

    private List<GameObject> RefreshStandingBunnyList(List<GameObject> standingBunntList)   //이거 잘못만든 것 같은데 일단 내비 둠
    {
        List<GameObject> bunnyList = normalStandingBunny;

        for (int i = 0; i < standingBunntList.Count; i++)
        {
            bunnyList.Add(standingBunntList[i]);
        }

        return bunnyList;
    }

    private void CreatePlayer(GameManager.Level level)  //단계에 맞게 플레이어 생성
    {
        Vector2 spawnPosition = new Vector2(-8f, 0f);
        switch(level)
        {
            case GameManager.Level.kinder:
                Instantiate(playerBunny[0], spawnPosition, Quaternion.identity);
                break;
            case GameManager.Level.elementary:
                Instantiate(playerBunny[1], spawnPosition, Quaternion.identity);
                break;
            case GameManager.Level.middle:
                Instantiate(playerBunny[2], spawnPosition, Quaternion.identity);
                break;
            case GameManager.Level.high:
                Instantiate(playerBunny[3], spawnPosition, Quaternion.identity);
                break;
            case GameManager.Level.university:
                Instantiate(playerBunny[4], spawnPosition, Quaternion.identity);
                break;
            default:
                Debug.Log("Something is Wrong at Create Player!!");
                break;
        }
    }
}
