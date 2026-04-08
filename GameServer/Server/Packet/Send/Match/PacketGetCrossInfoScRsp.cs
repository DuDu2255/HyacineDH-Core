using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Match;

public class PacketGetCrossInfoScRsp : BasePacket
{
    public PacketGetCrossInfoScRsp() : base(CmdIds.GetCrossInfoScRsp)
    {
        var proto = new GetCrossInfoScRsp
        {
            FightGameMode = (FightGameMode)0
        };

        SetData(proto);
    }
}
