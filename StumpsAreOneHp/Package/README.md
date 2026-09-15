# Stumps Are 1 HP

Tree stumps have 1 HP — clean up your stumps with a single hit.

Fork of [coemt/StumpsAreOneHp](https://thunderstore.io/c/valheim/p/coemt/StumpsAreOneHp/) 0.0.1, updated for **Valheim 1.0.12**.

## What it does

After the world loads, six tree-stump prefabs get their health set to 1:

- Beech_Stub
- BirchStub
- FirTree_Stub
- OakStub
- Pinetree_01_Stub
- SwampTree1_Stub

One hit from anything chops the stump. Works on newly spawned and already-placed stumps alike, in single-player and multiplayer (server + clients should run the mod).

## Compatibility

- **Valheim 1.0.12** — code verified against the 1.0.12 assemblies (all accessed members still public; version-check handshake and config sync use the same ServerSync implementation as other updated mods).
- **Haven't tested in-game yet** on 1.0.12 — build is code-verified, awaiting gameplay confirmation.
- Config: `BepInEx/config/coemt.StumpsAreOneHp.cfg` — includes a server-admin lock option (synced).

## Credits

- Original mod: **coemt** (0.0.1, Dec 2023)
- 1.0.12 fork: **DaiMinhTri**

## Changelog

- **0.0.2** — Rebuilt for Valheim 1.0.12: ServerSync implementation updated, warning logs for missing prefabs, no functional change to stump HP.
- **0.0.1** — Original release by coemt.
