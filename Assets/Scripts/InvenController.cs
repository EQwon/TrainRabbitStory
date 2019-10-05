using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenController : MonoBehaviour
{
    [Header("Inventory")]
    public List<GameObject> cell;
    private List<Image> cellItemImage = new List<Image>();
    private List<Text> cellItemCount = new List<Text>();
    public GameObject descriptionPanel;
    public Image itemImage;
    public Text itemName;
    public Text itemEffect;
    public Text itemDescription;
    private List<Item> items;
    private Item targetItem;

    public void OpenInven()
    {
        // 인벤 정보를 GameManager와 동기화
        items = GameManager.instance.itemList;

        if (cellItemCount.Count == 0) GetCellInfo();

        for (int i = 0; i < cell.Count; i++)
        {
            if (i >= items.Count)
            {
                cellItemImage[i].sprite = null;
                cellItemCount[i].text = "";
            }
            else
            {
                cellItemImage[i].sprite = items[i].info.image;
                cellItemCount[i].text = items[i].amount > 1 ? items[i].amount.ToString() : "";
            }
        }
    }

    public void ShowItem(int num)
    {        
        if (items.Count <= num)
        {
            descriptionPanel.SetActive(false);
            return;
        }

        descriptionPanel.SetActive(true);
        targetItem = items[num];
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

    private void GetCellInfo()
    {
        for (int i = 0; i < cell.Count; i++)
        {
            cellItemImage.Add(cell[i].transform.GetChild(0).GetComponent<Image>());
            cellItemCount.Add(cell[i].transform.GetChild(1).GetComponent<Text>());
        }
    }

    public void UseItem()
    {
        GameManager.instance.UseItem(targetItem);
        OpenInven();
        descriptionPanel.SetActive(false);
    }
}