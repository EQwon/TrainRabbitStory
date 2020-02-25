using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCellController : MonoBehaviour
{
    [SerializeField] private Image myImage;

    private Collection collection;

    public void Initialize(Vector2 spawnPos, Collection collection, CollectionPanelController controller)
    {
        GetComponent<RectTransform>().anchoredPosition = spawnPos;
        this.collection = collection;
        GetComponent<Button>().onClick.AddListener(() => controller.ShowCollectionInfo(collection));

        if (collection.IsGet)
        {
            myImage.sprite = collection.Info.Img;
        }
    }
}
