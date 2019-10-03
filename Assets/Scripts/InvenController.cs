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

    public void OpenInven()
    {
        // 인벤 정보를 GameManager와 동기화
        items = GameManager.instance.itemList;

        for (int i = 0; i < items.Count; i++)
        {
            cell[i].GetComponent<Image>().sprite = items[i].info.image;
        }
    }

    public void ShowItem(int num)
    {        
        if (items.Count <= num)
        {
            descriptionPanel.SetActive(false);
            return;
        }

        Item targetItem = items[num];
        descriptionPanel.SetActive(true);
        itemImage.sprite = targetItem.info.image;
        itemName.text = targetItem.info.name;
        itemEffect.text = EffectDescription(num);
        itemDescription.text = targetItem.info.description;
    }

    private string EffectDescription(int num)
    {
        ItemInfo targetItem = items[num].info;

        int hpChange = targetItem.hpChange;
        int mpChange = targetItem.mpChange;

        if (hpChange != 0 && mpChange != 0)
        {
            return "체력 " + (hpChange > 0 ? "+" : "") + hpChange + ", 토성 " + (mpChange > 0 ? "+" : "") + mpChange;
        }
        else if (hpChange != 0 && mpChange == 0)
        {
            return "체력 " + (hpChange > 0 ? "+" : "") + hpChange;
        }
        else if (hpChange == 0 && mpChange != 0)
        {
            return "토성 " + (mpChange > 0 ? "+" : "") + mpChange;
        }
        else return "효과 없음";
    }
}
