using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public string name;
    public Sprite image;
    public bool isForQuest;
    public string description;

    public ItemInfo(string itemName, Sprite itemImage, bool isQuestItem, string itemDescription)
    {
        name = itemName;
        image = itemImage;
        isForQuest = isQuestItem;
        description = itemDescription;
    }
}

public class Item
{
    public ItemInfo info;
    public int amount;

    public Item(ItemInfo myInfo, int myAmount)
    {
        info = myInfo;
        amount = myAmount;
    }
}
