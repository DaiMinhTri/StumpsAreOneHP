using HarmonyLib;
using UnityEngine;

namespace StumpsAreOneHp;

public static class StumpHp
{
    private static readonly string[] StumpPrefabs =
    {
        "Beech_Stub",
        "BirchStub",
        "FirTree_Stub",
        "OakStub",
        "Pinetree_01_Stub",
        "SwampTree1_Stub",
        "AshlandsTreeStump1",
        "AshlandsTreeStump2",
        "AshlandsTreeStump3"
    };

    [HarmonyPatch(typeof(ZNetScene), "Awake")]
    public static class ZNetScene_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ZNetScene __instance)
        {
            if (!__instance)
            {
                return;
            }
            foreach (string prefabName in StumpPrefabs)
            {
                Stumped(__instance, prefabName);
            }
        }

        private static void Stumped(ZNetScene instance, string prefabName)
        {
            GameObject prefab = instance.GetPrefab(prefabName);
            if (!prefab)
            {
                StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogWarning($"Prefab not found: {prefabName} (skipped)");
                return;
            }
            if (prefab.TryGetComponent<Destructible>(out Destructible destructible))
            {
                destructible.m_health = 1f;
            }
            else
            {
                StumpsAreOneHpPlugin.StumpsAreOneHpLogger.LogWarning($"No Destructible on {prefabName} (skipped)");
            }
        }
    }
}
