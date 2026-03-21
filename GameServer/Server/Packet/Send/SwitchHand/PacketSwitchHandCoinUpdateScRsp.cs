using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.SwitchHand;

public class PacketSwitchHandCoinUpdateScRsp : BasePacket
{
    // 没有找到这个消息的 proto 定义
    public PacketSwitchHandCoinUpdateScRsp(Retcode ret) : base(CmdIds.None)
    {
        //var proto = new PJGAKDEDHAH
        //{
        //    Retcode = (uint)ret
        //};
        //SetData(proto);
    }

    public PacketSwitchHandCoinUpdateScRsp(uint coinNum) : base(CmdIds.None)
    {
        //var proto = new PJGAKDEDHAH
        //{
        //    JMMIHOEDFCG = coinNum
        //};
        //SetData(proto);
    }
}
