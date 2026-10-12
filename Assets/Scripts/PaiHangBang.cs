using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapTapMiniGame;

/// <summary>
/// TapTap 排行榜/分享功能实现类
/// 提供分数上传、排行榜显示、游戏分享功能
/// </summary>
public class PaiHangBang : MonoBehaviour
{
    // TODO: 在 TapTap 开发者后台创建排行榜后，替换为真实的排行榜 ID
    public string leaderboardId = "your_leaderboard_id";

    // TODO: 在 TapTap 开发者后台创建分享模板后，替换为真实的模板 ID
    public string shareTemplateId = "your_share_template_id";

    // 测试用的上传分数按钮
    public Button btn_UploadScore;
    // 排行榜面板
    public GameObject rankPanel;
    // 分享按钮
    public Button btn_Share;

    // 测试用的分数
    private int testScore = 100;

    private void Start()
    {
        if (btn_UploadScore != null)
            btn_UploadScore.onClick.AddListener(() => UploadScore(testScore++));

        if (btn_Share != null)
            btn_Share.onClick.AddListener(ShareGame);
    }

    /// <summary>
    /// 打开排行榜面板
    /// </summary>
    public void OpenRankPanel()
    {
        ShowRankList();
    }

    /// <summary>
    /// 检查登录态，当前实现直接回调（后续可按需接入 TapTap Login）
    /// </summary>
    public void CheckSession(bool callShow = false, Action callback = null)
    {
        if (callShow)
        {
            callback?.Invoke();
        }
    }

    /// <summary>
    /// 上传分数到 TapTap 排行榜
    /// </summary>
    /// <param name="score">要上传的分数</param>
    public void UploadScore(int score)
    {
        if (string.IsNullOrEmpty(leaderboardId) || leaderboardId.Contains("your_"))
        {
            Debug.LogWarning("[PaiHangBang] 请先配置 TapTap 排行榜 ID (leaderboardId)");
            return;
        }

#if UNITY_WEBGL || UNITY_MINIGAME || WEIXINMINIGAME
        try
        {
            CheckSession(true, () =>
            {
                Tap.SubmitScore(new SubmitScoreOption
                {
                    scores = new List<ScoreItem>
                    {
                        new ScoreItem
                        {
                            leaderboardId = leaderboardId,
                            score = score
                        }
                    },
                    callback = new LeaderboardCallback<SubmitScoresResponse>
                    {
                        onSuccess = (response) =>
                        {
                            Debug.Log("[PaiHangBang] 上传分数成功");
                        },
                        onFailure = (code, message) =>
                        {
                            Debug.LogWarning($"[PaiHangBang] 上传分数失败: {code} - {message}");
                        }
                    }
                });
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"[PaiHangBang] 上传分数时发生错误：{e.Message}");
        }
#else
        Debug.Log($"[PaiHangBang] 上传分数 {score}（仅在 WebGL/小游戏平台生效，当前平台不执行）");
#endif
    }

    /// <summary>
    /// 显示 TapTap 排行榜
    /// </summary>
    /// <param name="rankType">排行榜类型，1表示总榜，2表示好友榜</param>
    public void ShowRankList(int rankType = 1)
    {
        if (string.IsNullOrEmpty(leaderboardId) || leaderboardId.Contains("your_"))
        {
            Debug.LogWarning("[PaiHangBang] 请先配置 TapTap 排行榜 ID (leaderboardId)");
            return;
        }

#if UNITY_WEBGL || UNITY_MINIGAME || WEIXINMINIGAME
        try
        {
            CheckSession(true, () =>
            {
                Tap.OpenLeaderboard(new OpenLeaderboardOption
                {
                    leaderboardId = leaderboardId,
                    collection = rankType == 1 ? "public" : "friends",
                    callback = new OpenLeaderboardCallback
                    {
                        onSuccess = (msg) => Debug.Log("[PaiHangBang] 打开排行榜成功"),
                        onFailure = (code, message) => Debug.LogWarning($"[PaiHangBang] 打开排行榜失败: {code} - {message}")
                    }
                });
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"[PaiHangBang] 获取排行榜时发生错误：{e.Message}");
        }
#else
        Debug.Log("[PaiHangBang] 打开排行榜（仅在 WebGL/小游戏平台生效，当前平台不执行）");
#endif
    }

    /// <summary>
    /// 分享功能实现
    /// </summary>
    public void ShareGame()
    {
        if (string.IsNullOrEmpty(shareTemplateId) || shareTemplateId.Contains("your_"))
        {
            Debug.LogWarning("[PaiHangBang] 请先配置 TapTap 分享模板 ID (shareTemplateId)");
            return;
        }

#if UNITY_WEBGL || UNITY_MINIGAME || WEIXINMINIGAME
        try
        {
            Debug.Log("[PaiHangBang] 准备分享游戏");

            Tap.ShowShareboard(new ShowShareboardOption
            {
                templateId = shareTemplateId,
                sceneParam = "source=share",
                success = (res) => Debug.Log("[PaiHangBang] 分享成功"),
                fail = (res) => Debug.LogWarning($"[PaiHangBang] 分享失败: {res.errMsg}"),
                complete = (res) => Debug.Log("[PaiHangBang] 分享完成")
            });
        }
        catch (Exception e)
        {
            Debug.LogError("[PaiHangBang] 分享游戏时发生错误：" + e.Message);
        }
#else
        Debug.Log("[PaiHangBang] 分享游戏（仅在 WebGL/小游戏平台生效，当前平台不执行）");
#endif
    }
}
