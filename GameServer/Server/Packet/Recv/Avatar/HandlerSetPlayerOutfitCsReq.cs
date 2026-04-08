using HyacineCore.Server.Data;
using HyacineCore.Server.Enums.Item;
using HyacineCore.Server.GameServer.Server.Packet.Send.Avatar;
using HyacineCore.Server.GameServer.Server.Packet.Send.PlayerSync;
using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.Avatar;

[Opcode(CmdIds.SetPlayerOutfitCsReq)]
public class HandlerSetPlayerOutfitCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = SetPlayerOutfitCsReq.Parser.ParseFrom(data);

        var outfitList = req.PlayerOutfitData?.PlayerOutfitList ?? [];
        var nextOutfits = new List<int>(outfitList.Count);

        foreach (var outfitId in outfitList)
        {
            var id = (int)outfitId;
            if (!GameData.ItemConfigData.TryGetValue(id, out var config) ||
                config.ItemSubType != ItemSubTypeEnum.PlayerOutfit)
            {
                await connection.SendPacket(new PacketSetPlayerOutfitScRsp(Retcode.RetPlayerOutfitConfigNotExist));
                return;
            }

            var item = player.InventoryManager!.GetItem(id, mainType: config.ItemMainType);
            if (item == null || item.Count <= 0)
            {
                await connection.SendPacket(new PacketSetPlayerOutfitScRsp(Retcode.RetPlayerOutfitNotOwned));
                return;
            }

            if (!nextOutfits.Contains(id))
                nextOutfits.Add(id);
        }

        player.Data.PlayerOutfitList = nextOutfits;

        await connection.SendPacket(new PacketSetPlayerOutfitScRsp());
        await player.SendPacket(new PacketPlayerSyncScNotify(player.Data.ToPlayerOutfitProto()));
    }
}
