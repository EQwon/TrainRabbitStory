using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affinity : MonoBehaviour
{
    public int bunnyNum;

    public void IncreaseAffinity()
    {
        GameManager.instance.Affinity[bunnyNum] += 10;
    }
}
