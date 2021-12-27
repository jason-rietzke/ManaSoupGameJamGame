using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class CreateServerButton : MonoBehaviour
{
    private Button button;
    private NetworkAPI network;
    private MessageBoxHandler messageBox;
    private Button joinServerButton;
    [SerializeField][Scene] private string initialGameScene;
    private bool changeInteractable;
    private bool interactable = true;
    private bool loadLevelScene = false;

    void Start()
    {
        network = NetworkAPI.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(CreateGame);
        messageBox = FindObjectOfType<MessageBoxHandler>();
        joinServerButton = FindObjectOfType<JoinServerButton>().GetComponent<Button>();
    }

    private void Update()
    {
        if (changeInteractable)
        {
            joinServerButton.interactable = interactable;
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
        joinServerButton = FindObjectOfType<JoinServerButton>().GetComponent<Button>();
        joinServerButton.interactable = true;
    }

    private void OnDisable()
    {
        UnregisterHandlers();
        messageBox.MessageBoxText = String.Empty;
    }

    public void RegisterHandlers()
    {
        NetworkAPI.OnCreated += OnCreated;
        NetworkAPI.OnClosed += OnClosed;
        //NetworkAPI.OnJoined
        NetworkAPI.OnStart += OnStart;
    }

    public void UnregisterHandlers()
    {
        NetworkAPI.OnCreated -= OnCreated;
        NetworkAPI.OnClosed -= OnClosed;
        //NetworkAPI.OnJoined
        NetworkAPI.OnStart -= OnStart;
    }

    private void CreateGame()
    {
        try
        {
            network.CreateServer();
        }
        catch (Exception e)
        {
            messageBox.MessageBoxText = $"Couldn't create a Server";
            Debug.Log($"{messageBox.MessageBoxText}{e.Message}");
        }
    }

    private void OnCreated(string roomId)
	{
        messageBox.MessageBoxText = $"Created room: {roomId}";
        Debug.Log(messageBox.MessageBoxText);
        interactable = false;
        changeInteractable = true;
    }

    private void OnClosed()
    {
        interactable = true;
        changeInteractable = true;
    }

    private void OnStart(string roomId)
    {
        loadLevelScene = true;
    }

    private IEnumerator LoadLevelScene()
    {
        Player.isPlayerOne = true;
        var operation = SceneManager.LoadSceneAsync(initialGameScene);
        //TODO: WaitWhile
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
        //    .Where(p => !p.isPlayerOne)
        //    .SingleOrDefault()
        //    .gameObject.SetActive(false);
    }
}
