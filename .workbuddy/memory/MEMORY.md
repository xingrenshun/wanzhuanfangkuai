# 项目长期记忆 — WanZhuanFangKuai（抖音→TapTap 迁移中）

## 平台定位
- 原抖音/TikTok 小游戏（WebGL + TTSDK），正在迁移到 **TapTap 小游戏**（WebGL 运行时，包 `com.taptap.minigame`）。
- **关键事实**：TapTap 小游戏的**唯一必接能力是「添加到桌面」(HomeScreenWidget)**，不是抖音的「侧边栏」。侧边栏是抖音的硬性要求，TapTap 没有。
- TapTap C# 命名空间 `TapTapMiniGame`：入口类 `Tap`，方法 `HasHomeScreenWidgetAndPinned` / `CreateHomeScreenWidget` / `SubmitScore` / `OpenLeaderboard` / `ShowShareboard`。
- 广告变现走 **Dirichlet (TapADN)**，0 分成、开发者拿 100%。

## 编译保护约定
- `Assets/AdSDK/Scripts/TikTokPlatform.cs` 与 `AdManager.cs` 中的抖音代码一律用 `#if TickTok` 包裹；`TickTok` 宏默认未定义，普通构建不编译。
- 改完 SDK 后让用户在 Unity 里刷新并看 Console 确认 0 error。

## 待办
- 填真实 TapTap `leaderboardId` / `shareTemplateId`。
- 用 Dirichlet 替换 `GuangGao.cs` 的广告桩。
- 确认上架目标（TapTap 小游戏 vs 原生 APK）再推进后续。
