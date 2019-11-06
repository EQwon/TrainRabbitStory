using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public GameObject handkerchief;

    private void Start()
    {
        float spawnPosX = (10 - 3) * 20 + Random.Range(-7f, 7f);
        float spawnPosY = Random.Range(-4f, 2f);

        Instantiate(handkerchief, new Vector3(spawnPosX, spawnPosY, 0), Quaternion.identity);
    }
}
