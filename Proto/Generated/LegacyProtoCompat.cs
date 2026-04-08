namespace HyacineCore.Server.Proto;

// Minimal compatibility layer kept only for fields not yet renamed in proto.
public sealed partial class HandInfo
{
    // MHINKADJCCG -> old server field name
    public global::Google.Protobuf.ByteString HandByteValue
    {
        get => MHINKADJCCG;
        set => MHINKADJCCG = value;
    }

    // AMBLLFLFKHC -> old server field name
    public uint CoinNum
    {
        get => AMBLLFLFKHC;
        set => AMBLLFLFKHC = value;
    }

    // MotionInfo -> old server field name
    public MotionInfo? HandMotion
    {
        get => MotionInfo;
        set => MotionInfo = value;
    }

    // JLMJFEDNBMF -> old server field name
    public uint HandState
    {
        get => JLMJFEDNBMF;
        set => JLMJFEDNBMF = value;
    }
}

public sealed partial class SwitchHandCoinUpdateCsReq
{
    // JLMJFEDNBMF -> old server field name
    public uint CKNFABPOMBL
    {
        get => JLMJFEDNBMF;
        set => JLMJFEDNBMF = value;
    }
}

public sealed partial class SwitchHandCoinUpdateScRsp
{
    // JLMJFEDNBMF -> old server field name
    public uint CKNFABPOMBL
    {
        get => JLMJFEDNBMF;
        set => JLMJFEDNBMF = value;
    }
}

public sealed partial class SwitchHandResetTransformCsReq
{
    // MotionInfo -> old server field name
    public MotionInfo? PKGLJDIHGCC
    {
        get => MotionInfo;
        set => MotionInfo = value;
    }
}

public sealed partial class SwitchHandResetGameCsReq
{
    // ResetHandInfo already exists in proto after direct rename.
}
