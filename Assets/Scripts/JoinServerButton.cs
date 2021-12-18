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
    private NetworkAPI network = NetworkAPI.Instance;
    private NetworkIdInputField inputField;
    private MessageBoxHandler messageBox;
    private Button createLobbyButton;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(JoinGame);
        inputField = FindObjectOfType<NetworkIdInputField>();
        messageBox = FindObjectOfType<MessageBoxHandler>();
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
        NetworkAPI.OnClosed += OnWrongId;
        NetworkAPI.OnJoined += OnJoined;
        NetworkAPI.OnStart += OnGameStart;
        NetworkAPI.OnCreated += OnGameCreated;
    }

    public void UnregisterHandlers()
    {
        NetworkAPI.OnClosed -= OnWrongId;
        NetworkAPI.OnJoined -= OnJoined;
        NetworkAPI.OnStart -= OnGameStart;
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

    private void OnGameStart(string id)
    {
        NetworkAPI.OnClosed -= OnWrongId;
        messageBox.MessageBoxText = "Game startet";
        Debug.Log(messageBox.MessageBoxText);
        //TODO: load game
        SceneManager.LoadScene("Raum 1");
        var scene = SceneManager.GetSceneByName("Raum 1");
        var root = scene.GetRootGameObjects();
        root.First().transform.Find("HUDPlayer 1").gameObject.SetActive(false);
        root.First().transform.Find("Mainscreen P 1").gameObject.SetActive(false);
        root.First().transform.Find("Main Camera A").gameObject.SetActive(false);
    }

	private void OnJoined(string id)
	{
        createLobbyButton.interactable = false;
        messageBox.MessageBoxText = "Raum beigetreten";
        Debug.Log(messageBox.MessageBoxText);
    }

    private void OnWrongId()
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
