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
    [SerializeField] private GameObject blockPrefab;

    [Header("Block Image")]
    [SerializeField] List<Row> level1;
    [SerializeField] List<Row> level2;
    [SerializeField] List<Row> level3;

    public override void StartQuest(Quest quest)
    {
        this.quest = quest.GetComponent<TelalaQuest>();
        level = 1;
        state = State.Start;
    }

    protected override void DuringQuest()
    {
        switch (state)
        {
            case State.Start:
                LoadPuzzle();
                state = State.Matching;
                break;
            case State.Matching:
                //InteractionActivate();
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
        List<Row> blocks = null;
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
        Debug.Log("Row : " + row + ", column : " + column + ", total = (" + totalWidth + ", " + totalHeight + ")");

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                if (blocks[x].sprites[y] == null) continue;

                float posX = y * blockWidth + (y + 1) * deltaX;
                float posY = (x * blockHeight + (x + 1) * deltaY) * -1;
                Vector2 blockPos = new Vector2(posX, posY);
                GameObject block = Instantiate(blockPrefab, blockBackground);
                block.GetComponent<RectTransform>().anchoredPosition = blockPos;
                block.GetComponent<Image>().sprite = blocks[x].sprites[y];
                block.GetComponent<Image>().SetNativeSize();
                block.GetComponent<TelalaBlockMover>().Initiailize(new Cord(x, y)
                    , SpriteToCord(blocks[x].sprites[y]), blockWidth + deltaX, blockHeight + deltaY);
            }
        }
    }

    private Cord SpriteToCord(Sprite sprite)
    {
        string name = sprite.name;
        string[] values = name.Split(' ');
        return new Cord(int.Parse(values[2]), int.Parse(values[3]));
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
