using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoosterPopup : MonoBehaviour
{
	public static BoosterPopup instance;
	bool _isUsingBooster = false;

	public bool isUsingBooster {
		set {
			GameplayControl.instance.isUsingBooster = value;
			_isUsingBooster = value;
			if (value == true) {
				//boosterGroup.SetActive (false);
				//tHowToUse.gameObject.SetActive (true);

			} else {
				//boosterGroup.SetActive (true);
				//tHowToUse.gameObject.SetActive (false);
			}
		}
		get { return _isUsingBooster; }
	}

	public GameObject boosterGroup;
	public Text tHowToUse;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		for (int i = 0; i < boostersList.Count; i++) {
			Booster booster = boostersList [i];
			booster.type = BoosterDefine.instance.listBoosterStruct [i].type;
		}
	}

	void OnEnable ()
	{
		isUsingBooster = false;
	}

	public List<Booster> boostersList;
	public BoosterType boosterUsingType;

	public void Close ()
	{
		isUsingBooster = false;
		gameObject.SetActive (false);
	}

	public void UseBooster (BoosterType type)
	{
		if (GameplayControl.instance.losePopup.activeInHierarchy == true) {
			//GameplayControl.instance.losePopup.SetActive (false);
			GameplayControl.instance.isFailed = false;
		}

		Debug.Log ("Use booster " + type.ToString ());
		if (type == BoosterType.BT_CHANGE_FIGURES) {
			for (int i = 0; i < GameplayControl.instance.listStartBlock.Count; i++) {
				Block _startBlock = GameplayControl.instance.listStartBlock [i];
				_startBlock.gameObject.SetActive (true);
				_startBlock.InitBlock (GameManager.GetRandomBlockId ());
				_startBlock.NewBlockEffect ();
			}
			GameplayControl.instance.CheckHaveStartBlockMatch ();
			GameplayControl.instance.coinCount -= BoosterDefine.instance.GetBoosterPrice (type);
			GameManager.SaveData ();
			Close ();
		} else {
			isUsingBooster = true;
			boosterUsingType = type;
			tHowToUse.text = GameManager.gameTextsDic [GameManager.dataSave.language].tapContinue;
		}
		//Close ();
	}

}

