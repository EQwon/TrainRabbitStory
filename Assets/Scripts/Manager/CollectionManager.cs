using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CollectionInfo
{
    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private Sprite img;

    public CollectionInfo(string name, string desc, Sprite img)
    {
        this.name = name;
        this.desc = desc;
        this.img = img;
    }

    public string Name { get { return name; } }
    public string Desc { get { return desc; } }
    public Sprite Img { get { return img; } }
}

[System.Serializable]
public struct Collection
{
    [SerializeField] private bool isGet;
    [SerializeField] private CollectionInfo info;

    public Collection(bool isGet, CollectionInfo info)
    {
        this.isGet = isGet;
        this.info = info;
    }

    public bool IsGet { get { return isGet; } }
    public CollectionInfo Info { get { return info; } }
}

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private List<Collection> collections = new List<Collection>();
    [SerializeField] private TextAsset collectionData;

    public List<Collection> Collections { get { return collections; } }

    private void Start()
    {
        List<CollectionInfo> collectionInfos = Parser.CollectionParse(collectionData);
        bool[] getCollection = GameManager.instance.getCollection;

        for (int i = 0; i < collectionInfos.Count; i++)
        {
            collections.Add(new Collection(getCollection[i], collectionInfos[i]));
        }
    }

    public static void AcquireCollection(string collectionName)
    {
        Debug.Log(collectionName + "에 해당하는 콜렉션을 획득했습니다.");
    }
}


