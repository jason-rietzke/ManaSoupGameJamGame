using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<Player> players;
    [SerializeField] public List<Item> allItems;

    private void Start()
    {
        allItems = players.SelectMany(p => p.items).ToList();

        players.ForEach(p => NetworkAPI.OnObjectPlaced += p.OnObjectPlaced);
        players.ForEach(p => NetworkAPI.OnObjectRemoved += p.OnObjectRemoved);
    }

    //Remember Range  -7 bis 7        -7 -6 -5 | -4 -3 -2 | -1 0 1 | 2 3 4 | 5 6 7
}
