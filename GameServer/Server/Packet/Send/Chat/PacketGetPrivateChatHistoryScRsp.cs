using HyacineCore.Server.GameServer.Game.Player;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Chat;

public class PacketGetPrivateChatHistoryScRsp : BasePacket
{
    public PacketGetPrivateChatHistoryScRsp(uint targetSide, uint contactId, PlayerInstance player) : base(
        CmdIds.GetPrivateChatHistoryScRsp)
    {
        var proto = new GetPrivateChatHistoryScRsp
        {
            Retcode = 0,
            TargetSide = targetSide,
            ContactSide = contactId
        };

        var infos = player.FriendManager!.GetHistoryInfo((int)contactId);
        proto.ChatMessageList.AddRange(infos);

        SetData(proto);
    }
}
