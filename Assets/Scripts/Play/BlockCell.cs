using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCell : MonoBehaviour
{

	public SpriteRenderer cellRender;
	public SpriteRenderer fruitRender;
	bool _isFruit;
	public BlockColor fruitColor;

	public bool haveFruit {
		get {
			return _isFruit;

		}
		set {
			_isFruit = value;
			if (value == true) {
				fruitColor = UITopControl.instance.target.color;
				fruitRender.gameObject.SetActive (true);
				fruitRender.sprite = BlockColorDefine.instance.GetFruitSpriteByColor (UITopControl.instance.target.color);
			} else {
				fruitRender.gameObject.SetActive (false);
			}
		}
	}
}
