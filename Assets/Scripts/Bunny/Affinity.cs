using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affinity : MonoBehaviour
{
    public GameObject affinityPrefab;
    public int bunnyNum;

    private Transform affinityBar;

    private void Start()
    {
        GameObject affinity = Instantiate(affinityPrefab, transform);

        affinityBar = affinity.transform.GetChild(1).GetChild(0);

        UpdateAffintiyBar();
    }

    private void UpdateAffintiyBar()
    {
        affinityBar.localScale = new Vector3((float)GameManager.instance.Affinity[bunnyNum] / 100, 1, 1);
    }

    public void IncreaseAffinity()
    {
        GameManager.instance.Affinity[bunnyNum] += 10;
        UpdateAffintiyBar();
    }
}
