using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class WinPopupControl : MonoBehaviour
{
	public List<GameObject> listStars;
	public Button bMenu, bReplay, bNext;
	public TextMeshProUGUI tCoin, tScore, tWin;
	int star = 3;



	void OnEnable ()
	{
		
		//tWin.text = GameManager.gameTextsDic [GameManager.dataSave.language].win;
		StartCoroutine (InitStar ());
		StartCoroutine (IncreaseCoinScore ());
        StartCoroutine(GuangGao.Instance.WaitShowXP());

    }

    IEnumerator InitStar ()
	{		
		int min = GameManager.levelsDic [GameManager.currentLevel].moves / 3;
		if (GameplayControl.instance.moveCountRemain < min) {
			star = 1;
		} else if (GameplayControl.instance.moveCountRemain >= min && GameplayControl.instance.moveCountRemain < min * 2) {
			star = 2;
		} else if (GameplayControl.instance.moveCountRemain >= min * 2) {
			star = 3;
		}
		Debug.Log ("Star : " + star);
		// tang star 
		for (int i = 0; i < 3; i++) {
			listStars [i].SetActive (false);
			//yield return new WaitForSecondsRealtime (1F);
		}
		yield return new WaitForSecondsRealtime (0.6F);
		for (int i = 0; i < star; i++) {
			listStars [i].SetActive (true);
			yield return new WaitForSecondsRealtime (0.4F);
		}
		GameManager.SaveData ();
	}


	IEnumerator IncreaseCoinScore ()
	{
		float score = 0;
		bMenu.gameObject.SetActive (false);
		bReplay.gameObject.SetActive (false);
		bNext.gameObject.SetActive (false);
		yield return new WaitForSecondsRealtime (0.5F);
		bMenu.gameObject.SetActive (true);
		bReplay.gameObject.SetActive (true);
		// tang diem 
		float coin = 0;
		int coinIncrease = GameplayControl.instance.coinCount - GameManager.dataSave.coin;
		while (Mathf.RoundToInt (coin) < coinIncrease) {
			coin += (coinIncrease - coin) / 10;

			tCoin.text = Mathf.RoundToInt (coin).ToString ();
			Debug.Log ("tang coin");
			yield return null;
		}
		GameManager.dataSave.coin = GameplayControl.instance.coinCount;
		while (Mathf.RoundToInt (score) < GameplayControl.instance.score) {
			score += ((float)GameplayControl.instance.score - score) / 10;

			tScore.text = Mathf.RoundToInt (score).ToString ();
			Debug.Log ("tang score");
			yield return null;
		}
		if (GameManager.dataSave.stagesList [GameManager.currentLevel].stars < star) {
			GameManager.dataSave.stagesList [GameManager.currentLevel].stars = star;
		}
		if (GameManager.dataSave.stagesList [GameManager.currentLevel].bestScore < Mathf.RoundToInt (score)) {
			GameManager.dataSave.stagesList [GameManager.currentLevel].bestScore = Mathf.RoundToInt (score);
		}
		if (GameManager.dataSave.heart < 5) {
			GameManager.dataSave.heart++;
		}
		if (GameManager.currentLevel == GameManager.dataSave.levelMax) {
			GameManager.dataSave.levelMax++;
		}
		GuangGao.Instance.UploadScore(GameManager.GetTotalStars());
		GameManager.SaveData ();
		// 
		yield return new WaitForSecondsRealtime (0.5F);
		bNext.gameObject.SetActive (true);

	}

	public void GoToMenu ()
	{
		LoadSceneFading.LoadScene ("scn_selectLevel");
	}

	public void Replay ()
	{
		LoadSceneFading.LoadScene ("scn_play");
         
	}

	public void Next ()
	{
		GameManager.currentLevel++;
		LoadSceneFading.LoadScene ("scn_play");

	}
}
