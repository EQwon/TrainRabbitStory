using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartManager : MonoBehaviour
{
    [Header("Holder")]
    public RectTransform train;
    public GameObject stageName;

    [Header("Resources")]
    public Sprite[] stageNameImage;

    private int nowStage;

    private void Start()
    {
        nowStage = GameManager.instance.Stage;
        stageName.GetComponent<Image>().sprite = stageNameImage[nowStage];
    }

    public void Reset()
    {
        GameManager.instance.Stage = 0;
        nowStage = 0;
        stageName.GetComponent<Image>().sprite = stageNameImage[nowStage];
    }

    public void StartGame()
    {
        stageName.GetComponent<Button>().enabled = false;

        float y = train.anchoredPosition.y;
        Vector2 rectPos = new Vector2(0, y);
        Vector2 endPos = new Vector2(-2000, y);

        Sequence trainSequence = DOTween.Sequence();
        trainSequence.Append(DOTween.To(() => train.anchoredPosition, x => train.anchoredPosition = x, rectPos, 2f).SetEase(Ease.OutQuart));
        trainSequence.AppendInterval(1f);
        trainSequence.Append(DOTween.To(() => train.anchoredPosition, x => train.anchoredPosition = x, endPos, 2f).SetEase(Ease.InQuart));
        trainSequence.AppendCallback(() => LoadGameScene());
    }

    private void LoadGameScene()
    {
        GameManager.instance.LoadData();
        SceneManager.LoadScene(nowStage + 1);
    }
}