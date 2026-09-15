using System.Collections.Generic;
using HarmonyLib;

namespace StumpsAreOneHp;

// Version handshake so clients without the mod (or an incompatible version) are
// rejected with a clear message instead of silently diverging on stump HP.
[HarmonyPatch(typeof(ZNet), "OnNewConnection")]
public static class RegisterAndCheckVersion
{
    private static void Prefix(ZNetPeer peer)
    {
        StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogDebug("Registering version RPC handler");
        peer.m_rpc.Register<ZPackage>("StumpsAreOneHp_VersionCheck", RpcHandlers.RPC_StumpsAreOneHp_Version);
        StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogDebug("Invoking version check");
        ZPackage pkg = new();
        pkg.Write(StumpsAreOneHpPlugin.ModVersion);
        peer.m_rpc.Invoke("StumpsAreOneHp_VersionCheck", new object[] { pkg });
    }
}

[HarmonyPatch(typeof(ZNet), "Disconnect")]
public static class RemoveDisconnectedPeerFromVerified
{
    private static void Prefix(ZNetPeer peer, ZNet __instance)
    {
        if (__instance.IsServer())
        {
            StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogInfo(
                $"Peer ({peer.m_rpc.m_socket.GetHostName()}) disconnected, removing from validated list");
            RpcHandlers.ValidatedPeers.Remove(peer.m_rpc);
        }
    }
}

public static class RpcHandlers
{
    public static readonly List<ZRpc> ValidatedPeers = new();

    public static void RPC_StumpsAreOneHp_Version(ZRpc rpc, ZPackage pkg)
    {
        string remote = pkg.ReadString();
        StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogInfo($"Version check, local: {StumpsAreOneHpPlugin.ModVersion}, remote: {remote}");
        if (remote != StumpsAreOneHpPlugin.ModVersion)
        {
            StumpsAreOneHpPlugin.ConnectionError = $"StumpsAreOneHp Installed: {StumpsAreOneHpPlugin.ModVersion}\n Needed: {remote}";
            if (ZNet.instance.IsServer())
            {
                StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogWarning(
                    $"Peer ({rpc.m_socket.GetHostName()}) has incompatible version, disconnecting...");
                rpc.Invoke("Error", new object[] { 3 });
            }
        }
        else if (!ZNet.instance.IsServer())
        {
            StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogInfo("Received same version from server!");
        }
        else
        {
            StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogInfo($"Adding peer ({rpc.m_socket.GetHostName()}) to validated list");
            ValidatedPeers.Add(rpc);
        }
    }
}
