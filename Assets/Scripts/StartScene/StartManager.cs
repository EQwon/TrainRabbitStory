﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject stageNameGroup;
    public GameObject[] stageName;
    public GameObject previousButton;
    public GameObject nextButton;
    public RectTransform Train;

    private int nowStage;

    private void Start()
    {
        DecideButtonActive();
        for (int i = 1; i < stageName.Length; i++) stageName[i].SetActive(false);
    }

    private void Update()
    {
        GameManager.instance.level = (GameManager.Level)nowStage;
    }

    public void nextStage()
    {
        nowStage += 1;
        StartCoroutine(ChangeStage(-1));
    }

    public void previousStage()
    {
        nowStage -= 1;
        StartCoroutine(ChangeStage(1));
    }

    private void DecideButtonActive()
    {
        previousButton.SetActive(true);
        nextButton.SetActive(true);

        if (nowStage == 0)
        {
            previousButton.SetActive(false);
        }
        if (nowStage == 4)
        {
            nextButton.SetActive(false);
        }
    }

    private void ChangeAD()
    {

    }

    public void StartGame()
    {
        StartCoroutine(MoveTrain());
    }

    private IEnumerator ChangeStage(int dir)
    {
        Vector3 targetPos = stageNameGroup.GetComponent<RectTransform>().localPosition + dir * new Vector3(800f, 0, 0);
        float sp = 1000f;

        previousButton.SetActive(false);
        nextButton.SetActive(false);
        stageName[nowStage].SetActive(true);

        while (Vector3.Distance(stageNameGroup.GetComponent<RectTransform>().localPosition, targetPos) > Mathf.Epsilon)
        {
            stageNameGroup.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(stageNameGroup.GetComponent<RectTransform>().localPosition, targetPos, Time.deltaTime * sp);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.2f);
        DecideButtonActive();
        stageName[nowStage + dir].SetActive(false);
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
        SceneManager.LoadScene(1);
    }
}