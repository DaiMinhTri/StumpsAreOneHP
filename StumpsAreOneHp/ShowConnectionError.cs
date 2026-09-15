using HarmonyLib;
using TMPro;

namespace StumpsAreOneHp;

[HarmonyPatch(typeof(FejdStartup), "ShowConnectError")]
public static class ShowConnectionError
{
    private static void Postfix(FejdStartup __instance)
    {
        if (__instance.m_connectionFailedPanel.activeSelf)
        {
            __instance.m_connectionFailedError.fontSizeMax = 25f;
            __instance.m_connectionFailedError.fontSizeMin = 15f;
            __instance.m_connectionFailedError.text += "\n" + StumpsAreOneHpPlugin.ConnectionError;
        }
    }
}
