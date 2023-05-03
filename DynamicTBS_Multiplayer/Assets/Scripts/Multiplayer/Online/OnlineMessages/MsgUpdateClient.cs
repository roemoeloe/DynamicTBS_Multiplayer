using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class MsgUpdateClient : OnlineMessage
{
    public bool isAdmin;
    public PlayerType side;

    public MsgUpdateClient() // Constructing a message.
    {
        Code = OnlineMessageCode.UPDATE_CLIENT;
    }

    public MsgUpdateClient(DataStreamReader reader) // Receiving a message.
    {
        Code = OnlineMessageCode.UPDATE_CLIENT;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer, int lobbyId)
    {
        base.Serialize(ref writer, lobbyId);
        writer.WriteByte(ToByte(isAdmin));
        writer.WriteByte((byte)side);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        base.Deserialize(reader);
        isAdmin = ToBool(reader.ReadByte());
        side = (PlayerType)reader.ReadByte();
    }

    public override void ReceivedOnClient()
    {
        OnlineClient.Instance.UpdateClient(OnlineClient.Instance.LobbyId, isAdmin, side);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        OnlineServer.Instance.AssignSides(LobbyId, cnn, side);
    }
}
