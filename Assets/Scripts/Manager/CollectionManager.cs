using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Collection
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

    public void Acquire()
    {
        isGet = true;
    }
}

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager instance;

    [SerializeField] private List<Collection> collections = new List<Collection>();
    [SerializeField] private TextAsset collectionData;

    public List<Collection> Collections { get { return collections; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        List<Collection> collectionInfos = Parser.CollectionParse(collectionData);
        bool[] getCollection = GameManager.instance.getCollection;

        for (int i = 0; i < collectionInfos.Count; i++)
        {
            Collection collection = new Collection(getCollection[i], collectionInfos[i]);
            collections.Add(collection);
        }
    }

    public void AcquireCollection(string collectionName)
    {
        Debug.Log(collectionName + "에 해당하는 콜렉션을 획득했습니다.");
        GetCollectionByName(collectionName).Acquire();
    }

    private Collection GetCollectionByName(string collectionName)
    {
        for (int i = 0; i < collections.Count; i++)
        {
            if (collections[i].name == collectionName) return collections[i];
        }

        Debug.LogError(collectionName + "에 해당하는 콜렉션을 찾을 수 없습니다. 0번째 콜렉션을 반환합니다.");
        return collections[0];
    }
}


