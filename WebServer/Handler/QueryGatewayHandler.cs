using HyacineCore.Server.Configuration;
using HyacineCore.Server.Data;
using HyacineCore.Server.Internationalization;
using HyacineCore.Server.Proto;
using HyacineCore.Server.Util;
using HyacineCore.Server.WebServer.Request;
using Google.Protobuf;

namespace HyacineCore.Server.WebServer.Handler;

internal class QueryGatewayHandler
{
    public static Logger Logger = new("GatewayServer");
    private static bool GatewayDebugEnabled => ConfigManager.Config.ServerOption.LogOption.EnableGamePacketLog;
    private static void Debug(string message)
    {
        if (GatewayDebugEnabled)
            Logger.Debug(message);
    }

    public string Data;

    public QueryGatewayHandler(GateWayRequest req)
    {
        var config = ConfigManager.Config;

        var isNewFormat = string.Equals(req.is_new_format, "1", StringComparison.Ordinal);
        Debug($"query_gateway begin: version={req.version} uid={req.uid} lang={req.language_type} platform={req.platform_type} channel={req.channel_id}/{req.sub_channel_id} is_new_format={isNewFormat}");

        var gateServer = new GateServer
        {
            RegionName = config.GameServer.GameServerId,
            Ip = config.GameServer.PublicAddress,
            Port = config.GameServer.Port,
            Msg = "Access verification failed. Please check if you have logged in to the correct account and server.",
        };

        gateServer.Unk1 = true;
        gateServer.Unk2 = true;
        gateServer.Unk3 = true;
        gateServer.Unk4 = true;
        gateServer.Unk5 = true;
        gateServer.Unk6 = true;
        gateServer.Unk7 = true;
        gateServer.Unk8 = true;
        gateServer.Unk9 = true;
        gateServer.MdkResVersion = "0";
        gateServer.IfixVersion = "0";
        if (ConfigManager.Config.GameServer.UsePacketEncryption)
            gateServer.ClientSecretKey = Convert.ToBase64String(Crypto.ClientSecretKey!.GetBytes());

        var baseUrl = req.version.StartsWith("CN", StringComparison.OrdinalIgnoreCase) ? BaseUrl.CN : BaseUrl.OS;

        var remoteHotfixSuccess = false;
        if (ConfigManager.Config.HttpServer.SendHotfix && ConfigManager.Config.HttpServer.UseFetchRemoteHotfix)
        {
            remoteHotfixSuccess = FetchRemoteHotfix(req, gateServer).GetAwaiter().GetResult();
        }

        if (ConfigManager.Config.HttpServer.SendHotfix)
        {
            if (!remoteHotfixSuccess) UseLocalHotfix(req, baseUrl, gateServer);
        }
        else
        {
            SetEmptyHotfix(gateServer);
        }

        if (!ResourceManager.IsLoaded)
        {
            Logger.Warn("query_gateway requested before ResourceManager finished loading; returning retcode=0 for client compatibility");
        }

        Logger.Info("Client request: query_gateway");

        var bytes = gateServer.ToByteArray();
        Data = Convert.ToBase64String(bytes);

        Debug($"query_gateway result: protoBytes={bytes.Length}, base64Length={Data.Length}, retcode={gateServer.Retcode}");
        Debug($"query_gateway gate: region={gateServer.RegionName} ip={gateServer.Ip} port={gateServer.Port} encryption={(gateServer.ClientSecretKey?.Length ?? 0) > 0}");
        Debug($"query_gateway hotfix: ab={gateServer.AssetBundleUrl} exRes={gateServer.ExResourceUrl} lua={gateServer.LuaUrl} ifix={gateServer.IfixUrl}");
    }

    private async Task<bool> FetchRemoteHotfix(GateWayRequest req, GateServer gateServer)
    {
        try
        {
            var gatewayUrl = GetGatewayUrlByVersion(req.version);
            var queryParams = new Dictionary<string, string>
            {
                ["version"] = req.version,
                ["t"] = req.t,
                ["uid"] = req.uid,
                ["language_type"] = req.language_type,
                ["platform_type"] = req.platform_type,
                ["dispatch_seed"] = req.dispatch_seed,
                ["channel_id"] = req.channel_id,
                ["sub_channel_id"] = req.sub_channel_id,
                ["is_need_url"] = req.is_need_url,
                ["game_version"] = req.game_version,
                ["account_type"] = req.account_type,
                ["account_uid"] = req.account_uid
            };

            var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={kv.Value}"));
            var fullUrl = $"{gatewayUrl}?{queryString}";

            var (statusCode, response) = await HttpNetwork.SendGetRequest(fullUrl, 5);

            if (statusCode == 200 && !string.IsNullOrEmpty(response))
            {
                try
                {
                    var bytes = Convert.FromBase64String(response);
                    var remoteGateServer = GateServer.Parser.ParseFrom(bytes);

                    if (!string.IsNullOrEmpty(remoteGateServer.AssetBundleUrl))
                    {
                        gateServer.AssetBundleUrl = remoteGateServer.AssetBundleUrl;
                        gateServer.AssetBundleUrlAndroid = remoteGateServer.AssetBundleUrlAndroid;
                        gateServer.ExResourceUrl = remoteGateServer.ExResourceUrl;
                        gateServer.LuaUrl = remoteGateServer.LuaUrl;
                        gateServer.IfixUrl = remoteGateServer.IfixUrl;
                        return true;
                    }

                    Logger.Warn("Remote hotfix return empty, fall back to local hotfix");
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Failed to parse remote hotfix response: {ex.Message}");
                }
            }
            else
            {
                Logger.Warn($"Remote hotfix request failed with status: {statusCode}");
            }
        }
        catch (Exception ex)
        {
            Logger.Warn($"Remote hotfix fetch failed: {ex.Message}");
        }

        return false;
    }

    private void UseLocalHotfix(GateWayRequest req, string baseUrl, GateServer gateServer)
    {
        ConfigManager.Hotfix.HotfixData.TryGetValue(req.version, out var urls);

        if (urls != null)
        {
            gateServer.AssetBundleUrl = NormalizeHotfixUrl(baseUrl, urls.AssetBundleUrl);
            gateServer.AssetBundleUrlAndroid = NormalizeHotfixUrl(baseUrl, urls.AssetBundleUrlB);
            gateServer.ExResourceUrl = NormalizeHotfixUrl(baseUrl, urls.ExResourceUrl);
            gateServer.LuaUrl = NormalizeHotfixUrl(baseUrl, urls.LuaUrl);
            gateServer.IfixUrl = NormalizeHotfixUrl(baseUrl, urls.IfixUrl);
        }
        else
        {
            Logger.Warn($"No local hotfix found for version: {req.version}");
        }
    }

    private static string NormalizeHotfixUrl(string baseUrl, string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return value;
        }

        if (value.StartsWith("/", StringComparison.Ordinal)) return baseUrl.TrimEnd('/') + value;
        return baseUrl.TrimEnd('/') + "/" + value;
    }

    private static void SetEmptyHotfix(GateServer gateServer)
    {
        gateServer.AssetBundleUrl = string.Empty;
        gateServer.AssetBundleUrlAndroid = string.Empty;
        gateServer.ExResourceUrl = string.Empty;
        gateServer.LuaUrl = string.Empty;
        gateServer.IfixUrl = string.Empty;
    }

    private string GetGatewayUrlByVersion(string version)
    {
        if (version.Contains("CNPROD", StringComparison.OrdinalIgnoreCase))
        {
            return GateWayBaseUrl.CNPROD;
        }
        else if (version.Contains("CNBETA", StringComparison.OrdinalIgnoreCase))
        {
            return GateWayBaseUrl.CNBETA;
        }
        else if (version.Contains("OSPROD", StringComparison.OrdinalIgnoreCase))
        {
            return GateWayBaseUrl.OSPROD;
        }
        else if (version.Contains("OSBETA", StringComparison.OrdinalIgnoreCase))
        {
            return GateWayBaseUrl.OSBETA;
        }

        var region = version[..2];
        return region.Equals("CN", StringComparison.OrdinalIgnoreCase) ? GateWayBaseUrl.CNPROD : GateWayBaseUrl.OSPROD;
    }
}

