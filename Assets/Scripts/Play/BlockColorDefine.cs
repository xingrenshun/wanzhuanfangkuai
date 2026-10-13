using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlockColorDefine : MonoBehaviour
{
	public static BlockColorDefine instance;
	public Sprite sprCellLose, sprGrid;
	public Sprite sprGrid1, sprGrid2, sprGridBlank, sprGridDirt, sprGridDirt2, sprIce1, sprIce2, sprIce3, sprBomb;


	void Awake ()
	{
		instance = this;
	}



	[System.Serializable]
	public struct BlockColorStruct
	{
		public BlockColor col;
		public Sprite blockSprite;
		public Sprite fruitSprite;
	}

	public List<BlockColorStruct> listBlockColorStruct;

	public Sprite GetBlockSpriteByColor (BlockColor col)
	{
		foreach (BlockColorStruct str in listBlockColorStruct) {
			if (str.col == col) {
				return str.blockSprite;
			}
		}
		Debug.Log ("khong tim thay color " + col.ToString ());
		return null;
	}

	public Sprite GetFruitSpriteByColor (BlockColor col)
	{
		foreach (BlockColorStruct str in listBlockColorStruct) {
			if (str.col == col) {
				return str.fruitSprite;
			}
		}
		return null;
	}
}

[System.Serializable]
public enum BlockColor
{
	BC_0 = 1,
	BC_1 = 2,
	BC_2 = 3,
	BC_3 = 4,
	BC_4 = 5,
	BC_5 = 6,
	BC_6 = 7,
}