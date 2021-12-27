using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkSandbox : MonoBehaviour
{

    [SerializeField] private Button AddObjectButton;
    [SerializeField] private Button RemoveObjectButton;

    // Start is called before the first frame update
    void Start() {
        NetworkAPI network1 = new NetworkAPI();
        NetworkAPI network2 = new NetworkAPI();
        AddObjectButton.onClick.AddListener(delegate { Debug.Log("Placing Object..."); network1.PlaceObject("1", 4); });
        RemoveObjectButton.onClick.AddListener(delegate { Debug.Log("Removing Object..."); network1.RemoveObject("1", 4); });

        network1.CreateServer();
        
        // network.JoinServer();
        
        // network.LeaveServer();
        
        // network.PlaceObject();
        
        // network.RemoveObject();
        
        NetworkAPI.OnCreated += (roomId) => {
            Debug.Log("Server created ROOM ID: " + roomId);
            network2.JoinServer(roomId);
        };
        
        NetworkAPI.OnJoined += (roomId) => {
            Debug.Log("Server joined");
        };
        
        NetworkAPI.OnStart += (roomId) => {
            Debug.Log("Server started");
        };

        NetworkAPI.OnObjectPlaced += (objId, objPos) => {
            Debug.Log($"Object {objId} placed at pos {objPos}");
        };
        
        NetworkAPI.OnObjectRemoved += (objId, objPos) => {
            Debug.Log($"Object {objId} removed from pos {objPos}");
        };
    }
}
