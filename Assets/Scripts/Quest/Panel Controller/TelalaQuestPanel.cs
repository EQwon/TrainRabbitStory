using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelalaQuestPanel : QuestPanel
{
    private enum State { None, Start, Matching, Correct }
    private TelalaQuest quest;

    [Header("Info")]
    [SerializeField] private State state = State.None;
    [SerializeField] private int level;
    [SerializeField] private int moveCnt;
    [SerializeField] private int starCnt;

    [Header("UI Holder")]
    [SerializeField] private RectTransform blockBackground;
    [SerializeField] private Text moveCntText;
    [SerializeField] private Text starCntText;
    [SerializeField] private GameObject blockPrefab;

    [Header("Block Image")]
    [SerializeField] List<Row> level1;
    [SerializeField] List<Row> level2;
    [SerializeField] List<Row> level3;

    private Vector2 touchPos;
    private List<List<TelalaBlock>> nowPuzzle = new List<List<TelalaBlock>>();

    public override void StartQuest(Quest quest)
    {
        this.quest = quest.GetComponent<TelalaQuest>();
        level = 1;
        starCnt = 0;
        state = State.Start;
    }

    protected override void DuringQuest()
    {
        switch (state)
        {
            case State.Start:
                LoadPuzzle();
                state = State.Matching;
                touchPos = Vector2.zero;
                break;
            case State.Matching:
                MatchingAction();
                break;
            case State.Correct:
                //SaveResult();
                level += 1;
                state = State.Start;
                break;
        }
    }

    private void LoadPuzzle()
    {
        nowPuzzle = new List<List<TelalaBlock>>();
        List<Row> blocks = null;
        moveCnt = 0;

        if (level == 1) blocks = level1;
        else if (level == 2) blocks = level2;
        else if (level == 3) blocks = level3;

        int row = blocks.Count;
        int column = blocks[0].sprites.Count;
        float totalWidth = blockBackground.rect.width;
        float totalHeight = blockBackground.rect.height;
        float blockWidth = blocks[0].sprites[0].rect.width;
        float blockHeight = blocks[0].sprites[0].rect.height;
        float deltaX = (totalWidth - column * blockWidth) / (column + 1);
        float deltaY = (totalHeight - row * blockHeight) / (row + 1);

        for (int x = 0; x < row; x++)
        {
            nowPuzzle.Add(new List<TelalaBlock>());
            for (int y = 0; y < column; y++)
            {
                if (blocks[x].sprites[y] == null)
                {
                    nowPuzzle[x].Add(null);
                    continue;
                }

                float posX = y * blockWidth + (y + 1) * deltaX;
                float posY = (x * blockHeight + (x + 1) * deltaY) * -1;
                Vector2 blockPos = new Vector2(posX, posY);
                GameObject block = Instantiate(blockPrefab, blockBackground);
                nowPuzzle[x].Add(block.GetComponent<TelalaBlock>());
                block.GetComponent<RectTransform>().anchoredPosition = blockPos;
                block.GetComponent<Image>().sprite = blocks[x].sprites[y];
                block.GetComponent<Image>().SetNativeSize();
                block.GetComponent<TelalaBlock>().Initiailize(SpriteToCord(blocks[x].sprites[y])
                                                    , blockWidth + deltaX, blockHeight + deltaY);
            }
        }
    }

    private void MatchingAction()
    {
        UpdateMovableDirection();
        moveCntText.text = moveCnt.ToString();
        starCntText.text = starCnt.ToString();

        if (CheckMatch()) Debug.Log("Success");
    }

    private void UpdateMovableDirection()
    {
        int nullX = 0;
        int nullY = 0;

        for (int x = 0; x < nowPuzzle.Count; x++)
        {
            for (int y = 0; y < nowPuzzle[x].Count; y++)
            {
                if (nowPuzzle[x][y] != null) nowPuzzle[x][y].movableDir = TelalaBlock.Direction.None;
                else
                {
                    nullX = x;
                    nullY = y;
                }
            }
        }

        if (nullX - 1 >= 0) nowPuzzle[nullX - 1][nullY].movableDir = TelalaBlock.Direction.Down;
        if (nullX + 1 < nowPuzzle.Count) nowPuzzle[nullX + 1][nullY].movableDir = TelalaBlock.Direction.Up;
        if (nullY - 1 >= 0) nowPuzzle[nullX][nullY - 1].movableDir = TelalaBlock.Direction.Right;
        if (nullY + 1 < nowPuzzle[nullX].Count) nowPuzzle[nullX][nullY + 1].movableDir = TelalaBlock.Direction.Left;
}

    private bool CheckMatch()
    {
        for (int x = 0; x < nowPuzzle.Count; x++)
        {
            for (int y = 0; y < nowPuzzle[x].Count; y++)
            {
                if (nowPuzzle[x][y] == null) continue;

                if (nowPuzzle[x][y].answerRow != x || nowPuzzle[x][y].answerColumn != y)
                    return false;
            }
        }

        return true;
    }

    private Cord SpriteToCord(Sprite sprite)
    {
        string name = sprite.name;
        string[] values = name.Split(' ');
        return new Cord(int.Parse(values[2]), int.Parse(values[3]));
    }

    public void OnTouchDownPanel()
    {
        touchPos = Input.mousePosition;
    }

    public void OnTouchUpPanel()
    {
        Vector2 vector = (Vector2)Input.mousePosition - touchPos;
        if (vector.magnitude <= 50f)
        {
            Debug.Log("거리가 너무 작습니다. 현재 거리 = " + vector.magnitude);
            return;
        }

        float x = vector.x;
        float y = vector.y;
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0) OrderToMove(TelalaBlock.Direction.Right);
            else if (x < 0) OrderToMove(TelalaBlock.Direction.Left);
        }
        else if(Mathf.Abs(x) < Mathf.Abs(y))
        {
            if (y > 0) OrderToMove(TelalaBlock.Direction.Up);
            else if (y < 0) OrderToMove(TelalaBlock.Direction.Down);
        }

        touchPos = Vector2.zero;
    }

    private void OrderToMove(TelalaBlock.Direction dir)
    {
        for (int x = 0; x < nowPuzzle.Count; x++)
        {
            for (int y = 0; y < nowPuzzle[x].Count; y++)
            {
                if (nowPuzzle[x][y] == null) continue;
                if (nowPuzzle[x][y].Move(dir))
                {
                    moveCnt += 1;
                    switch (dir)
                    {
                        case TelalaBlock.Direction.Right:
                            nowPuzzle[x][y + 1] = nowPuzzle[x][y];
                            nowPuzzle[x][y] = null;
                            break;
                        case TelalaBlock.Direction.Left:
                            nowPuzzle[x][y - 1] = nowPuzzle[x][y];
                            nowPuzzle[x][y] = null;
                            break;
                        case TelalaBlock.Direction.Up:
                            nowPuzzle[x - 1][y] = nowPuzzle[x][y];
                            nowPuzzle[x][y] = null;
                            break;
                        case TelalaBlock.Direction.Down:
                            nowPuzzle[x + 1][y] = nowPuzzle[x][y];
                            nowPuzzle[x][y] = null;
                            break;
                    }
                    return;
                }
            }
        }
    }

    public struct Cord
    {
        public int row;
        public int column;

        public Cord(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }


    [System.Serializable]
    public struct Row
    {
        public List<Sprite> sprites;
    }
}
