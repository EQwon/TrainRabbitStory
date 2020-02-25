using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPanelController : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject collectionCellPrefab;

    [Header("UI Holder")]
    [SerializeField] private GameObject collectionListPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemOwnerText;
    [SerializeField] private Text itemDescText;

    private Vector2 offset = new Vector2(50, -50f);

    private void OnEnable()
    {
        CollectionManager collectionManager = GameManager.instance.GetComponent<CollectionManager>();
        List<Collection> collections = collectionManager.Collections;

        for (int i = 0; i < collections.Count; i++)
        {
            Collection collection = collections[i];
            Vector2 spawnPos = new Vector2(offset.x + 150 * (i % 4), offset.y - 150 * (i / 4));
            CollectionCellController cell = Instantiate(collectionCellPrefab, collectionListPanel.transform)
                                            .GetComponent<CollectionCellController>();

            cell.Initialize(spawnPos, collection, this);
        }

        infoPanel.SetActive(false);
    }

    public void ShowCollectionInfo(Collection collection)
    {
        if (collection.IsGet)
        {
            CollectionInfo info = collection.Info;
            itemImage.sprite = info.Img;
            itemNameText.text = info.Name;
            itemDescText.text = info.Desc;

            infoPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }
}
