using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Sprite emptyImage;
    [SerializeField] private Image icon;
    [SerializeField] public bool hasItem;
    [SerializeField] public Item currentItem;
    public void AddItem(Item item)
    {
        if(item == null)
        {
            RemoveItem();
            return;
        }
        currentItem?.DropItem();
        item.PickItem();
        currentItem = item;
        icon.sprite = item.GetComponent<Image>().sprite;
        hasItem = true;
    }

    public void RemoveItem()
    {
        currentItem.DropItem();
        currentItem = null;
        icon.sprite = emptyImage;
        hasItem = false;
    }
}
