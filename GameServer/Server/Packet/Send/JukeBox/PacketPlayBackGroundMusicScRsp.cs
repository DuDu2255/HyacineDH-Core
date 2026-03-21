using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Send.JukeBox;

public class PacketPlayBackGroundMusicScRsp : BasePacket
{
    public PacketPlayBackGroundMusicScRsp(uint musicId) : base(CmdIds.PlayBackGroundMusicScRsp)
    {
        var proto = new PlayBackGroundMusicScRsp
        {
            JILKOPINLDC = new MCPPIEJEBEF
            {
                LEKGBMCOJDB = new NKLAFFLJCHG
                {
                    Id = musicId
                }
            }
        };

        SetData(proto);
    }
}
