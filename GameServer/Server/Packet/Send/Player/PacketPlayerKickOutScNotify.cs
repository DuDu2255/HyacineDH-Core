using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Player;

public class PacketPlayerSqueezedScNotify : BasePacket
{
    public PacketPlayerSqueezedScNotify() : base(CmdIds.FightKickOutScNotify)
    {
        var proto = new PlayerKickOutScNotify
        {
            KickType = KickType.KickSqueezed
        };
        SetData(proto);
    }

    public PacketPlayerSqueezedScNotify(KickType type, BlackInfo? info = null) : base(CmdIds.PlayerSqueezedScNotify)
    {
        var proto = new PlayerKickOutScNotify
        {
            KickType = type
        };

        if (info != null) proto.BlackInfo = info;

        var proto = new FightKickOutScNotify();
        SetData(proto);
    }
}
