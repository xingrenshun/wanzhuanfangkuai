using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialControl : MonoBehaviour
{
	public static TutorialControl instance;
	public bool isShowingTutorial = false;
	public TutorialHand tutorialHand;
	public GameObject tutorialPopup;
	//public Text tTutorial;
	public Text tTutorial;

	void Awake ()
	{
		instance = this;
		Invoke ("Init", 2.5F);
	}

	void Init ()
	{
		string tutorialText = "";
		if (GameManager.dataSave.stagesList [GameManager.currentLevel].stars == 0) {
			switch (GameManager.currentLevel) {
			case 1: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_score;
				break;
			case 2: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_boosters;
				break;
			case 9: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_color;
				break;
			case 17: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_dirt;
				break;
			case 25: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_double;
				break;
			case 32: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_fruit;
				break;
			case 41: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_ice;
				break;
			case 54: 
				tutorialText = GameManager.gameTextsDic [GameManager.dataSave.language].tutorial_combo;
				break;
			}
			if (tutorialText != "") {
				string newIconString = "<sprite name=" + UITopControl.instance.iTargetIcon.sprite.name + ">";
				tutorialText = tutorialText.Replace ("{ICON}", newIconString);
				ShowTutorial (tutorialText);
			}
		}
	}

	public void ShowTutorial (string str)
	{
		//tutorialPopup.SetActive (true);
		tTutorial.text = str;
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			tutorialPopup.SetActive (false);
		}
	}



}