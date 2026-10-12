using System.Collections;
using System.Collections.Generic;
using TapTapMiniGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CeBianLan : MonoBehaviour
{
    public bool IsDeleteData;
    //前往按钮
    public Button btn_Go;
    //侧边栏
    public GameObject ceBianLan;

#if UNITY_EDITOR
    private void Awake()
    {
        if (IsDeleteData)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
#endif

    private void Start()
    {
        btn_Go.onClick.AddListener(NavigateToHomeScreenWidget);
    }

    /// <summary>
    /// 跳转到 TapTap 桌面小组件（添加到桌面）
    /// TapTap 小游戏没有抖音的侧边栏，必接能力是桌面文件夹
    /// </summary>
    public void NavigateToHomeScreenWidget()
    {
#if UNITY_WEBGL || UNITY_MINIGAME || WEIXINMINIGAME
        try
        {
            Tap.HasHomeScreenWidgetAndPinned(new HasHomeScreenWidgetOption
            {
                success = (res) =>
                {
                    Debug.Log($"[HomeScreenWidget] hasWidget:{res.hasWidget}, isPinned:{res.isPinned}");
                    if (!res.hasWidget || !res.isPinned)
                    {
                        Tap.CreateHomeScreenWidget(new CreateHomeScreenWidgetOption
                        {
                            success = (r) => Debug.Log("创建桌面小组件成功"),
                            fail = (r) => Debug.Log($"创建桌面小组件失败: {r.errMsg}"),
                            complete = (r) => Debug.Log("创建桌面小组件完成") 
                        });
                    }
                },
                fail = (res) => Debug.Log($"[HomeScreenWidget] 查询失败: {res.errMsg}"),
                complete = (res) => Debug.Log("[HomeScreenWidget] 查询完成")
            });
        }
        catch (System.Exception e)
        {
            Debug.LogError("添加到桌面异常: " + e.Message);
        }
#else
        Debug.Log("[CeBianLan] 添加到桌面功能仅在 WebGL/小游戏平台生效，当前平台不执行");
#endif
    }
}
