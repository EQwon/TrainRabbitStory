using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUpQuest : MonoBehaviour
{
    private void Start()
    {
        CreateItem();
    }

    protected abstract void CreateItem();

    public abstract void GetItem();
}
