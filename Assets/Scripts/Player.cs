using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public Guid Id;
    [SerializeField] public List<Item> items;
    private ItemManager itemManager;
    private InventoryManager inventoryManager;
    [SerializeField] private List<PlacementBox> placementBoxes;



    [SerializeField] private Text alarmTextBox;
    [SerializeField] private Text rememberTextfield;
    [SerializeField] private string neutralText;
    [SerializeField] private Animator animReminder;
    [SerializeField] private Animator animTextBG;



    private void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
        items = GetComponentInChildren<ItemHolder>().itemList;
        placementBoxes = FindObjectsOfType<PlacementBox>().ToList();

        items.ForEach(i => i.RegisterOnClick(delegate { inventoryManager.AddItem(i); }));
        for (int i = 0; i < placementBoxes.Count; i++)
        {
            var box = placementBoxes.ElementAt(i);
            box.RegisterOnClick(delegate { OnPlacementBoxClicked(box, i); });
        }
    }

    private void OnPlacementBoxClicked(PlacementBox box, int itemPos)
    {
        NetworkAPI.Instance.PlaceObject(inventoryManager.currentItem?.itemID.ToString(), itemPos);
        var temp = box.CurrentItem;
        if (temp != null)
        {
            temp.onDesk = false;
        }
        box.CurrentItem = inventoryManager.currentItem;
        inventoryManager.currentItem.onDesk = true;
        inventoryManager.AddItem(temp);
        
    }

    public void OnObjectPlaced(string itemId, int itemPos)
    {
        var remoteID = int.Parse(itemId);
        var remoteItem = itemManager.allItems[remoteID];
        placementBoxes.ElementAt(itemPos).CurrentItem = remoteItem;
    }

    public void OnObjectRemoved(string itemId, int itemPos)
    {
        placementBoxes.ElementAt(itemPos).CurrentItem = null;
    }

    public void CheckBoxes()
    {
        if (placementBoxes.All(b => b.CurrentItem != null))
        {
            var itemA = placementBoxes.First().CurrentItem;
            var itemB = placementBoxes.Last().CurrentItem;
            alarmTextBox.enabled = false;
            animTextBG.SetTrigger("FadeIn");
            if (itemA.IsPositiveMemoryCombination(itemB))
            {
                //positiv Audio
                //FMODUnity.RuntimeManager.PlayOneshot(EventRef, GetComponent<Transform>().position);
                rememberTextfield.text = (itemA.positiveMemoryText + itemB.positiveMemoryText);
                animReminder.SetTrigger("positv");
            }
            else if (itemA.IsNegativeMemoryCombination(itemB))
            {
                //negativ Audio
                rememberTextfield.text = (itemA.negativeMemoryText + itemB.negativeMemoryText);
                animReminder.SetTrigger("negativ");
            }
            else
            {
                //neutral Audio
                rememberTextfield.text = (neutralText);
                itemA.DropItem();
                animReminder.SetTrigger("neutral");
                itemB.DropItem();
            }
            placementBoxes.ForEach(b => b.CurrentItem = null);
            animTextBG.SetTrigger("FadeOut");
        }
        else if (placementBoxes.Any(b => b.CurrentItem != null))
        {
            alarmTextBox.enabled = true;
            //alarm Audio
        }
        else
        {
            alarmTextBox.enabled = false;
        }
    }
}