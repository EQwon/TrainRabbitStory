using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int stage;
    public int mp;
    public int[] affinity;

    public Data(Data data)
    {
        stage = data.stage;
        mp = data.mp;

        affinity = new int[8];
        for (int i = 0; i < affinity.Length; i++)
        {
            affinity[i] = data.affinity[i];
        }
    }
}