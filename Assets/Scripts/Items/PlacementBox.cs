using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlacementBox : MonoBehaviour
{
    [SerializeField] public int Id;
    private Item currentItem;
    private Button button;
    [SerializeField] private Sprite EmptyImage;
    public Item CurrentItem
    {
        set
        {
            currentItem = value;
            placedItemImage.sprite = value == null ? EmptyImage : value.GetComponent<Image>().sprite;
        } 
        get => currentItem;
    }
    
    [SerializeField] public Image placedItemImage { get; private set; }
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        placedItemImage = GetComponentsInChildren<Image>().Where(i => i != GetComponent<Image>()).Single();
    }

    public void RegisterOnClick(UnityAction call)
    {
        button.onClick.AddListener(call);
    }
}
