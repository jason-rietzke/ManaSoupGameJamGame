using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public static bool isPlayerOne;
    [SerializeField] public List<Item> playerItems;
    [SerializeField] public List<Item> allItems;
    private InventoryManager inventoryManager;
    [SerializeField] private List<PlacementBox> placementBoxes;



    [SerializeField] private Text alarmTextBox;
    [SerializeField] private Text rememberTextfield;
    [SerializeField] private string neutralText;
    [SerializeField] private Animator animReminder;
    [SerializeField] private Animator animTextBG;
    private string placedItemId;
    private int placedItemPos;
    private string removedItemId;
    private int removedItemPos;

    private void Start()
    {
        allItems = GetComponentsInChildren<Item>().ToList();
        var playerTwoObjects = allItems.Where(i => i.transform.parent.name.Contains("Player 2")).ToList();
        var playerOneObjects = allItems.Where(i => i.transform.parent.name.Contains("Player 1")).ToList();
        if (isPlayerOne)
        {
            playerItems = playerOneObjects;
            playerTwoObjects.ForEach(o => o.gameObject.SetActive(false));
        }
        else
        {
            playerItems = playerTwoObjects;
            playerOneObjects.ForEach(o => o.gameObject.SetActive(false));
        }

        inventoryManager = GetComponentInChildren<InventoryManager>();
        placementBoxes = FindObjectsOfType<PlacementBox>().ToList();

        playerItems.ForEach(i => i.RegisterOnClick(delegate { inventoryManager.AddItem(i); }));
        placementBoxes.ForEach(b => b.RegisterOnClick(delegate { OnPlacementBoxClicked(b, b.Id); }));

        NetworkAPI.OnObjectPlaced += OnObjectPlaced;
        NetworkAPI.OnObjectRemoved += OnObjectRemoved;
    }

    private void Update()
    {
        if(placedItemId != null)
        {
            ObjectPlaced(placedItemId, placedItemPos);
            placedItemId = null;
        }

        if(removedItemId != null)
        {
            ObjectRemoved(removedItemId, removedItemPos);
            removedItemId = null;
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
        if(box.CurrentItem != null)
        {
            box.CurrentItem.onDesk = true;
        }
        
        inventoryManager.AddItem(temp);
        
    }

    public void OnObjectPlaced(string itemId, int itemPos)
    {
        placedItemId = itemId;
        placedItemPos = itemPos;
    }

    private void ObjectPlaced(string itemId, int itemPos)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            ObjectRemoved(itemId, itemPos);
            return;
        }
        var remoteID = int.Parse(itemId);
        var remoteItem = allItems[remoteID];
        placementBoxes.Single(b => b.Id.Equals(itemPos)).CurrentItem = remoteItem;
    }

    public void OnObjectRemoved(string itemId, int itemPos)
    {
        removedItemId = itemId;
        removedItemPos = itemPos;
    }

    private void ObjectRemoved(string itemId, int itemPos)
    {
        placementBoxes.Single(b => b.Id.Equals(itemPos)).CurrentItem = null;
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