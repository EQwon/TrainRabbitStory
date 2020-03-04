using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Collection
{
    public bool isGet;
    public string name;
    public string desc;
    public Sprite img;

    public Collection(bool isGet, string name, string desc, Sprite img)
    {
        this.isGet = isGet;
        this.name = name;
        this.desc = desc;
        this.img = img;
    }

    public Collection(bool isGet, Collection collection)
    {
        this.isGet = isGet;
        this.name = collection.name;
        this.desc = collection.desc;
        this.img = collection.img;
    }
}

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private List<Collection> collections = new List<Collection>();
    [SerializeField] private TextAsset collectionData;

    public List<Collection> Collections { get { return collections; } }

    private void Start()
    {
        List<Collection> collectionInfos = Parser.CollectionParse(collectionData);
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


