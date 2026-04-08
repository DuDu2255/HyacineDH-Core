using HyacineCore.Server.GameServer.Game.Player;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.BattleCollege;

public class PacketBattleCollegeDataChangeScNotify : BasePacket
{
    public PacketBattleCollegeDataChangeScNotify(PlayerInstance player) : base(CmdIds.SyncBattleCollegeDataChangeScNotify)
    {
        var proto = new SyncBattleCollegeDataChangeScNotify();

        foreach (var id in player.BattleCollegeData?.FinishedCollegeIdList ?? [])
            proto.FinishedCollegeIdList.Add((uint)id);

        SetData(proto);
    }
}
