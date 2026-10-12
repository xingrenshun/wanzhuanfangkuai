using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MoreCoinPopup : MonoBehaviour
{
	public Canvas mainCanvas;
	public Button bWatchAds;
	//public TextMeshProUGUI tWatchAds, tDescription;
	public static MoreCoinPopup instance;
	public GameObject popup;

	void Awake ()
	{
		
		instance = this;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

	void SceneManager_sceneLoaded (Scene scene, LoadSceneMode loadSceneMode)
	{
		mainCanvas.worldCamera = Camera.main;
	}

	public void ShowPopup ()
	{
		//tWatchAds.text = GameManager.gameTextsDic [GameManager.dataSave.language].watch;
		//string earnCoins = GameManager.gameTextsDic [GameManager.dataSave.language].earnCoins;
		//earnCoins = earnCoins.Replace ("{X}", GameConfig.videoRewardCoin.ToString ());
		//tDescription.text = earnCoins;
		popup.SetActive (true);
		Time.timeScale = 0;
	}

	public void Close ()
	{
		popup.SetActive (false);
		Time.timeScale = 1;
	}

	public void WatchAds ()
	{
		//Admob.instance.ShowRewardedAd (VideoAdsRewardType.AD_REWARD_MORE_COIN);
		TouchWatchAds();

    }

	public void OnRewarded ()
	{
		GameManager.dataSave.coin += 15;
		string sceneName = SceneManager.GetActiveScene ().name;
		switch (sceneName) {
		case "scn_selectLevel":
			SelectLevelControler.instance.coin = GameManager.dataSave.coin;
			break;
		case "scn_play": 
			GameplayControl.instance.coinCount += 15;
			break;
		case "scn_home":
			TextMeshProUGUI homeCoinText = GameObject.Find("tCoinValue (1)")?.GetComponent<TextMeshProUGUI>();
			if (homeCoinText != null)
				homeCoinText.text = GameManager.dataSave.coin.ToString();
			else
				Debug.LogWarning("[MoreCoinPopup] 首页未找到 tCoinValue (1) 金币文本，显示未刷新");
			break;
        }
		GameManager.SaveData ();
	}


    public void TouchWatchAds()
    {
		//AdManager.Instance.ShowRewardedVideoAd(OnAdWatched, OnAdError);
		GuangGao.Instance.ShowReward(() =>
		{
			OnAdWatched();
		});
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
