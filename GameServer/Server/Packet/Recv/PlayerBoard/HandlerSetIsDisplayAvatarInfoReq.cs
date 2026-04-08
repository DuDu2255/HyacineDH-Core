using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.PlayerBoard;

// 某些版本会用非标准命名的 opcode（SetIsDisplayAvatarInfoReq）发起请求
[Opcode(CmdIds.SetIsDisplayAvatarInfoReq)]
public class HandlerSetIsDisplayAvatarInfoReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SetIsOHOMDMEJLFKReq.Parser.ParseFrom(data);

        var rsp = new SetIsOHOMDMEJLFKScRsp
        {
            Retcode = 0,
            IsDisplay = req.IsDisplay
        };

        var packet = new BasePacket(CmdIds.SetIsDisplayAvatarInfoScRsp);
        packet.SetData(rsp);
        await connection.SendPacket(packet);
    }
}

