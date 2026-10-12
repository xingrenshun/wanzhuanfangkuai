using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuangGao : Singleton<GuangGao>
{
    // TODO: 接入 Dirichlet (TapADN) 广告后，替换为真实的广告位 ID
    // private static string XPId = "your_interstitial_ad_unit_id";
    // private static string BannerId = "your_banner_ad_unit_id";
    // private static string rewardId = "your_rewarded_video_ad_unit_id";

    Action rewardAction;
    private float gameStartTime; // 游戏启动时间
    private float showXPTime;//插屏广告显示时间
    private float showRewardTime;//激励广告显示时间

    protected override void Awake()
    {
        base.Awake();
        gameStartTime = Time.time; // 记录游戏启动时间
        // TODO: 接入 Dirichlet 广告 SDK 后，在这里初始化广告
        // DirichletAdSdk.Init(...);
    }

    /// <summary>
    /// 检查游戏启动是否已经超过30秒
    /// </summary>
    /// <returns>如果游戏启动时间超过30秒返回true，否则返回false</returns>
    public bool IsGameStartedOver30Seconds()
    {
        return Time.time - gameStartTime >= 30f;
    }

    public bool IsShowXPStartedOver60Seconds()
    {
        if (showXPTime == 0) return true;
        return Time.time - showXPTime >= 60f;
    }

    public bool IsShowRewardStartedOver60Seconds()
    {
        if (showRewardTime == 0) return true;
        return Time.time - showRewardTime >= 60;
    }

    /// <summary>
    /// 预加载插屏广告（抖音 SDK 已移除，后续接入 Dirichlet）
    /// </summary>
    public void CreateXP()
    {
        Debug.Log("[GuangGao] CreateXP: 插屏广告待接入 Dirichlet");
    }

    public IEnumerator WaitShowXP()
    {
        yield return new WaitForSeconds(0.5f);
        GuangGao.Instance.ShowXP();
    }

    /// <summary>
    /// 展示插屏广告（抖音 SDK 已移除，后续接入 Dirichlet）
    /// </summary>
    public void ShowXP()
    {
        if (IsGameStartedOver30Seconds())
        {
            if (IsShowRewardStartedOver60Seconds())
            {
                if (IsShowXPStartedOver60Seconds())
                {
                    Debug.Log("[GuangGao] ShowXP: 插屏广告展示（待接入 Dirichlet）");
                    showXPTime = Time.time;
                }
                else
                {
                    Debug.Log("离打开插屏广告没到60s");
                }
            }
            else
            {
                Debug.Log("离打开激励广告没到60s");
            }
        }
        else
        {
            Debug.Log("30s没到");
        }
    }

    /// <summary>
    /// 创建 Banner 广告（抖音 SDK 已移除，后续接入 Dirichlet）
    /// </summary>
    public void CreatBanner()
    {
        Debug.Log("[GuangGao] CreatBanner: Banner 广告待接入 Dirichlet");
    }

    public void ShowBanner()
    {
        Debug.Log("[GuangGao] ShowBanner: Banner 广告展示（待接入 Dirichlet）");
    }

    public void CloseBanner()
    {
        Debug.Log("[GuangGao] CloseBanner: Banner 广告关闭（待接入 Dirichlet）");
    }

    /// <summary>
    /// 创建激励视频广告（抖音 SDK 已移除，后续接入 Dirichlet）
    /// </summary>
    public void CreateReward()
    {
        Debug.Log("[GuangGao] CreateReward: 激励视频广告待接入 Dirichlet");
    }

    /// <summary>
    /// 展示激励视频广告（抖音 SDK 已移除，后续接入 Dirichlet）
    /// 目前直接回调成功，用于保持原有调用链路可运行
    /// </summary>
    public void ShowReward(Action action)
    {
        Debug.Log("[GuangGao] ShowReward: 激励视频广告展示（待接入 Dirichlet），直接发放奖励");
        rewardAction = action;
        showRewardTime = Time.time;
        // 测试阶段直接发奖励，接入真实广告后改为看完再发
        rewardAction?.Invoke();
    }

    /// <summary>
    /// 上传分数到排行榜（抖音 SDK 已移除，后续接入 TapTap 排行榜）
    /// 当前转发给 PaiHangBang 处理
    /// </summary>
    public void UploadScore(int score, string zoneId = "default")
    {
        Debug.Log($"[GuangGao] UploadScore: {score}，待接入 TapTap 排行榜");
        // 保留原调用链路，实际排行榜逻辑请在 PaiHangBang.cs 中维护
    }
}
