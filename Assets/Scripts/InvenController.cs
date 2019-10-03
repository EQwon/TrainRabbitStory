using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenController : MonoBehaviour
{
    [Header("Inventory")]
    public List<GameObject> cell;
    public GameObject descriptionPanel;
    public Image itemImage;
    public Text itemName;
    public Text itemEffect;
    public Text itemDescription;
    private List<Item> items;

    private void OpenInven()
    {
        // 인벤 정보를 GameManager와 동기화
        items = GameManager.instance.Items;

        for (int i = 0; i < items.Count; i++)
        {
            cell[i].GetComponent<Image>().sprite = items[i].info.image;
        }
    }

    public void ShowItem(int num)
    {
        OpenInven();
        
        if (items.Count <= num)
        {
            descriptionPanel.SetActive(false);
            return;
        }

        Item targetItem = items[num];
        descriptionPanel.SetActive(true);
        itemImage.sprite = targetItem.info.image;
        itemName.text = targetItem.info.name;
        itemDescription.text = targetItem.info.description;
    }
}
