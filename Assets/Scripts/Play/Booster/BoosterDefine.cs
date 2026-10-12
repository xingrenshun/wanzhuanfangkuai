using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoosterDefine : MonoBehaviour
{
	public static BoosterDefine instance;

	void Awake ()
	{
		instance = this;
	}

	public List<BoosterStruct> listBoosterStruct;

	public Sprite GetBoosterSprite (BoosterType type)
	{
		foreach (var booster in listBoosterStruct) {
			if (booster.type == type) {
				return booster.sprite;
			}
		}
		return null;
	}

	public int GetBoosterPrice (BoosterType type)
	{
		foreach (var booster in listBoosterStruct) {
			if (booster.type == type) {
				return booster.price;
			}
		}
		return 0;
	}

	public string GetBoosterName (BoosterType type)
	{
		switch (type) {
		case BoosterType.BT_CHANGE_FIGURES:
			return GameManager.gameTextsDic [GameManager.dataSave.language].changeFigures;
		case BoosterType.BT_CLEAR_AREA:
			return GameManager.gameTextsDic [GameManager.dataSave.language].clearArea;

		case BoosterType.BT_CLEAR_ONE_FIELD:
			return GameManager.gameTextsDic [GameManager.dataSave.language].clearOne;

		case BoosterType.BT_CLEAR_TYPE:
			return GameManager.gameTextsDic [GameManager.dataSave.language].clearSame;

		}
		return "";
	}



}

[System.Serializable]
public class BoosterStruct
{
	public BoosterType type;
	public int price;
	public Sprite sprite;
	public string name;
	public string howToUse;
}

public enum BoosterType
{
	BT_CHANGE_FIGURES,
	BT_CLEAR_ONE_FIELD,
	BT_CLEAR_TYPE,
	BT_CLEAR_AREA
}