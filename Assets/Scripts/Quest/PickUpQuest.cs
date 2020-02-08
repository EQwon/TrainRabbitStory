using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUpQuest : MonoBehaviour
{
    protected virtual void Start()
    {
        CreateItem();
    }

    protected abstract void CreateItem();

    public abstract void GetItem();
}
