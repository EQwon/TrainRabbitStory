using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TelalaBlockMover : MonoBehaviour
{
    public int myRow;
    public int myColumn;
    public float duration;

    private int answerRow;
    private int answerColumn;
    private float blockWidth;
    private float blockHeight;

    public bool isCorrectPos
    {
        get
        {
            if (myRow == answerRow && myColumn == answerColumn) return true;
            else return false;
        }
    }

    public void Initiailize(TelalaQuestPanel.Cord myCord
                          , TelalaQuestPanel.Cord answerCord
                          , float blockWidth, float blockHeight)
    {
        myRow = myCord.row;
        myColumn = myCord.column;
        answerRow = answerCord.row;
        answerColumn = answerCord.column;
        this.blockWidth = blockWidth;
        this.blockHeight = blockHeight;
    }

    public void MoveX(bool goRight)
    {
        RectTransform rect = GetComponent<RectTransform>();
        float nowX = rect.anchoredPosition.x;
        float deltaX = goRight ? blockWidth : -blockWidth;
        myRow += goRight ? 1 : -1;

        rect.DOAnchorPosX(nowX + deltaX, duration).SetEase(Ease.OutBack);
    }

    public void MoveY(bool goUp)
    {
        RectTransform rect = GetComponent<RectTransform>();
        float nowY = rect.anchoredPosition.y;
        float deltaY = goUp ? blockHeight : -blockHeight;
        myColumn += goUp ? 1 : -1;

        rect.DOAnchorPosY(nowY + deltaY, duration).SetEase(Ease.OutBack);
    }
}
