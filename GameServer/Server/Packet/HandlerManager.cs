using System.Reflection;

namespace HyacineCore.Server.GameServer.Server.Packet;

public static class HandlerManager
{
    public static Dictionary<int, Handler> handlers = [];

    public static void Init()
    {
        var classes = Assembly.GetExecutingAssembly().GetTypes(); // Get all classes in the assembly
        foreach (var cls in classes)
        {
            var attribute = (Opcode?)Attribute.GetCustomAttribute(cls, typeof(Opcode));

            if (attribute == null) continue;

            // Allow aliases that map to the same opcode; last discovered handler wins.
            handlers[attribute.CmdId] = (Handler)Activator.CreateInstance(cls)!;
        }
    }

    public static Handler? GetHandler(int cmdId)
    {
        try
        {
            return handlers[cmdId];
        }
        catch
        {
            return null;
        }
    }
}
