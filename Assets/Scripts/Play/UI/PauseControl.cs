using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseControl : MonoBehaviour
{
	public Button bPlay, bMenu, bMoreGame, bReplay, bSound;

	void OnEnable ()
	{
		
		Time.timeScale = 0;
		bSound.image.sprite = AudioManager.instance.GetSoundStatus ();
	}

	public void GotoMenu ()
	{
		Time.timeScale = 1;
		LoadSceneFading.LoadScene ("scn_selectLevel");

	}

	public void Close ()
	{
		Time.timeScale = 1;
		gameObject.SetActive (false);
	}

	public void Replay ()
	{
		Time.timeScale = 1;
		LoadSceneFading.LoadScene ("scn_play");
	}

	public void ChangeSoundStatus ()
	{
		bSound.image.sprite = AudioManager.instance.ChangeSoundStatus ();
	}

	public void MoreGame ()
	{
		Debug.Log ("MoreGame clicked");
	}
}

