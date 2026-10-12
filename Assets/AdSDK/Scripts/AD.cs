//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using TTSDK;
//using TTSDK.UNBridgeLib.LitJson;
//using UnityEngine;
//using UnityEngine.UI;

//public class AD : MonoBehaviour
//{
//    public static AD Instance { get; private set; }
//    public TextMeshProUGUI CoinNum;

//    void Awake()
//    {
//        // 如果实例已存在，则销毁当前实例
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        CoinNum.text = GameManager.dataSave.coin.ToString();
//    }
//    public void BtnSliderOnClick() {
//        AdSliderBar.Show(() => {
//            Debug.Log("领取侧边栏奖励");
//        });
//    }

//    public void BtnFreeCoinOnClick()
//    {
//        AdConfirm.Show(() => {
//            Debug.Log("领取奖励");
            
//        }, false);
//    }


//    public void TouchWatchAds()
//    {
//        //GameManager.dataSave.coin += 10;
//        //CoinNum.text = GameManager.dataSave.coin.ToString();
//        AdManager.Instance.ShowRewardedVideoAd(OnAdWatched, OnAdError);
//    }
//    public void ShowMoreCoinPopup ()
//	{
//		MoreCoinPopup.instance.ShowPopup ();
//	}

//    private void OnAdWatched()
//    {
//        Debug.Log("广告观看完成");
//        GameManager.dataSave.coin += 10;
//        CoinNum.text = GameManager.dataSave.coin.ToString();

//    }

//    private void OnAdError(string errorMessage)
//    {
//        Debug.LogError($"广告播放失败: {errorMessage}");
//        // 加载 AdError 预制体
//        GameObject adErrorPrefab = Resources.Load<GameObject>("AdError");
//        if (adErrorPrefab != null)
//        {
//            // 实例化 AdError
//            GameObject adErrorInstance = Instantiate(adErrorPrefab, transform.position, Quaternion.identity);
//            // 1 秒后销毁实例
//            Destroy(adErrorInstance, 1.5f);
//        }
//        else
//        {
//            Debug.LogError("AdError 预制体未找到，请确保其位于 Resources 文件夹中。");
//        }
//    }

//    public void TestShare()
//    {
//        // 通用分享
//        JsonData shareJson = new JsonData();
//        shareJson["channel"] = "video";
//        shareJson["title"] = "Some Title";
//        shareJson["extra"] = new JsonData();
//        shareJson["extra"]["videoPath"] = "/xxx/xxx.mp4";//录屏分享的话，路径是 OnRecordComplete 拿到的路径
//        JsonData videoTopics = new JsonData();
//        videoTopics.SetJsonType(JsonType.Array);
//        videoTopics.Add("Some Topic1");
//        videoTopics.Add("Some Topic2");
//        shareJson["extra"]["videoTopics"] = videoTopics;
//        shareJson["extra"]["hashtag_list"] = videoTopics;

//        Debug.Log($"ShareAppMessageBtnClicked jsonData: {videoTopics.ToJson()}");
//        TT.ShareAppMessage(videoTopics, (data) =>
//        {
//            Debug.Log($"ShareAppMessage success: {videoTopics.ToJson()}");//
//        },
//            (errMsg) =>
//            {
//                Debug.Log($"ShareAppMessage failed: {errMsg}");
//            },
//            () =>
//            {
//                Debug.Log($"ShareAppMessage cancel");
//            });
//    }


//}
