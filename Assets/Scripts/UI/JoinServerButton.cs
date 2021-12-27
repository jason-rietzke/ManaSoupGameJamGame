using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class JoinServerButton : MonoBehaviour
{
    private Button button;
    private NetworkAPI network;
    private NetworkIdInputField inputField;
    private MessageBoxHandler messageBox;
    private Button createLobbyButton;
    [SerializeField][Scene] private string initialGameScene;
    private bool changeInteractable;
    private bool interactable;
    private bool loadLevelScene;

    void Start()
    {
        network = NetworkAPI.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(JoinGame);
        inputField = FindObjectOfType<NetworkIdInputField>();
        messageBox = FindObjectOfType<MessageBoxHandler>();
    }

    private void Update()
    {
        if (changeInteractable)
        {
            createLobbyButton.interactable = interactable;
            changeInteractable = false;
        }

        if (loadLevelScene)
        {
            loadLevelScene = false;
            StartCoroutine(LoadLevelScene());
        }
    }

    private void OnEnable()
    {
        RegisterHandlers();
        createLobbyButton = FindObjectOfType<CreateServerButton>().GetComponent<Button>();
        createLobbyButton.interactable = true;
    }

    private void OnDisable()
    {
        UnregisterHandlers();
        messageBox.MessageBoxText = String.Empty;
    }

    public void RegisterHandlers()
    {
        NetworkAPI.OnClosed += OnRoomNotExist;
        NetworkAPI.OnJoined += OnJoined;
        NetworkAPI.OnStart += OnStart;
        NetworkAPI.OnCreated += OnGameCreated;
    }

    public void UnregisterHandlers()
    {
        NetworkAPI.OnClosed -= OnRoomNotExist;
        NetworkAPI.OnJoined -= OnJoined;
        NetworkAPI.OnStart -= OnStart;
        NetworkAPI.OnCreated -= OnGameCreated;
    }

    private void JoinGame()
    {
        //TODO: show error message
        if (!string.IsNullOrWhiteSpace(inputField.id))
        {
            network.JoinServer(inputField.id);
        }
    }

    private void OnStart(string id)
    {
        loadLevelScene = true;
    }

    private IEnumerator LoadLevelScene()
    {
        NetworkAPI.OnClosed -= OnRoomNotExist;
        messageBox.MessageBoxText = "Game startet";
        Debug.Log(messageBox.MessageBoxText);

        Player.isPlayerOne = false;
        var operation = SceneManager.LoadSceneAsync(initialGameScene);
        while (!operation.isDone)
        {
            Debug.Log($"Scene is loading...");
            yield return new WaitForFixedUpdate();
        }
        //var scene = SceneManager.GetSceneByName(initialGameScene);
        Debug.Log("Scene is loaded: ");
        //var root = scene.GetRootGameObjects();
        //root.First()
        //    .GetComponentsInChildren<Player>()
        //    .Where(p => p.isPlayerOne)
        //    .SingleOrDefault()
        //    .gameObject.SetActive(false);
    }


    private void OnJoined(string id)
	{
        interactable = false;
        changeInteractable = true;
        messageBox.MessageBoxText = "Raum beigetreten";
        Debug.Log(messageBox.MessageBoxText);
    }

    private void OnRoomNotExist()
    {
        messageBox.MessageBoxText = "Raum existiert nicht!";
        Debug.Log(messageBox.MessageBoxText);
    }

    private void OnGameCreated(string id)
    {
        //messageBox.MessageBoxText = "Lobby erstellt";
        //Debug.Log(messageBox.MessageBoxText);
    }
}
