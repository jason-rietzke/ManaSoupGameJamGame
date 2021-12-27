using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private Button button;

    [SerializeField] public int itemID;
    [SerializeField] public int positiveMemoryID;
    [SerializeField] public int negativeMemoryID;
    [SerializeField] public bool onDesk;

    private bool disableInScene;
    private bool enableInScene;

    [Header("RememberTexts")]
    [SerializeField] public string positiveMemoryText;
    [SerializeField] public string negativeMemoryText;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void RegisterOnClick(UnityAction call)
    {
        button.onClick.AddListener(call);
    }

    public void PickItem()
    {
        disableInScene = true;
    }

    public void DropItem()
    {
        if (onDesk)
        {
            return;
        }
        enableInScene = true;
    }

    public bool IsPositiveMemoryCombination(Item item)
    {
        return this.positiveMemoryID.Equals(item.itemID);
    }
    public bool IsNegativeMemoryCombination(Item item)
    {
        return this.negativeMemoryID.Equals(item.itemID);
    }

    private void FixedUpdate()
    {
        if (disableInScene)
        {
            disableInScene = false;
            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().interactable = false;
        }
        if (enableInScene)
        {
            enableInScene = false;
            GetComponent<CanvasGroup>().alpha = 1;
            GetComponent<CanvasGroup>().interactable = true;
        }
    }
}
