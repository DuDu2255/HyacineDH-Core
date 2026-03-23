using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Player;

public class PacketPlayerKickOutScNotify : BasePacket
{
    public PacketPlayerKickOutScNotify() : base(CmdIds.PlayerSqueezedScNotify)
    {
        var proto = new PlayerSqueezedScNotify
        {
            KickType = KickType.KickSqueezed
        };
        SetData(proto);
    }

    public PacketPlayerKickOutScNotify(KickType type, BlackInfo? info = null) : base(CmdIds.PlayerSqueezedScNotify)
    {
        var proto = new PlayerSqueezedScNotify
        {
            KickType = type
        };

        if (info != null) proto.BlackInfo = info;

        SetData(proto);
    }
}