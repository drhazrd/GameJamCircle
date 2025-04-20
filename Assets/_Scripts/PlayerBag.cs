using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    int itemACount,itemBCount,itemCCount, itemDCount;

    void Start()
    {
        {
            items.Add(new Item { count = itemACount, type = ItemType.ItemA, sprite = null});
            items.Add(new Item { count = itemBCount, type = ItemType.ItemB, sprite = null});
            items.Add(new Item { count = itemCCount, type = ItemType.ItemC, sprite = null});
            items.Add(new Item { count = itemDCount, type = ItemType.ItemD, sprite = null});
        }
    }
}
public class Item{
    public int count;
    public ItemType type;
    public Sprite sprite;
}
public enum ItemType{
    ItemA,
    ItemB,
    ItemC,
    ItemD,

}