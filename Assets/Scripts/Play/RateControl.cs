using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateControl : MonoBehaviour
{
	public Toggle tgNeverShowAgain;
	public GameObject ratePopup2;

	void Enable ()
	{
		Time.timeScale = 0;
		tgNeverShowAgain.isOn = false;
	}

	public void Close ()
	{
		if (tgNeverShowAgain.isOn == true) {
			GameManager.dataSave.isRate = true;
			GameManager.SaveData ();
			//SUGame.Get<SUAnalytics> ().Rating (1, "Don't ask me again");
		}
		Time.timeScale = 1;
		if (ratePopup2.activeInHierarchy == true) {
			ratePopup2.SetActive (false);
		}
		gameObject.SetActive (false);

	}

	public void Rate ()
	{
		//Application.OpenURL (SUGame.Get<SURemoteConfig> ().GetRateUrl ());
		GameManager.dataSave.isRate = true;
		GameManager.SaveData ();
		Close ();
		//SUGame.Get<SUAnalytics> ().Rating (1, "5 star");
	}

	public void FeedBack ()
	{
		ratePopup2.SetActive (true);
		gameObject.SetActive (false);

	}

	public void NotNow ()
	{
		tgNeverShowAgain.isOn = true;
		Close ();
	}

	public void EmailUs ()
	{
		//SUGame.Get<SUAnalytics> ().Rating (1, "1-4 star");
		//email Id to send the mail to
		//SoundBase.instance.Click();
		string email = "sugame293@gmail.com";
		//subject of the mail
		string subject = MyEscapeURL ("FEEDBACK");
		//body of the mail which consists of Device Model and its Operating System
		string body = MyEscapeURL ("Please leave your feedback here: \n\n\n\n\n" +
		              "_____________________" +
		              "\n\nPlease Do Not Modify This:\n\n" +
		              "Model: " + SystemInfo.deviceModel + "\n" +
		              "OS: " + SystemInfo.operatingSystem + "\n" +
		              "_____________________");
		//Open the Default Mail App
		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
		PlayerPrefs.SetInt ("Rate", 1);
		//GetComponent<ManagePopupEffect>().Close();
		//PopupSystem.instance.Win();
		Close ();

	}

	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL (url).Replace ("+", "%20");
	}

}
