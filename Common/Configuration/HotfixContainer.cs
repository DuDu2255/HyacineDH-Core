using Newtonsoft.Json;

namespace HyacineCore.Server.Configuration;

public class HotfixContainer
{
    public Dictionary<string, DownloadUrlConfig> HotfixData { get; set; } = [];
}

public class DownloadUrlConfig
{
    [JsonProperty("asset_bundle_url")]
    public string AssetBundleUrl { get; set; } = "";

    [JsonProperty("asset_bundle_url_b")]
    public string AssetBundleUrlB { get; set; } = "";

    [JsonProperty("ex_resource_url")]
    public string ExResourceUrl { get; set; } = "";

    [JsonProperty("lua_url")]
    public string LuaUrl { get; set; } = "";

    [JsonProperty("ifix_url")]
    public string IfixUrl { get; set; } = "";
}

public static class GateWayBaseUrl
{
    public const string CNBETA = "https://beta-release01-cn.bhsr.com/query_gateway";
    public const string CNPROD = "https://prod-gf-cn-dp01.bhsr.com/query_gateway";
    public const string OSBETA = "https://beta-release01-asia.starrails.com/query_gateway";
    public const string OSPROD = "https://prod-official-asia-dp01.starrails.com/query_gateway";
}

public static class BaseUrl
{
    public const string CN = "https://autopatchcn.bhsr.com/";
    public const string OS = "https://autopatchos.starrails.com/";
}
