using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public int indexNum;
    public string name;
    public Sprite image;
    public int hpChange;
    public int mpChange;
    public string description;

    public ItemInfo(int index, string itemName, Sprite itemImage, int hp, int mp, string itemDescription)
    {
        indexNum = index;
        name = itemName;
        image = itemImage;
        hpChange = hp;
        mpChange = mp;
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
