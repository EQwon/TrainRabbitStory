using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject gameName;
    public GameObject startGameButton;
    public GameObject stageSelectButton;
    public GameObject collectionButton;
    public RectTransform train;
    public GameObject[] stageCharacters;

    private Vector3 trainTarget = new Vector3(50, -240, 0);
    private float trainSpeed = 1500f;

    private void Start()
    {
        stageCharacters[0].SetActive(true);
        stageCharacters[1].SetActive(false);
        stageCharacters[2].SetActive(false);
        stageCharacters[3].SetActive(false);
        stageCharacters[4].SetActive(false);
    }

    private void UIFadeOut()
    {
        startGameButton.GetComponent<Button>().interactable = false;
        stageSelectButton.GetComponent<Button>().interactable = false;
        collectionButton.GetComponent<Button>().interactable = false;

        gameName.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        startGameButton.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        stageSelectButton.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        collectionButton.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
    }

    public void StartGame()
    {
        UIFadeOut();
        StartCoroutine(MoveTrain());
    }

    private void SceneChange()
    {
        Debug.Log("게임을 새로 시작합니다.");
        SceneManager.LoadScene(1);
    }

    public void SelectStage(string levelName)
    {
        stageCharacters[0].SetActive(false);
        stageCharacters[1].SetActive(false);
        stageCharacters[2].SetActive(false);
        stageCharacters[3].SetActive(false);
        stageCharacters[4].SetActive(false);

        switch (levelName)
        {
            case "kinder":
                GameManager.instance.level = GameManager.Level.kinder;
                stageCharacters[0].SetActive(true);
                break;
            case "elementary":
                GameManager.instance.level = GameManager.Level.elementary;
                stageCharacters[1].SetActive(true);
                break;
            case "middle":
                GameManager.instance.level = GameManager.Level.middle;
                stageCharacters[2].SetActive(true);
                break;
            case "high":
                GameManager.instance.level = GameManager.Level.high;
                stageCharacters[3].SetActive(true);
                break;
            case "university":
                GameManager.instance.level = GameManager.Level.university;
                stageCharacters[4].SetActive(true);
                break;
            default:
                Debug.Log("오류! 오류!");
                break;
        }
    }

    private IEnumerator MoveTrain()
    {
        float n = 0;
        while (Vector3.Distance(train.localPosition, trainTarget) > Mathf.Epsilon)
        {
            train.localPosition = Vector3.MoveTowards(train.localPosition, trainTarget, Time.deltaTime * (trainSpeed - n));
            n += 9.5f;
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(1.5f);

        SceneChange();
    }
}