using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject mapPanel;

    public void OpenBag()
    {
        mapPanel.SetActive(false);
    }
}
