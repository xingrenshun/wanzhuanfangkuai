using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TargetPopup : MonoBehaviour
{
	
	// Use this for initialization
	public Text text;

	void Start ()
	{
		//for (int i = 0; i < text.spriteAsset.spriteInfoList.Count; i++) {
		//	text.spriteAsset.spriteInfoList [i].xOffset = 5;
		//	text.spriteAsset.spriteInfoList [i].yOffset = 50;
		//}
		StartCoroutine (Init ());
	}

	IEnumerator Init ()
	{
		yield return null;
		TargetModule target = UITopControl.instance.target;
		string task = "";
		switch (target.type) {
		case TargetType.TGT_COLOR: 
			task = GameManager.gameTextsDic [GameManager.dataSave.language].taskColor;
			break;
		case TargetType.TGT_COMBO:
			task = GameManager.gameTextsDic [GameManager.dataSave.language].taskCombo;
			break;
		case TargetType.TGT_DIRT:
			task = GameManager.gameTextsDic [GameManager.dataSave.language].taskDirt;
			break;
		case TargetType.TGT_DOUBLE:
			task = GameManager.gameTextsDic [GameManager.dataSave.language].taskDouble;
			break;
		case TargetType.TGT_FRUIT:
			task = GameManager.gameTextsDic [GameManager.dataSave.language].taskCommon;
			break;
		case TargetType.TGT_ICE:
			task = GameManager.gameTextsDic [GameManager.dataSave.language].taskIce;
			break;
		case TargetType.TGT_SCORE:
			task = GameManager.gameTextsDic [GameManager.dataSave.language].taskScore;
			break;
		}
		task = task.Replace ("{AMOUNT}", target.amount.ToString ());
		string newIconString = "<sprite name=" + UITopControl.instance.iTargetIcon.sprite.name + ">";
		task = task.Replace ("{ICON}", newIconString);
		text.text = task;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
