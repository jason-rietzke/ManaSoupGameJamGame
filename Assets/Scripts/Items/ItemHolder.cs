using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] public List<Item> itemList;
    private void Start()
    {
        itemList = GetComponentsInChildren<Item>().ToList();
    }
}
