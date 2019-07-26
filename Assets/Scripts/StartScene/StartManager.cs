using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [Header("Holder")]
    public RectTransform Train;
    public Image stageName;

    [Header("Resources")]
    public Sprite[] stageNameImage;

    private int nowStage;

    private void Start()
    {
        nowStage = GetStageNum();
        stageName.sprite = stageNameImage[nowStage];
    }

    private void Update()
    {
        GameManager.instance.level = (GameManager.Level)nowStage;
    }

    private int GetStageNum()
    {
        return 0;
    }

    private void ChangeAD()
    {

    }

    public void StartGame()
    {
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

        Debug.Log("게임을 새로 시작합니다.");
        SceneManager.LoadScene(nowStage + 1);
    }
}