using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TelalaBlock : MonoBehaviour
{
    public enum Direction { None, Right, Left, Up, Down }

    public float moveDuration;
    public Direction movableDir = Direction.None;

    public int answerRow;
    public int answerColumn;
    private float moveWidth;
    private float moveHeight;

    public void Initiailize(TelalaQuestPanel.Cord answerCord
                          , float moveWidth, float moveHeight)
    {
        answerRow = answerCord.row;
        answerColumn = answerCord.column;
        this.moveWidth = moveWidth;
        this.moveHeight = moveHeight;
    }

    public bool Move(Direction dir)
    {
        if (movableDir != dir) return false;

        switch (dir)
        {
            case Direction.Right:
                MoveX(true);
                break;
            case Direction.Left:
                MoveX(false);
                break;
            case Direction.Up:
                MoveY(true);
                break;
            case Direction.Down:
                MoveY(false);
                break;
        }

        return true;
    }

    private void MoveX(bool goRight)
    {
        RectTransform rect = GetComponent<RectTransform>();
        float nowX = rect.anchoredPosition.x;
        float deltaX = goRight ? moveWidth : -moveWidth;

        rect.DOAnchorPosX(nowX + deltaX, moveDuration).SetEase(Ease.OutBack);
    }

    private void MoveY(bool goUp)
    {
        RectTransform rect = GetComponent<RectTransform>();
        float nowY = rect.anchoredPosition.y;
        float deltaY = goUp ? moveHeight : -moveHeight;

        rect.DOAnchorPosY(nowY + deltaY, moveDuration).SetEase(Ease.OutBack);
    }
}
