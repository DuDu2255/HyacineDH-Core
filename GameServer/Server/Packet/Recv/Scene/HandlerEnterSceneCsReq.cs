using HyacineCore.Server.GameServer.Server.Packet.Send.Scene;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.EnterSceneCsReq)]
public class HandlerEnterSceneCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = EnterSceneCsReq.Parser.ParseFrom(data);
        var storyLineId = (int)(req.SceneIdentifier?.GameStoryLineId ?? 0);
        var overMapTp = await connection.Player!.EnterScene((int)req.EntryId, (int)req.TeleportId, true,
            storyLineId, req.IsCloseMap);

        await connection.SendPacket(new PacketEnterSceneScRsp(overMapTp, req.IsCloseMap, storyLineId));
    }
}
