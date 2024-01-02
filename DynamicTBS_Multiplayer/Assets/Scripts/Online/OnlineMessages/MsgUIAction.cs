using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class MsgUIAction : OnlineMessage
{
    public PlayerType playerId;
    public UIAction uiAction;

    public MsgUIAction() // Constructing a message.
    {
        Code = OnlineMessageCode.UI_ACTION;
    }

    public MsgUIAction(DataStreamReader reader) // Receiving a message.
    {
        Code = OnlineMessageCode.UI_ACTION;
        Id = reader.ReadFixedString64().Value;
        LobbyId = reader.ReadInt();
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer, int lobbyId)
    {
        base.Serialize(ref writer, lobbyId);
        writer.WriteByte((byte)playerId);
        writer.WriteByte((byte)uiAction);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = (PlayerType)reader.ReadByte();
        uiAction = (UIAction)reader.ReadByte();
    }

    public override void ReceivedOnClient()
    {
        if (OnlineClient.Instance.ShouldReadMessage(playerId))
        {
            GameplayEvents.UIActionExecuted(playerId, uiAction);
        }
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        OnlineServer.Instance.Broadcast(this, LobbyId);

        if (uiAction == UIAction.PAUSE_GAME || uiAction == UIAction.UNPAUSE_GAME)
        {
            OnlineServer.Instance.PauseGame(LobbyId, uiAction);
        }
    }
}
