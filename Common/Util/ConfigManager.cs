using HyacineCore.Server.Configuration;
using HyacineCore.Server.Internationalization;
using Newtonsoft.Json;

namespace HyacineCore.Server.Util;

public static class ConfigManager
{
    public static readonly Logger Logger = new("ConfigManager");
    private static readonly string ConfigFilePath = Path.Combine("Config", "ServerConfig.json");
    private static string HotfixFilePath => Config.Path.ConfigPath + "/Hotfix.json";
    public static ConfigContainer Config { get; private set; } = new();
    public static HotfixContainer Hotfix { get; private set; } = new();

    public static void LoadConfig()
    {
        LoadConfigData();
        LoadHotfixData();
    }

    private static void LoadConfigData()
    {
        var file = new FileInfo(ConfigFilePath);
        if (!file.Exists)
        {
            var legacyFile = new FileInfo("Config.json");
            if (legacyFile.Exists)
            {
                if (file.Directory is { Exists: false }) file.Directory.Create();
                legacyFile.MoveTo(file.FullName, true);
                file.Refresh();
            }
        }

        if (!file.Exists)
        {
            Config = new ConfigContainer
            {
                MuipServer =
                {
                    AdminKey = Guid.NewGuid().ToString()
                },
                ServerOption =
                {
                    Language = UtilTools.GetCurrentLanguage()
                }
            };

            Logger.Info("Current Language is " + Config.ServerOption.Language);
            Logger.Info("Muipserver Admin key: " + Config.MuipServer.AdminKey);
            SaveData(Config, ConfigFilePath);
        }

        using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(stream))
        {
            var json = reader.ReadToEnd();
            Config = JsonConvert.DeserializeObject<ConfigContainer>(json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            })!;
        }

        GameConstants.ApplyChallengePeakConfig(Config.ServerOption.ChallengePeak);
        SaveData(Config, ConfigFilePath);
    }

    private static void LoadHotfixData()
    {
        var file = new FileInfo(HotfixFilePath);

        if (!file.Exists)
        {
            Hotfix = new HotfixContainer();
            SaveData(Hotfix.HotfixData, HotfixFilePath);
            Logger.Info(I18NManager.Translate("Server.ServerInfo.CurrentVersion", GameConstants.GAME_VERSION));
            return;
        }

        using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(stream))
        {
            var json = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<Dictionary<string, DownloadUrlConfig>>(json) ?? [];
            Hotfix = new HotfixContainer { HotfixData = data };
        }

        Logger.Info(I18NManager.Translate("Server.ServerInfo.CurrentVersion", GameConstants.GAME_VERSION));
    }

    private static void SaveData(object data, string path)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        var file = new FileInfo(path);
        if (file.Directory is { Exists: false }) file.Directory.Create();
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        using var writer = new StreamWriter(stream);
        writer.Write(json);
    }

    public static void InitDirectories()
    {
        foreach (var property in Config.Path.GetType().GetProperties())
        {
            var dir = property.GetValue(Config.Path)?.ToString();

            if (!string.IsNullOrEmpty(dir))
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
        }
    }
}
