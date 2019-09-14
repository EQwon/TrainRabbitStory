using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int stage;
    public int hp;
    public int mp;
    public int[] affinity;
    public int[] talkCnt;

    public Data()
    {
        stage = 0;
        hp = 1000;
        mp = 100;
        affinity = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        talkCnt = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public Data(Data data)
    {
        stage = data.stage;
        hp = 1000;
        mp = data.mp;

        affinity = new int[9];
        talkCnt = new int[9];
        for (int i = 0; i < affinity.Length; i++)
        {
            affinity[i] = data.affinity[i];
            talkCnt[i] = data.talkCnt[i];
        }
    }
}