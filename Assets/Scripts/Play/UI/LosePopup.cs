using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour
{
	public Button bMenu, bReplay;
	public Text tLevelFailed;

	void OnEnable ()
	{
		//tLevelFailed.text = GameManager.gameTextsDic [GameManager.dataSave.language].noMoves;
		AudioManager.instance.PlaySound (AudioClipType.AC_LOSE);
		StartCoroutine (Init ());
		if (GameplayControl.instance.moveCount > 0) {
			GameplayControl.instance.ShowBooster (true);
		}
		StartCoroutine(GuangGao.Instance.WaitShowXP());

    }

	IEnumerator Init ()
	{
		bMenu.gameObject.SetActive (false);
		bReplay.gameObject.SetActive (false);
		yield return new WaitForSecondsRealtime (0.5F);
		bMenu.gameObject.SetActive (true);
		yield return new WaitForSecondsRealtime (1F);
		bReplay.gameObject.SetActive (true);
	}

	public void GoToMenu ()
	{
		LoadSceneFading.LoadScene ("scn_selectLevel");

	}

	public void Replay ()
	{
		if (GameManager.dataSave.heart > 0) {
			LoadSceneFading.LoadScene ("scn_play");
		} else {
			LoadSceneFading.LoadScene ("scn_selectLevel");
		}

	}
}
