using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Mission;

public class PacketMissionAcceptScNotify : BasePacket
{
    public PacketMissionAcceptScNotify(int missionId) : this([missionId])
    {
    }

    public PacketMissionAcceptScNotify(List<int> missionIds) : base(CmdIds.MainMissionAcceptNotify)
    {
        var proto = new MainMissionAcceptNotify();
        foreach (var missionId in missionIds) proto.SubMissionIdList.Add((uint)missionId);

        SetData(proto);
    }
}
