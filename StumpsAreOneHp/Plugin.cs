using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ServerSync;
using UnityEngine;

namespace StumpsAreOneHp;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class StumpsAreOneHpPlugin : BaseUnityPlugin
{
    public enum Toggle
    {
        On = 1,
        Off = 0
    }

    internal const string ModName = "StumpsAreOneHp";
    internal const string ModVersion = "0.0.2";
    internal const string Author = "DaiMinhTri (fork of coemt 0.0.1)";
    private const string ModGUID = "coemt.StumpsAreOneHp";

    private static string ConfigFileName = ModGUID + ".cfg";
    private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

    internal static string ConnectionError = "";

    private readonly Harmony _harmony = new(ModGUID);

    private static ManualLogSource _logger;

    internal static ManualLogSource StumpsAreOneHpLogger => _logger;

    private static readonly ConfigSync ConfigSync = new(ModGUID)
    {
        DisplayName = ModName,
        CurrentVersion = ModVersion,
        MinimumRequiredVersion = "0.0.1"
    };

    private static ConfigEntry<Toggle> _serverConfigLocked = null;

    public void Awake()
    {
        _logger = BepInEx.Logging.Logger.CreateLogSource("StumpsAreOneHp");
        _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
            "If on, the configuration is locked and can be changed by server admins only.");
        ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

        _harmony.PatchAll(Assembly.GetExecutingAssembly());
        SetupWatcher();
    }

    private void OnDestroy()
    {
        Config.Save();
    }

    private void SetupWatcher()
    {
        FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName)
        {
            IncludeSubdirectories = true,
            SynchronizingObject = ThreadingHelper.SynchronizingObject
        };
        watcher.Changed += ReadConfigValues;
        watcher.Created += ReadConfigValues;
        watcher.Renamed += ReadConfigValues;
        watcher.EnableRaisingEvents = true;
    }

    private void ReadConfigValues(object sender, FileSystemEventArgs e)
    {
        if (!File.Exists(ConfigFileFullPath))
        {
            return;
        }
        try
        {
            StumpsAreOneHpLogger.LogDebug("ReadConfigValues called");
            Config.Reload();
        }
        catch
        {
            StumpsAreOneHpLogger.LogError($"There was an issue loading your {ConfigFileName}");
            StumpsAreOneHpLogger.LogError("Please check your config entries for spelling and format!");
        }
    }

    private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
    {
        ConfigDescription extended = new(
            description.Description + (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
            description.AcceptableValues,
            description.Tags);
        ConfigEntry<T> configEntry = Config.Bind(group, name, value, extended);
        SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
        syncedConfigEntry.SynchronizedConfig = synchronizedSetting;
        return configEntry;
    }

    private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true)
    {
        return config(group, name, value, new ConfigDescription(description, null, Array.Empty<object>()), synchronizedSetting);
    }
}
