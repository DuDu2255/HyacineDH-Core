using HyacineCore.Server.GameServer.Server.Packet.Send.Scene;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.Scene;

//[Opcode(CmdIds.SetGroupCustomSaveDataCsReq)] // 这个也没有找到
public class HandlerSetGroupCustomSaveDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SetGroupCustomSaveDataCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        player.SetCustomSaveData((int)req.EntryId, (int)req.GroupId, req.SaveData);
        await connection.SendPacket(new PacketSetGroupCustomSaveDataScRsp(req.EntryId, req.GroupId));
    }
}