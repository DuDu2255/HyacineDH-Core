using HyacineCore.Server.GameServer.Server.Packet.Send.ServerPrefs;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.ServerPrefs;

//[Opcode(CmdIds.GetServerPrefsDataCsReq)] //没有找到这个 Cmdid，暂时注释掉
public class HandlerGetServerPrefsDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetServerPrefsDataCsReq.Parser.ParseFrom(data);

        var info = connection.Player!.ServerPrefsData?.ServerPrefsDict.GetValueOrDefault((int)req.ServerPrefsId);

        await connection.SendPacket(new PacketGetServerPrefsDataScRsp(info, req.ServerPrefsId));
    }
}