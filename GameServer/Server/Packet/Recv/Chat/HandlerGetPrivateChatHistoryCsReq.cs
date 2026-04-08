using HyacineCore.Server.GameServer.Server.Packet.Send.Chat;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.Chat;

[Opcode(CmdIds.GetPrivateChatHistoryCsReq)]
public class HandlerGetPrivateChatHistoryCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetPrivateChatHistoryCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;
        var targetSide = req.TargetSide != 0 ? req.TargetSide : (uint)player.Uid;
        await connection.SendPacket(new PacketGetPrivateChatHistoryScRsp(targetSide, req.ContactSide, player));
    }
}
