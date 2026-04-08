using HyacineCore.Server.Kcp;
using HyacineCore.Server.Proto;

namespace HyacineCore.Server.GameServer.Server.Packet.Recv.Chat;

[Opcode(CmdIds.SendMsgCsReq)]
public class HandlerSendMsgCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SendMsgCsReq.Parser.ParseFrom(data);
        var nestedChatData = req.MessageDatas?.ChatData;

        string? text = null;
        if (nestedChatData?.HasMessageText == true)
            text = nestedChatData.MessageText;

        text = text?.Trim('\0').Trim();

        var extraId = 0u;
        if (nestedChatData?.HasExtraId == true)
            extraId = nestedChatData.ExtraId;

        var msgType = MsgType.None;
        if (req.MessageDatas != null && req.MessageDatas.MessageType != MsgType.None)
            msgType = req.MessageDatas.MessageType;
        else if (!string.IsNullOrWhiteSpace(text))
            msgType = MsgType.CustomText;
        else if (extraId != 0)
            msgType = MsgType.Emoji;

        if (req.TargetList.Count == 0)
        {
            await connection.SendPacket(CmdIds.SendMsgScRsp);
            return;
        }

        foreach (var targetUid in req.TargetList)
        {
            if (msgType == MsgType.CustomText)
                await connection.Player!.FriendManager!.SendMessage(connection.Player!.Uid, (int)targetUid, text);
            else if (msgType == MsgType.Emoji)
                await connection.Player!.FriendManager!.SendMessage(connection.Player!.Uid, (int)targetUid, null,
                    (int)extraId);
        }

        await connection.SendPacket(CmdIds.SendMsgScRsp);
    }
}
