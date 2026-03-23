using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Scene;

public class PacketEnterSceneScRsp : BasePacket
{
    public PacketEnterSceneScRsp(bool overMapTp = false, bool tpByMap = false, int storyLineId = 0) : base(
        CmdIds.EnterSceneScRsp)
    {
        var proto = new EnterSceneScRsp
        {
            SceneIdentifier = new SceneIdentifier
            {
                GameStoryLineId = (uint)storyLineId,
            },
            
            IsCloseMap = tpByMap,
            IsOverMap = overMapTp
        };

        SetData(proto);
    }

    public PacketEnterSceneScRsp(Retcode retcode) : base(
        CmdIds.EnterSceneScRsp)
    {
        var proto = new EnterSceneScRsp
        {
            Retcode = (uint)retcode
        };

        SetData(proto);
    }
}