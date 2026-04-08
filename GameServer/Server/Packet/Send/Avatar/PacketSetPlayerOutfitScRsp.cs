using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Avatar;

public class PacketSetPlayerOutfitScRsp : BasePacket
{
    public PacketSetPlayerOutfitScRsp(Retcode retcode = Retcode.RetSucc) : base(CmdIds.SetPlayerOutfitScRsp)
    {
        var proto = new SetPlayerOutfitScRsp
        {
            Retcode = (uint)retcode
        };

        SetData(proto);
    }
}
