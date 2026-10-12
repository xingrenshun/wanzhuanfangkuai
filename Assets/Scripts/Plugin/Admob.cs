using UnityEngine;
using System.Collections;
//using GoogleMobileAds.Api;


public class Admob : MonoBehaviour
{
	public static Admob instance;
	[SerializeField]
	private string AndroidBannerId;
	[SerializeField]
	private string AndroidIadsId;
	[SerializeField]
	private string IOSBannerId;
	[SerializeField]
	private string IOSIadsId;
	private string AndroidUnityVideoId;
	private string IOSUnityVideoId;
	[SerializeField]
	private string Admob_AdroidVideoRewardId;
	[SerializeField]
	private string Admob_IOSVideoRewardId;
	private string admob_VideoUnit;
	//private BannerView GA_Banner;
	//private InterstitialAd GA_Iad;
	//private RewardBasedVideoAd rewardBasedVideo;
	private string adUnitId;
	[SerializeField] private float GA_IAd_Reload = 60;
	private float timer = 0;
	bool iad_need_reload = true;

	void Awake ()
	{
		//AndroidBannerId = GameConfig.admob_android_banner_id;
		//AndroidIadsId = GameConfig.admob_android_full_id;
		//Admob_AdroidVideoRewardId = GameConfig.admob_android_video_id;
		instance = this;
		Init ();
		RequestBanner ();

	}

	void Update ()
	{
		if (iad_need_reload) {
			timer -= Time.deltaTime;
			if (timer < 0) {
				ReloadIads ();
			}
		}
	}

	private void ReloadIads ()
	{
		//GA_Iad.OnAdLoaded -= GA_Iad_OnAdLoaded;
		//GA_Iad.OnAdFailedToLoad -= GA_Iad_OnAdFailedToLoad;
		//GA_Iad.OnAdClosed -= GA_Iad_OnAdClosed;
		//GA_Iad.OnAdOpening -= GA_Iad_OnAdOpening;
		//GA_Iad.Destroy ();
		//GA_Iad = new InterstitialAd (AndroidIadsId);
		//GA_Iad.LoadAd (new AdRequest.Builder ().Build ());
		//GA_Iad.OnAdLoaded += GA_Iad_OnAdLoaded;
		//GA_Iad.OnAdFailedToLoad += GA_Iad_OnAdFailedToLoad;
		//GA_Iad.OnAdClosed += GA_Iad_OnAdClosed;
		//GA_Iad.OnAdOpening += GA_Iad_OnAdOpening;
		//timer = GA_IAd_Reload;
		//iad_need_reload = false;
	}

	public  void Init ()
	{
		#if UNITY_EDITOR
		adUnitId = "unused";
		#elif UNITY_ANDROID
		admob_VideoUnit = Admob_AdroidVideoRewardId;
		// unity ads
		adUnitId = AndroidUnityVideoId;
		#elif UNITY_IPHONE
		admob_VideoUnit = Admob_IOSVideoRewardId;
		// unity ads
		adUnitId = IOSUnityVideoId;
		#else
		adUnitId = "unexpected_platform";
		#endif

		//rewardBasedVideo = RewardBasedVideoAd.Instance;
		//this.RequestRewardBasedVideo();

		//AdRequest Unitrequest = new AdRequest.Builder ().Build ();
		InitHandler ();
		ReloadVideoAds ();
		//rewardBasedVideo.LoadAd (Unitrequest, admob_VideoUnit);

		//#if UNITY_IOS
		//GA_Iad = new GoogleMobileAds.Api.InterstitialAd (IOSIadsId);   
		//#else
		//GA_Iad = new GoogleMobileAds.Api.InterstitialAd (AndroidIadsId);
		//#endif
		//AdRequest iadRequest = new AdRequest.Builder ().Build ();
		//GA_Iad.OnAdLoaded += GA_Iad_OnAdLoaded;
		//GA_Iad.OnAdFailedToLoad += GA_Iad_OnAdFailedToLoad;
		//GA_Iad.OnAdClosed += GA_Iad_OnAdClosed;
		//GA_Iad.OnAdOpening += GA_Iad_OnAdOpening;
		//GA_Iad.LoadAd (iadRequest);
	}

	private void RequestRewardBasedVideo ()
	{
		// Create an empty ad request.
		//AdRequest request = new AdRequest.Builder ().Build ();
		// Load the rewarded video ad with the request.
//		#if UNITY_ANDROID
		//this.rewardBasedVideo.LoadAd (request, admob_VideoUnit);
//		#endif
//		#if UNITY_IOS
//		this.rewardBasedVideo.LoadAd(request, Admob_IOSVideoRewardId);
//		#endif
	}

	public void ReloadVideoAds ()
	{
		//AdRequest Unitrequest = new AdRequest.Builder ().Build ();
		////InitHandler ();
		//rewardBasedVideo.LoadAd (Unitrequest, admob_VideoUnit);
	}

	bool rewardedVideo = false;

	void InitHandler ()
	{
		//rewardBasedVideo.OnAdLoaded += RewardBasedVideo_OnAdLoaded;
		//// has failed to load.
		////rewardBasedVideo.OnAdFailedToLoad += RewardBasedVideo_OnAdFailedToLoad;
		//// is opened.
		//rewardBasedVideo.OnAdOpening += RewardBasedVideo_OnAdOpening;
		//// has started playing.
		//rewardBasedVideo.OnAdStarted += RewardBasedVideo_OnAdStarted;
		//// has rewarded the user.
		//rewardBasedVideo.OnAdRewarded += RewardBasedVideo_OnAdRewarded;
		//// is closed.
		//rewardBasedVideo.OnAdClosed += RewardBasedVideo_OnAdClosed;
		//// is leaving the application.
		//rewardBasedVideo.OnAdLeavingApplication += RewardBasedVideo_OnAdLeavingApplication;
	}

	private void RewardBasedVideo_OnAdLeavingApplication (object sender, System.EventArgs e)
	{
		//Debug.Log("Leaving App");
	}

	private void RewardBasedVideo_OnAdClosed (object sender, System.EventArgs e)
	{
		ReloadVideoAds ();
		if (rewardedVideo) {
			rewardedVideo = false;
			onVideoAdsFinish ();
		}
	}

	//private void RewardBasedVideo_OnAdRewarded (object sender, Reward e)
	//{
	//	//onVideoRewardBooster ();
	//	rewardedVideo = true;

	//}

	private void RewardBasedVideo_OnAdStarted (object sender, System.EventArgs e)
	{

	}

	private void RewardBasedVideo_OnAdOpening (object sender, System.EventArgs e)
	{
		//Debug.Log("Video Opening");
	}

	private void RewardBasedVideo_OnAdLoaded (object sender, System.EventArgs e)
	{
		//Debug.Log("Video is loaded");
//		GameplayControl.instance.bHint.image.color = new Color (1, 1, 1, 1);
	}

	//private void RewardBasedVideo_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs e)
	//{
	//	Invoke ("ReloadVideoAds", 10F);
	//	//Debug.Log("Video has failed to load");
	//}

	void GA_Iad_OnAdOpening (object sender, System.EventArgs e)
	{
		iad_need_reload = true;
		#if UNITY_IOS
		// SoundBase.instance.Mute();
		//FindObjectOfType<MusicBase>().source.volume = 0f;
		#endif
		//Time.timeScale = 0;
	}

	void GA_Iad_OnAdClosed (object sender, System.EventArgs e)
	{
		iad_need_reload = true;
		#if UNITY_IOS

		#endif

	}

	//void GA_Iad_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs e)
	//{
	//	iad_need_reload = true;
	//}

	void GA_Iad_OnAdLoaded (object sender, System.EventArgs e)
	{

	}
	//
	void GA_Banner_OnAdOpening (object sender, System.EventArgs e)
	{

	}

	void GA_Banner_OnAdClosed (object sender, System.EventArgs e)
	{

	}

	//void GA_Banner_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs e)
	//{

	//}

	void GA_Banner_OnAdLoaded (object sender, System.EventArgs e)
	{

	}

	//		private static string BuyCoin = "BuyCoin";

	//public void ShowIads ()
	//{

	//	if (GA_Iad.IsLoaded ()) {

	//		GA_Iad.Show ();
	//	}


	//}

	//public bool isFullScreenLoaded ()
	//{
	//	return GA_Iad != null && GA_Iad.IsLoaded ();
	//}

	private void RequestBanner ()
	{
//		#if UNITY_EDITOR
////		string adUnitId = "unused";
//		#elif UNITY_ANDROID
//		string adUnitId = AndroidBannerId;
//		#elif UNITY_IPHONE
//		string adUnitId = IOSBannerId;
//		#else
//		string adUnitId = "unexpected_platform";
//		#endif

//		// Create a 320x50 banner at the top of the screen.
//		GA_Banner = new BannerView (adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

//		// Create an empty ad request.
//		AdRequest request = new AdRequest.Builder ().Build ();
//		// Load the banner with the request.
//		//bannerView.LoadAd(request);

//		GA_Banner.LoadAd (request);
	}

	public void ShowBanner ()
	{
		//if (GA_Banner != null) {
		//	GA_Banner.Show ();

		//	Debug.Log ("Show Banner!");
		//}
	}

	public void HideBanner ()
	{
		//if (GA_Banner != null) {
		//	GA_Banner.Hide ();
		//}
	}

	void ShowVideoAds ()
	{
		//rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
		//if (rewardBasedVideo.IsLoaded ()) {
		//	rewardBasedVideo.Show ();
		//} else {
		//	StartCoroutine (WaitToLoadVideo ());
		//}
	}

	//IEnumerator WaitToLoadVideo ()
	//{
	//	AdRequest Unitrequest = new AdRequest.Builder ().Build ();
	//	rewardBasedVideo.LoadAd (Unitrequest, admob_VideoUnit);
	//	while (!rewardBasedVideo.IsLoaded ()) {
	//		yield return new WaitForSeconds (0.5f);
	//	}
	//	rewardBasedVideo.Show ();

	//}

	public void ShowRewardedAd (VideoAdsRewardType rewardType)
	{
		videoRewardType = rewardType;
		Debug.Log ("show admob video");
		ShowVideoAds ();


	}

	VideoAdsRewardType videoRewardType = VideoAdsRewardType.AD_REWARD_MORE_COIN;






	private void onVideoAdsFinish ()
	{
		///
		switch (videoRewardType) {
		case VideoAdsRewardType.AD_REWARD_MORE_COIN: 
			Debug.Log ("Get 15 coin");
			GameManager.dataSave.coin += 15;
			UITopControl.instance.tCoin.text = GameManager.dataSave.coin.ToString ();
			break;


		}

	}

	private void onVideoRewardBooster ()
	{
		//InAppButtonManager.instance.RewardBooster ();
	}

	//public bool isVideoAdmobLoad ()
	//{
	//	return rewardBasedVideo != null && rewardBasedVideo.IsLoaded ();
	//}



}

public enum VideoAdsRewardType
{
	AD_REWARD_MORE_COIN
}