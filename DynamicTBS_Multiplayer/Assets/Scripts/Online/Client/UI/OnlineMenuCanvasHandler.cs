using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineMenuCanvasHandler : MonoBehaviour
{
    [SerializeField] private OnlineClient client;

    [SerializeField] private InputField userName;
    [SerializeField] private InputField lobbyNameOrId;
    [SerializeField] private Toggle spectatorToggle;

    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;

    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject onlineMetadataCanvas;

    private void Awake()
    {
        onlineMetadataCanvas.SetActive(false);
    }

    private void Update()
    {
        createLobbyButton.interactable = IsValidName() && IsValidLobbyName();
        joinLobbyButton.interactable = IsValidName() && IsValidLobbyID();
    }

    public void CreateLobby()
    {
        JoinLobby(new LobbyId(lobbyNameOrId.text));
    }

    public void JoinLobby()
    {
        JoinLobby(LobbyId.FromFullId(lobbyNameOrId.text));
    }

    private void JoinLobby(LobbyId lobbyId)
    {
        UserData userData = new UserData(userName.text.Trim(), spectatorToggle.isOn ? ClientType.SPECTATOR : ClientType.PLAYER);
        client.Init(ConfigManager.Instance.IpAdress, ConfigManager.Instance.Port, userData, lobbyId);

        lobbyCanvas.SetActive(true);
        onlineMetadataCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private bool IsValidName()
    {
        return userName.text.Trim().Length > 0;
    }

    private bool IsValidLobbyName()
    {
        return lobbyNameOrId.text.Trim().Length > 0 && !lobbyNameOrId.text.Contains("#");
    }

    private bool IsValidLobbyID()
    {
        return lobbyNameOrId.text.Trim().Length > 0 && LobbyId.FromFullId(lobbyNameOrId.text) != null;
    }
}