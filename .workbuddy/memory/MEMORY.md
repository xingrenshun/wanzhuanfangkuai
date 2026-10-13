# 项目长期记忆 — WanZhuanFangKuai（抖音→TapTap 迁移中）

## 平台定位
- 原抖音/TikTok 小游戏（WebGL + TTSDK），正在迁移到 **TapTap 小游戏**（WebGL 运行时，包 `com.taptap.minigame`）。
- **关键事实**：TapTap 小游戏的**唯一必接能力是「添加到桌面」(HomeScreenWidget)**，不是抖音的「侧边栏」。侧边栏是抖音的硬性要求，TapTap 没有。
- TapTap C# 命名空间 `TapTapMiniGame`：入口类 `Tap`，方法 `HasHomeScreenWidgetAndPinned` / `CreateHomeScreenWidget` / `SubmitScore` / `OpenLeaderboard` / `ShowShareboard`。
- 广告变现走 **Dirichlet (TapADN)**，0 分成、开发者拿 100%。

## 编译保护约定
- `Assets/AdSDK/Scripts/TikTokPlatform.cs` 与 `AdManager.cs` 中的抖音代码一律用 `#if TickTok` 包裹；`TickTok` 宏默认未定义，普通构建不编译。
- 改完 SDK 后让用户在 Unity 里刷新并看 Console 确认 0 error。

## 工程副本与 Git 发布（重要）
- **同一游戏两个工作副本，别搞混**：
  - `D:\Unity\UnityXM\WanZhuanFangKuai` = 源码 / 新平台（TapTap）版，含 `taptap-minigame` + `tt-minigame`。
  - `D:\Unity\WanZhuanFangKuai` = **抖音版**（仅 `tt-minigame`，appid `tt6fd41969f1b3697802`）。
- 发布约定：**一个仓库、多分支区分平台版本**。仓库 `https://github.com/xingrenshun/wanzhuanfangkuai`
  - `main` = 源码（`8f75bdf`，来自 UnityXM 副本）
  - `抖音版` = 抖音副本（`5a2f30a` + `163f667`，独立历史，无共同祖先）
  - UnityXM 副本本地另有分支 `冰川裂隙探秘`
- 上架名 ↔ 工程映射：**《冰川裂隙探秘》= WanZhuanFangKuai 工程**（方块拼图消除，非忍者/矿坑）。
- 上传注意：两副本 `simhei SDF.asset` 字节一致（LFS oid `7f6def82…`），同仓库只存一份；排除 `/tt-minigame/`、`/taptap-minigame/`、`/log/`、`._*`；网络时好时坏——直连与代理都可能失败，两个都试，并设 `lfs.locksverify false`。

## 待办
- 填真实 TapTap `leaderboardId` / `shareTemplateId`。
- 用 Dirichlet 替换 `GuangGao.cs` 的广告桩。
- 确认上架目标（TapTap 小游戏 vs 原生 APK）再推进后续。
