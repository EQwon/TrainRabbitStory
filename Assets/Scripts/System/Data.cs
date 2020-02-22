using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StoryBunny
{
    private int affinity;
    private int talkCnt;

    public int Affinity { get { return affinity; } }
    public int TalkCnt { get { return talkCnt; } }

    public StoryBunny(int _affinity, int _talkCnt)
    {
        affinity = _affinity;
        talkCnt = _talkCnt;
    }

    public void ChangeAffinity(int amount)
    {
        int target = affinity + amount;

        if (target < 0) target = 0;
        if (target > 100) target = 100;

        affinity = target;
    }

    public void IncreaseTalkCnt()
    {
        talkCnt += 1;
    }
}

[System.Serializable]
public class Data
{
    public int stage;
    public int hp;
    public int mp;
    public StoryBunny[] storyBunnies;
    public bool[] getCollection;

    public Data()
    {
        stage = 0;
        hp = 1000;
        mp = 100;
        storyBunnies = new StoryBunny[9]
            {
                new StoryBunny(0,0), new StoryBunny(0,0),new StoryBunny(0,0),
                new StoryBunny(0,0),new StoryBunny(0,0),new StoryBunny(0,0),
                new StoryBunny(0,0),new StoryBunny(0,0),new StoryBunny(0,0)
            };
        getCollection = new bool[50];
    }
}