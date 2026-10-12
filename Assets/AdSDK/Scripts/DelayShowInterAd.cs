using System.Collections;
using UnityEngine;

/// <summary>
/// 游戏暂停后延时5秒展示插屏广告
/// </summary>
public class DelayShowInterAd : MonoBehaviour
{
    private Coroutine delay;

    private void OnEnable()
    {
        if (delay != null)
        {
            StopCoroutine(delay);
        }
        Debug.Log("游戏暂停，x秒后展示插屏广告");
        delay = StartCoroutine(ShowInterAd());
    }

    private void OnDisable()
    {
        if (delay != null)
        {
            Debug.Log("游戏继续，取消插屏广告展示");
            StopCoroutine(delay);
            delay = null;
        }
    }

   private IEnumerator ShowInterAd()
    {
        yield return new WaitForSecondsRealtime(30f);
        Debug.Log("计时结束，展示插屏广告");
        AdManager.Instance.ShowInterstitialAd();
    }
}
