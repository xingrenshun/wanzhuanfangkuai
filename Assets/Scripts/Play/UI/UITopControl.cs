using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITopControl : MonoBehaviour
{
	
	public static UITopControl instance;
	public TextMeshProUGUI tCoin, tTargetCount, tMoveCount;
	public bool isShowingPauseMenu = false;
	public TargetModule target;
	public Image iTargetIcon;
	public Sprite sprCup, sprCupX2;
	public GameObject boosterPopup, pausePopup;

	void Awake ()
	{
		instance = this;
	}

	public void Init ()
	{
		target = GameManager.levelsDic [GameManager.currentLevel].targets [0];
		InitTargetIcon ();
	}
    private void Start()
    {
		ShowBoosterPopup();
    }
    void InitTargetIcon ()
	{
		switch (target.type) {
		case TargetType.TGT_SCORE: 
			iTargetIcon.sprite = sprCup;
			break;
		case TargetType.TGT_COLOR: 
			iTargetIcon.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (target.color);
			break;
		case TargetType.TGT_COMBO: 
			iTargetIcon.sprite = sprCupX2;
			break;
		case TargetType.TGT_DIRT: 
			iTargetIcon.sprite = BlockColorDefine.instance.sprGridDirt;
			break;
		case TargetType.TGT_DOUBLE: 
			iTargetIcon.sprite = sprCup;
			break;
		case TargetType.TGT_FRUIT: 
			iTargetIcon.sprite = BlockColorDefine.instance.GetFruitSpriteByColor (target.color);
			break;
		case TargetType.TGT_ICE: 
			iTargetIcon.sprite = BlockColorDefine.instance.sprIce3;
			break;
		}
	}

	public void ShowPausePopup ()
	{
		pausePopup.SetActive (true);
		//Admob.instance.ShowIads ();

	}

	public void ShowBoosterPopup ()
	{
		if (boosterPopup.activeInHierarchy == false) {
			boosterPopup.SetActive (true);
		}
	}

	public void ShowMoreCoinPopup ()
	{
		MoreCoinPopup.instance.ShowPopup ();
	}
}


