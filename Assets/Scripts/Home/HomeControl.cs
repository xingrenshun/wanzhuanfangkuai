using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HomeControl : MonoBehaviour
{
	public static HomeControl Instance;
	public UnityEngine.UI.Button bPlay, bSound, bMoreGame;
	public TextMeshProUGUI text;
	public TextMeshProUGUI TimerText;
    private void Awake()
    {
        Instance = this;
    }
    void Start ()
	{
        HomeControl.Instance.text.text = GameManager.dataSave.heart + "/5";
        //Admob.instance.ShowBanner ();
        bSound.image.sprite = AudioManager.instance.GetSoundStatus ();
		if (AudioManager.instance.AS_MUSIC.isPlaying == false) {
			AudioManager.instance.PlayMusic (AudioClipType.AC_MAIN_MUSIC);
		}
	}

	public void Play ()
	{
		LoadSceneFading.LoadScene ("scn_selectLevel");
		//Admob.instance.HideBanner ();
	}

	public void bSoundClick ()
	{
		bSound.image.sprite = AudioManager.instance.ChangeSoundStatus ();
	}
	public void bMoreGames()
	{
		#if UNITY_IPHONE
		Application.OpenURL ("https://itunes.apple.com/us/developer/tam-tran/id1390927450?mt=8");
		#endif
	}
}