using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private NetworkAPI network = NetworkAPI.Instance;

    public int PlayerID = 1;
    public Text alarmTextBox;
    public string alarmtext = "Ein Gegenstand wurde in eine Box gelegt.";
    
    public Roomchange roomchangeCS;

    public int itemID;
    public bool inventarFull;

    public Item[] allItems;
    public Item lastPicked;

    public Item boxA_Item;
    public Image spriteBoxA;
    public Item boxB_Item;
    public Image spriteBoxB;

    //Inventar
    public Image icon;
    public Sprite[] itemSprites;
    public Sprite empty;

    //STATUS
    public Text rememberTextfield;
    public string neutralText;
    public int rememberCount = 0;

    public Animator animReminder;
    public Animator animTextBG;


    private void FixedUpdate()
    {
        if (!inventarFull)
            icon.sprite = empty;
        else
            icon.sprite = itemSprites[lastPicked.itemID];

        //Remember Range  -7 bis 7        -7 -6 -5 | -4 -3 -2 | -1 0 1 | 2 3 4 | 5 6 7

    }

    public void PickedUp()
    {   
        icon.sprite = itemSprites[itemID];
        inventarFull = true;      
    }

    private void Start()
    {
        //Other Player see item
        NetworkAPI.OnObjectPlaced += (objId) =>
        {
            int remoteID = int.Parse(objId);
            Item remoteItem = allItems[remoteID];
            if (PlayerID == 1)
            {
                boxB_Item = remoteItem;
                spriteBoxB.sprite = itemSprites[remoteID];
            }
            else
            {
                boxA_Item = remoteItem;
                spriteBoxA.sprite = itemSprites[remoteID];
            }
            CheckBoxes();
        };

        NetworkAPI.OnObjectRemoved += (objId) => {
            

            if (PlayerID == 1)
            {
                boxB_Item = null;
                spriteBoxB.sprite = empty;
            }
            else
            {
                boxA_Item = null;
                spriteBoxA.sprite = empty;
            }
        };
    }

    public void BoxClicked_A() //Button Call
    {
  
        if (inventarFull)
        {
          if (boxA_Item == null)
            {
                boxA_Item = lastPicked;
                spriteBoxA.sprite = itemSprites[lastPicked.itemID];
                inventarFull = false;
            }
          else
            {
                Item inventarSwap = boxA_Item;
                network.RemoveObject(boxA_Item.itemID.ToString());
                boxA_Item = lastPicked;
                spriteBoxA.sprite = itemSprites[lastPicked.itemID];
                lastPicked = inventarSwap;
                inventarFull = true;
            }

            //Call other Player
            network.PlaceObject(boxA_Item.itemID.ToString());
        }

    }
    public void BoxClicked_B() //Button Call
    {
      
        if (inventarFull)
        {
            if (boxB_Item == null)
            {
                boxB_Item = lastPicked;
                spriteBoxB.sprite = itemSprites[lastPicked.itemID];
                inventarFull = false;
            }
            else
            {
                Item inventarSwap = boxB_Item;
                network.RemoveObject(boxB_Item.itemID.ToString());
                boxB_Item = lastPicked;
                spriteBoxB.sprite = itemSprites[lastPicked.itemID];
                lastPicked = inventarSwap;
                inventarFull = true;
            }

            //Call other Player
            network.PlaceObject(boxB_Item.itemID.ToString());
            CheckBoxes();
        }

    }

    public void CheckBoxes()
    {
        if (boxA_Item != null && boxB_Item != null)
            {
            alarmTextBox.enabled = false;
            Compare();
            }
        else if (boxA_Item != null || boxB_Item != null)
        {
            alarmTextBox.enabled = true;
            //alarm Audio
        }
        else
            alarmTextBox.enabled = false;
       
    }

    public void Compare()
    {
        if (boxA_Item.posID == boxB_Item.itemID)
        {
            StartCoroutine(Wait(10));
            //positiv Audio
            rememberCount++;
            animTextBG.SetTrigger("FadeIn");
            rememberTextfield.text = (boxA_Item.posText + boxB_Item.posText);
            boxA_Item = null;
            spriteBoxA.sprite = empty;

            animReminder.SetTrigger("positv");

            boxB_Item = null;
            spriteBoxB.sprite = empty;

        }

        else if (boxA_Item.negID == boxB_Item.itemID)
        {
            StartCoroutine(Wait(10));
            //negativ Audio
            rememberCount--;
            animTextBG.SetTrigger("FadeIn");
            rememberTextfield.text = (boxA_Item.negText + boxB_Item.negText);
            boxA_Item = null;
            spriteBoxA.sprite = empty;

            animReminder.SetTrigger("negativ");

            boxB_Item = null;
            spriteBoxB.sprite = empty;
        }

        else
        {
            StartCoroutine(Wait(10));
            //neutral Audio
            animTextBG.SetTrigger("FadeIn");
            rememberTextfield.text = (neutralText);
            boxA_Item = null;
            spriteBoxA.sprite = empty;
            boxA_Item.picked = false;

            animReminder.SetTrigger("neutral");

            boxB_Item = null;
            spriteBoxB.sprite = empty;
            boxB_Item.picked = false;
        }

        //can change room again

    }

    private IEnumerator Wait(int duration)
    {
        roomchangeCS.isAnimating = false;
        yield return new WaitForSeconds(duration);
        roomchangeCS.isAnimating = true;
        animTextBG.SetTrigger("FadeOut");
    }

}
