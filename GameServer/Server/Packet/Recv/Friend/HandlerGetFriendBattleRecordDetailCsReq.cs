using HyacineCore.Server.Database;
using HyacineCore.Server.Database.Avatar;
using HyacineCore.Server.Database.Challenge;
using HyacineCore.Server.Database.Friend;
using HyacineCore.Server.GameServer.Server.Packet.Send.Friend;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.Friend;

//[Opcode(CmdIds.GetFriendBattleRecordDetailCsReq)]
public class HandlerGetFriendBattleRecordDetailCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetFriendBattleRecordDetailCsReq.Parser.ParseFrom(data);
        var uid = req.Uid;

        // get data from db
        var recordData = DatabaseHelper.Instance!.GetInstance<FriendRecordData>((int)uid);
        var challengeData = DatabaseHelper.Instance!.GetInstance<ChallengeData>((int)uid);
        var avatarData = DatabaseHelper.Instance!.GetInstance<AvatarData>((int)uid);

        if (recordData == null || challengeData == null || avatarData == null)
        {
            await connection.SendPacket(new PacketGetFriendBattleRecordDetailScRsp(Retcode.RetFriendPlayerNotFound));
            return;
        }

        await connection.SendPacket(new PacketGetFriendBattleRecordDetailScRsp(recordData, challengeData, avatarData));
    }
}