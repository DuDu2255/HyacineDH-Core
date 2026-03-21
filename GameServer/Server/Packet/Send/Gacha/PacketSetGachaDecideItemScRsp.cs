using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.Gacha;

public class PacketSetGachaDecideItemScRsp : BasePacket
{
    public PacketSetGachaDecideItemScRsp(uint gachaId, List<uint> order) : base(CmdIds.SetGachaDecideItemScRsp)
    {
        var proto = new SetGachaDecideItemScRsp
        {
            LECPJJAMNPF = new OEIEJHBCOOM
            {
                MBOEFLAHLEM = gachaId,
                DGOMHDMJHEK = { order },
                PAPOKACIPPJ = 1,
                IPLLMNPANID = { 11 }
            }
        };

        SetData(proto);
    }
}
