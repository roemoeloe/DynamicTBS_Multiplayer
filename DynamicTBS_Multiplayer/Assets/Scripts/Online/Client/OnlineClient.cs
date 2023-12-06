using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using System.Net;
using System.Net.Sockets;

public enum ClientType
{
    PLAYER = 1,
    SPECTATOR = 2
}

public enum ConnectionStatus
{
    UNCONNECTED = 0,
    CONNECTED = 1,
    LOBBY_NOT_FOUND = 2,
    CONNECTION_DECLINED = 3,
    ATTEMPT_CONNECTION = 4,
    IN_LOBBY = 5
}

public class OnlineClient : MonoBehaviour
{
    [SerializeField] private MessageBroker messageBroker;

    #region SingletonImplementation

    public static OnlineClient Instance { set; get; }

    private void Awake()
    {
        Instance = this;
        Shutdown();
    }

    #endregion

    private string ip;
    private ushort port;

    private NetworkDriver driver;
    private NetworkConnection connection;
    public Action connectionDropped;

    private bool isActive = false;
    public bool IsActive { get { return isActive; } }

    private ConnectionStatus connectionStatus = ConnectionStatus.UNCONNECTED;
    public ConnectionStatus ConnectionStatus { get { return connectionStatus; } set { connectionStatus = value; } }

    private bool isLoadingGame = false;
    public bool IsLoadingGame { get { return isLoadingGame; } }

    private LobbyId lobbyId;
    public LobbyId LobbyId { get { return lobbyId; } }
    private UserData userData;
    public UserData UserData { get { return userData; } }
    private bool isAdmin;
    public bool IsAdmin { get { return isAdmin; } }

    private PlayerType side;
    public PlayerType Side { get { return side; } }

    private int playerCount = 0;
    public int PlayerCount { get { return playerCount; } set { playerCount = value; } }
    private int spectatorCount = 0;
    public int SpectatorCount { get { return spectatorCount; } set { spectatorCount = value; } }

    private string opponentName;
    public string OpponentName { get { return opponentName == null ? "" : opponentName; } }

    private Dictionary<PlayerType, string> playerNames = new();

    private float serverTimeDiff = 0;
    public float ServerTimeDiff { get { return serverTimeDiff; } set { serverTimeDiff = value; } }

    #region Networking
    public void Init(string ip, ushort port, UserData userData, LobbyId lobbyId)
    {
        this.ip = IPResolver.ResolveIp(ip);
        Debug.Log("Resolved IP is " + this.ip);

        this.port = port;
        this.userData = userData;
        this.lobbyId = lobbyId;

        driver = NetworkDriver.Create();
        NetworkEndPoint endPoint = NetworkEndPoint.Parse(this.ip, port); // Specific endpoint for connection.

        connection = driver.Connect(endPoint); // Connecting based on the endpoint that was just created.
        connectionStatus = ConnectionStatus.ATTEMPT_CONNECTION;
        Debug.Log("Client: Attempting to connect to server on " + endPoint.Address);

        messageBroker.Driver = driver;

        isActive = true;
    }

    private void Update()
    {
        if (!isActive) return;

        driver.ScheduleUpdate().Complete(); // Makes sure driver processed all incoming messages.
        CheckAlive(); // Checks if connection to server is alive.
        UpdateMessagePump(); // Check for messages and if server has to reply.
    }

    public void SendToServer(OnlineMessage msg)
    {
        messageBroker.SendMessage(msg, connection, lobbyId.Id);
    }

    private void CheckAlive()
    {
        if (!connection.IsCreated && isActive) // If no connections is created, but client is active, something went wrong.
        {
            Debug.Log("Client: Something went wrong. Lost connection to server.");
            connectionDropped?.Invoke();
            connectionStatus = ConnectionStatus.UNCONNECTED;
            Reconnect();
        }
    }

    private void UpdateMessagePump()
    {
        try
        {
            DataStreamReader stream; // Reads incoming messages.
            NetworkEvent.Type cmd;

            while (connection != null && (cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("Client: We're connected!");
                    connectionStatus = ConnectionStatus.CONNECTED;
                    SyncTimeWithServer();
                    JoinLobby();
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    OnlineMessageHandler.HandleData(stream, connection);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client: Client disconnected from server.");
                    connectionStatus = ConnectionStatus.UNCONNECTED;
                    connection = default(NetworkConnection);
                    connectionDropped?.Invoke();
                    Shutdown();
                }
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    private void Reconnect()
    {
        Init(ip, port, userData, lobbyId);
    }

    public void Shutdown()
    {
        if (isActive)
        {
            Debug.Log("Shutting down Client.");
            connection.Disconnect(driver);
            driver.Dispose();
            isActive = false;
            connectionStatus = ConnectionStatus.UNCONNECTED;
            Destroy(this.gameObject);
        }
    }

    #endregion

    public bool ShouldReadMessage(PlayerType playerType)
    {
        return side != playerType || isLoadingGame;
    }

    public bool ShouldSendMessage(PlayerType playerType)
    {
        return side == playerType && !isLoadingGame;
    }

    public bool AdminShouldSendMessage()
    {
        return isAdmin && !isLoadingGame;
    }

    public void UpdateClient(LobbyId lobbyId, bool isAdmin, PlayerType side, string opponentName)
    {
        this.lobbyId = lobbyId;
        this.isAdmin = isAdmin;
        this.side = side;
        this.opponentName = opponentName;
    }

    public string GetPlayerName(PlayerType side)
    {
        if (userData.Role == ClientType.SPECTATOR)
            return playerNames.ContainsKey(side) ? playerNames[side] : "";

        return Side == side ? UserData.Name : OpponentName;
    }

    public void UpdatePlayerNames(string pinkName, string blueName)
    {
        playerNames[PlayerType.pink] = pinkName;
        playerNames[PlayerType.blue] = blueName;
    }

    public void ChooseSide(PlayerType side)
    {
        this.side = side;
        SendToServer(new MsgUpdateClient
        {
            isAdmin = isAdmin,
            side = side,
            opponentName = opponentName
        });
    }

    private void SyncTimeWithServer()
    {
        SendToServer(new MsgSyncTime { });
    }

    private void JoinLobby()
    {
        SendToServer(new MsgJoinLobby
        {
            create = lobbyId.IsNewLobby,
            lobbyName = lobbyId.Name,
            userData = userData
        });
    }

    public void ToggleIsLoadingGame()
    {
        isLoadingGame = !isLoadingGame;
        GameEvents.IsGameLoading(isLoadingGame);
    }

    public void StartGame(TimerSetupType timerSetup, MapType selectedMap)
    {
        Board.selectedMapType = selectedMap;
        Timer.InitTime(timerSetup);
        GameManager.GameType = GameType.ONLINE;

        GameEvents.StartGame();
    }

    private void OnDestroy()
    {
        Shutdown();
    }
}
