using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [Header("Holder")]
    public RectTransform Train;
    public GameObject stageName;

    [Header("Resources")]
    public Sprite[] stageNameImage;

    private int nowStage;

    private void Start()
    {
        nowStage = GetStageNum();
        stageName.GetComponent<Image>().sprite = stageNameImage[nowStage];
    }

    private int GetStageNum()
    {
        return GameManager.instance.Stage;
    }

    private void ChangeAD()
    {

    }

    public void StartGame()
    {
        stageName.GetComponent<Button>().enabled = false;
        StartCoroutine(MoveTrain());
    }

    private IEnumerator MoveTrain()
    {
        Vector3 targetPos = new Vector3(0, Train.localPosition.y, Train.localPosition.z);
        float speed = 1000f;

        while (Vector3.Distance(Train.localPosition, targetPos) > Mathf.Epsilon)
        {
            Train.localPosition = Vector3.MoveTowards(Train.localPosition, targetPos, Time.deltaTime * speed);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        targetPos -= new Vector3(2000, 0, 0);

        while(Vector3.Distance(Train.localPosition, targetPos) > Mathf.Epsilon)
        {
            Train.localPosition = Vector3.MoveTowards(Train.localPosition, targetPos, Time.deltaTime * speed);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        //Debug.Log("게임을 새로 시작합니다.");
        SceneManager.LoadScene(nowStage + 1);
    }
}