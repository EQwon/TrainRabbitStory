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

    public Data()
    {
        stage = 0;
        hp = 1000;
        mp = 100;
        affinity = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public Data(Data data)
    {
        stage = data.stage;
        hp = 1000;
        mp = data.mp;

        affinity = new int[8];
        for (int i = 0; i < affinity.Length; i++)
        {
            affinity[i] = data.affinity[i];
        }
    }
}