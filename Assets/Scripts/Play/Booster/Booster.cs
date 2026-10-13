using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Booster : MonoBehaviour
{

	BoosterType _type;
	public Image iBooster;
	public int price;
	public TextMeshProUGUI tPrice, tBoosterName;
	public Button bMoreCoin;

	public BoosterType type {
		get { return _type; }
		set {
			_type = value;
			iBooster.sprite = BoosterDefine.instance.GetBoosterSprite (value);
			price = BoosterDefine.instance.GetBoosterPrice (value);
			tPrice.text = price.ToString ();
			tBoosterName.text = BoosterDefine.instance.GetBoosterName (value);
		}
	}

	public void MoreCoinClick ()
	{
		MoreCoinPopup.instance.ShowPopup ();
	}

	public void OnClick ()
	{
		if (GameManager.dataSave.coin >= price) {
			// su dung booster;
			BoosterPopup.instance.UseBooster (type);
		} else {
			MoreCoinClick ();
		}
	}
}

