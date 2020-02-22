using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CollectionInfo
{
    private string name;
    private string desc;
    private Sprite img;

    public CollectionInfo(string name, string desc, Sprite img)
    {
        this.name = name;
        this.desc = desc;
        this.img = img;
    }
}

[System.Serializable]
public struct Collection
{
    private bool isGet;
    private CollectionInfo info;

    public Collection(bool isGet, CollectionInfo info)
    {
        this.isGet = isGet;
        this.info = info;
    }
}

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private List<Collection> collections = new List<Collection>();
    [SerializeField] private TextAsset collectionData;

    private void Start()
    {
        List<CollectionInfo> collectionInfos = Parser.CollectionParse(collectionData);
        bool[] getCollection = GameManager.instance.getCollection;

        for (int i = 0; i < collectionInfos.Count; i++)
        {
            collections.Add(new Collection(getCollection[i], collectionInfos[i]));
        }
    }
}


