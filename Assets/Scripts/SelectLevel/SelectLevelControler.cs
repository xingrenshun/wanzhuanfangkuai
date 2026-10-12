using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class SelectLevelControler : MonoBehaviour
{
	public static SelectLevelControler instance;

	public Button bSound, bHome, bMoreCoin;

	public TextMeshProUGUI tCoin, tHeartTime, text;

	//public List<GameObject> heartList;

	public Sprite sprLevelButtonPassed, sprLevelButtonNoPassed;
	public GameObject highLight;
	public GameObject MoreHeartPopup;
	int _coin;

	public int coin {
		get { return _coin; }
		set {
			_coin = value;
			tCoin.text = value.ToString ();
		}
	}

	void Awake ()
	{
		instance = this;
	}

	public void InitHeart ()
	{
        //for (int i = 0; i < heartList.Count; i++) {
        //	if (i < GameManager.dataSave.heart) {
        //		heartList [i].SetActive (true);
        //	} else {
        //		heartList [i].SetActive (false);
        //	}
        //}
        text.text = GameManager.dataSave.heart + "/5";

    }



	void Start ()
	{

		coin = GameManager.dataSave.coin;
		bSound.image.sprite = AudioManager.instance.GetSoundStatus ();
		InitHeart ();
	}


	public void bSoundClick ()
	{
		bSound.image.sprite = AudioManager.instance.ChangeSoundStatus ();
	}

	public void GoToHome ()
	{
		LoadSceneFading.LoadScene ("scn_home");
		//Admob.instance.ShowBanner ();
	}

	public void ShowMoreCoinPopup ()
	{
		MoreCoinPopup.instance.ShowPopup ();
	}

}
