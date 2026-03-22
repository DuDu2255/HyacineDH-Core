using HyacineCore.Server.Data;
using HyacineCore.Server.GameServer.Game.Player;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Avatar;

public class PacketGetAvatarDataScRsp : BasePacket
{
    public PacketGetAvatarDataScRsp(PlayerInstance player) : base(CmdIds.GetAvatarDataScRsp)
    {
        var proto = new GetAvatarDataScRsp
        {
            IsGetAll = true
        };

        player.PlayerUnlockData!.Skins.Values.ToList().ForEach(skin =>
            proto.SkinList.AddRange(skin.Select(x => (uint)x)));

        var basicTypeIds = new HashSet<uint>();

        foreach (var avatar in player.AvatarManager?.AvatarData?.FormalAvatars ?? Enumerable.Empty<HyacineCore.Server.Database.Avatar.FormalAvatarInfo>())
        {
            proto.AvatarList.Add(avatar.ToProto());

            var pathProtos = avatar.ToAvatarPathProto();
            proto.AvatarPathDataInfoList.AddRange(pathProtos);

            foreach (var path in pathProtos)
                if (GameData.MultiplePathAvatarConfigData.ContainsKey((int)path.AvatarId))
                    basicTypeIds.Add(path.AvatarId);
        }

        proto.BasicTypeIdList.AddRange(basicTypeIds);
        proto.CurAvatarPath.Add(GrowthTartgetFuncType.Iipiakjfgnh);

        SetData(proto);
    }
}
