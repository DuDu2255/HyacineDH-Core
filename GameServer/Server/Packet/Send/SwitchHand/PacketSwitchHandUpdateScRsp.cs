using HyacineCore.Server.Database.Scene;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.SwitchHand;

public class PacketSwitchHandUpdateScRsp : BasePacket
{
    public PacketSwitchHandUpdateScRsp(SwitchHandInfo info, GODHDEIPDJL? operationInfo, HKLKGJCJJEB? actionInfo = null) : base(
        CmdIds.GetSwitchHandUpdateScRsp)
    {
        var proto = new GetSwitchHandUpdateScRsp
        {
            OMHAENBIKCN = operationInfo ?? info.ToSwitchHandProto(),
            CNPILGNBDNB = actionInfo ?? new HKLKGJCJJEB
            {
                GroupId = (uint)info.ConfigId,
                MNPPEEGEAEJ = info.State,
                NDHEAAKAPKK = info.CoinNum < 0 ? 0u : (uint)info.CoinNum
            }
        };
        SetData(proto);
    }

    public PacketSwitchHandUpdateScRsp(Retcode ret, GODHDEIPDJL? operationInfo, HKLKGJCJJEB? actionInfo = null) : base(
        CmdIds.GetSwitchHandUpdateScRsp)
    {
        var proto = new GetSwitchHandUpdateScRsp
        {
            Retcode = (uint)ret,
            OMHAENBIKCN = operationInfo ?? new GODHDEIPDJL(),
            CNPILGNBDNB = actionInfo ?? new HKLKGJCJJEB()
        };

        SetData(proto);
    }
}
