using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreHeartPopup : MonoBehaviour
{
	public static MoreHeartPopup instance;
	public Button bBuy;
	Color col = new Color (1, 1, 1, 1);
	Color colAlpha = new Color (1, 1, 1, 0.5F);
	public Text tNoLife, tFillLife, bBuyText, bWatchText;

	void Awake ()
	{
		tNoLife.text = GameManager.gameTextsDic [GameManager.dataSave.language].noLives;
		tFillLife.text = GameManager.gameTextsDic [GameManager.dataSave.language].payLives;
		bBuyText.text = GameConfig.heartPrice + " " + GameManager.gameTextsDic [GameManager.dataSave.language].buy; 
		bWatchText.text = GameManager.gameTextsDic [GameManager.dataSave.language].watch;
		instance = this;
	}

	public void Close ()
	{
		gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	void OnEnable ()
	{
		Time.timeScale = 0;
		if (GameManager.dataSave.coin < 50) {
			bBuy.image.color = colAlpha;
		} else {
			bBuy.image.color = col;
		}
	}

	public void WatchAd ()
	{
		//Admob.instance.ShowRewardedAd (VideoAdsRewardType.AD_REWARD_MORE_COIN);
		TouchWatchAds();

    }

	public void OnRewarded ()
	{
		GameManager.dataSave.heart++;
		SelectLevelControler.instance.InitHeart ();
        Close ();
	}

	public void Buy ()
	{
		if (GameManager.dataSave.coin > 50) {
			GameManager.dataSave.coin -= 50;
			GameManager.dataSave.heart++;
			SelectLevelControler.instance.InitHeart ();
            SelectLevelControler.instance.coin = GameManager.dataSave.coin;
			Close ();
		}
	}

    public void TouchWatchAds()
    {
       // AdManager.Instance.ShowRewardedVideoAd(OnAdWatched, OnAdError);激励广告
        OnRewarded();
    }

    private void OnAdWatched()
    {
        Debug.Log("广告观看完成");
        OnRewarded();
    }

    private void OnAdError(string errorMessage)
    {
        Debug.LogError($"广告播放失败: {errorMessage}");
        // 加载 AdError 预制体
        GameObject adErrorPrefab = Resources.Load<GameObject>("AdError");
        if (adErrorPrefab != null)
        {
            // 实例化 AdError
            GameObject adErrorInstance = Instantiate(adErrorPrefab, transform.position, Quaternion.identity);
            // 1 秒后销毁实例
            Destroy(adErrorInstance, 1.5f);
        }
        else
        {
            Debug.LogError("AdError 预制体未找到，请确保其位于 Resources 文件夹中。");
        }
    }

}
