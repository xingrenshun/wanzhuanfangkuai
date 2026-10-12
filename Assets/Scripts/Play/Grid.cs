using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

	public int X, Y;
	public SpriteRenderer render;
	public GameObject renderSpriteRenderer;
	public GameObject subGrid, hintGrid;
	public GameObject icePrefap;
	public bool haveBlockCell = false;
	public GameObject clearEffectPrefap, bombEffectPrefap, iceBreackEffectPrefap, bombLineEffectPrefap;
	//public GameObject putDownEffect;
	public GameObject bombArrowPrefap;
	GameObject bombArrow;
	Color col1 = new Color (1, 1, 1, 1);
	Color col2 = new Color (1, 1, 1, 0.3F);
	public BlockColor gridCol, fruitColor;
	public SpriteRenderer subRender, fruitRender;
	public Sprite beforeBlockSprite;

	public Sprite sprCell {
		get {
			return (11 * X + Y) % 2 == 1 ? BlockColorDefine.instance.sprGrid1 : BlockColorDefine.instance.sprGrid2;
		}
	}

	BombType _bombType;

	public BombType bombType {
		get { return _bombType; }
		set {
			
			switch (value) {
			case BombType.NOONE: 
				if (bombArrow != null) {
					Destroy (bombArrow);
				}
				break;
			case BombType.HORIZONTAL: 
				if (bombArrow == null) {
					bombArrow = (GameObject)Instantiate (bombArrowPrefap, transform.position, Quaternion.Euler (0, 0, 0));
				} else {
					bombArrow.transform.rotation = Quaternion.Euler (0, 0, 0);
				}
				subGrid.gameObject.SetActive (true);
				subRender.sprite = BlockColorDefine.instance.sprBomb;
				subRender.color = col1;
				break;
			case BombType.VERTICAL: 
				if (bombArrow == null) {
					bombArrow = (GameObject)Instantiate (bombArrowPrefap, transform.position, Quaternion.Euler (0, 0, 90));
				} else {
					bombArrow.transform.rotation = Quaternion.Euler (0, 0, 90);
				}
				subGrid.gameObject.SetActive (true);
				subRender.sprite = BlockColorDefine.instance.sprBomb;
				subRender.color = col1;
				break;
			case BombType.SQUAREBOMB:
				subGrid.gameObject.SetActive (true);
				subRender.sprite = BlockColorDefine.instance.sprBomb;
				subRender.color = col1;
				break;
			}
			_bombType = value;
		}
	}

	public void CreateBomb (BombType _type)
	{
		Debug.Log ("Ham create bomb");
		bombType = _type;
		if (_type != BombType.NOONE) {
			haveBlockCell = true;
		}
	}

	int _iceLevel;
	GameObject iceObject;

	public int iceLevel {
		get { return _iceLevel; }
		set {
			_iceLevel = value;
			switch (value) {
			case -1: 
				if (iceObject != null) {
					Destroy (iceObject);
				}
				break;
			case 0: 
				if (iceObject != null) {
					Destroy (iceObject);
					if (UITopControl.instance.target.type == TargetType.TGT_ICE) {
						GameplayControl.instance.targetCount++;
					}
				}
				break;
			case 1: 
				if (iceObject == null) {
					iceObject = (GameObject)Instantiate (icePrefap, transform.position, Quaternion.Euler (0, 0, 0));
				}
				iceObject.GetComponent<SpriteRenderer> ().sprite = BlockColorDefine.instance.sprIce1;
				break;
			case 2: 
				if (iceObject == null) {
					iceObject = (GameObject)Instantiate (icePrefap, transform.position, Quaternion.Euler (0, 0, 0));
				}
				iceObject.GetComponent<SpriteRenderer> ().sprite = BlockColorDefine.instance.sprIce2;
				break;
			case 3: 
				if (iceObject == null) {
					iceObject = (GameObject)Instantiate (icePrefap, transform.position, Quaternion.Euler (0, 0, 0));
				}
				iceObject.GetComponent<SpriteRenderer> ().sprite = BlockColorDefine.instance.sprIce3;
				break;
			}

		}
	}

	bool _haveFruit;

	public bool haveFruit {
		get { return _haveFruit; }
		set {
			_haveFruit = value;
			if (value == true) {
				fruitRender.gameObject.SetActive (true);
				fruitRender.sprite = BlockColorDefine.instance.GetFruitSpriteByColor (fruitColor);
			} else {
				fruitRender.gameObject.SetActive (false);
			}
		}
	}

	[SerializeField]
	bool _isBlank;

	public bool isBlank {
		get {
			return _isBlank;
		}
		set {
			_isBlank = value;
			if (value == true) {
				render.sprite = BlockColorDefine.instance.sprGridBlank;
			}

		}
	}

	int _dirtAmount;

	public int dirAmount {
		get { return _dirtAmount; }
		set {
			_dirtAmount = value;
			if (value == 2) {
				render.sprite = BlockColorDefine.instance.sprGridDirt2;
			} else if (value == 1) {
				render.sprite = BlockColorDefine.instance.sprGridDirt;
			} else {
				if (UITopControl.instance.target.type == TargetType.TGT_DIRT) {
					GameplayControl.instance.targetCount++;
				}
				render.sprite = sprCell;
			}
		}
	}

	int SubRenderSoftLayer = 0;

	void Awake ()
	{
		render = renderSpriteRenderer.GetComponent<SpriteRenderer> ();
		subRender = subGrid.GetComponent<SpriteRenderer> ();
		SubRenderSoftLayer = subRender.sortingOrder;

	}

	void Start ()
	{
		//isBlank = false;
		gameObject.name = "Gird_X" + X + "_Y" + Y;
	}
	// clear when match
	IEnumerator clearCoroutine;

	public void Clear (float time)
	{
		if (haveBlockCell == false) {
			return;
		}
		if (clearCoroutine == null) {
			clearCoroutine = ClearCoroutine (time);
			StartCoroutine (clearCoroutine);
		}
	}

	public void SetGrid (ElementType _type, BlockColor _color, BlockColor _fruitColor, bool _haveFruit, bool _haveIce, int _iceType, int _dirtAmount)
	{
		switch (_type) {
		case ElementType.ET_BLANK:
			isBlank = true;
			break;
		case ElementType.ET_DIRT: 
			dirAmount = _dirtAmount;
			break;
		case ElementType.ET_FILL: 
			if (_haveIce == true) {
				Debug.Log ("iceType" + _iceType); 
				switch (_iceType) {
				case -1:
				case 0: 
				case 1:
					iceLevel = 3;
					break;
				case 2:
					iceLevel = 2;
					break;
				case 3: 
					iceLevel = 1;
					break;
				}
			}
			if (_haveFruit == true) {
				subRender.gameObject.SetActive (true);
				subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (_color);
				fruitRender.gameObject.SetActive (true);
				fruitRender.sprite = BlockColorDefine.instance.GetFruitSpriteByColor (_fruitColor);
				fruitColor = _fruitColor;
				//subRender.color = new Color (1, 1, 1, 0.7F);
				haveBlockCell = true;
				haveFruit = true;
			} else {
				subRender.gameObject.SetActive (true);
				subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (_color);
				haveBlockCell = true;
			}
			break;
		}
	}

	IEnumerator ClearCoroutine (float time)
	{
		haveBlockCell = false;
		yield return new WaitForSeconds (time);
		if (bombArrow != null) {
			Destroy (bombArrow);
		}
		if (iceLevel > 0) {
			GameObject iceBreakEffect = (GameObject)Instantiate (iceBreackEffectPrefap, transform.position, Quaternion.Euler (0, 0, 0));
			Destroy (iceBreakEffect, 2F);
			iceLevel--;
			if (iceLevel > 0) {
				haveBlockCell = true;
				clearCoroutine = null;
				yield break;
			}
		}
		switch (bombType) {
		case BombType.NOONE: 
			GameObject clearEffect = (GameObject)Instantiate (clearEffectPrefap, transform.position, Quaternion.Euler (0, 0, 0));
			clearEffect.GetComponent<ClearEffect> ().render.sprite = subRender.sprite;

			break;
		case BombType.HORIZONTAL: 
			GameObject bombLineEffect = (GameObject)Instantiate (bombLineEffectPrefap, transform.position, Quaternion.Euler (0, 0, 0));
			bombLineEffect.GetComponent<LineBombEffect> ().type = LineBombType.Horizontal;
			for (int x = 0; x < GameManager.xCount; x++) {
				if (GameplayControl.instance.grids [x, Y].haveBlockCell == true) {
					GameplayControl.instance.grids [x, Y].Clear (0.2F);
				}
			}
			break;
		case BombType.VERTICAL: 
			bombLineEffect = (GameObject)Instantiate (bombLineEffectPrefap, transform.position, Quaternion.Euler (0, 0, 0));
			bombLineEffect.GetComponent<LineBombEffect> ().type = LineBombType.Vertical;
			for (int y = 0; y < GameManager.yCount; y++) {
				if (GameplayControl.instance.grids [X, y].haveBlockCell == true) {
					GameplayControl.instance.grids [X, y].Clear (0.2F);
				}
			}
			break;
		case BombType.SQUAREBOMB: 
			GameObject bombEffect = (GameObject)Instantiate (bombEffectPrefap, transform.position, Quaternion.Euler (0, 0, 0));
			Destroy (bombEffect, 2F);
			for (int x = 0; x < GameManager.xCount; x++) {
				for (int y = 0; y < GameManager.yCount; y++) {
					if (Mathf.Abs (x - X) <= 2 && Mathf.Abs (y - Y) <= 2) {
						GameplayControl.instance.grids [x, y].Clear (0.2F);
					}
				}
			}
			break;
		}




		if (UITopControl.instance.target.type == TargetType.TGT_COLOR && gridCol == UITopControl.instance.target.color) {
			GameplayControl.instance.targetCount++;
		}
		if (dirAmount > 0) {
			dirAmount--;
		}

		subGrid.gameObject.SetActive (false);
		if (haveFruit == true) {
			haveFruit = false;
			fruitRender.gameObject.SetActive (false);
			if (UITopControl.instance.target.type == TargetType.TGT_FRUIT && fruitColor == UITopControl.instance.target.color) {
				//Debug.Log ("grid color " + gridCol + " : " + UITopControl.instance.target.color);

				GameplayControl.instance.targetCount++;
			}
		}

		clearCoroutine = null;
		yield return new WaitForSeconds (2f);

	}
	// clear to default grid
	public void ClearGrid ()
	{
		haveBlockCell = false;
		subGrid.SetActive (false);
		hintGrid.SetActive (false);
	}
	// clear to default grid
	public void ClearInit ()
	{
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				int _x = X - 2 + x;
				int _y = Y - 2 + y;
				if (_x >= 0 && _x < GameManager.xCount && _y >= 0 && _y < GameManager.yCount) {
					if (GameplayControl.instance.grids [_x, _y].haveBlockCell == false) {						
						GameplayControl.instance.grids [_x, _y].subGrid.SetActive (false);
						GameplayControl.instance.grids [_x, _y].fruitRender.gameObject.SetActive (false);
					}

				}
			}
		}

	}

	public bool[] Init (int _id, bool _createCell, bool isCheck, bool isCreateHint)
	{
		bool canCreateCell = true;
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				bool isActive = GameManager.listBlockData [_id].blockGrid [x, y] == 1 ? true : false;
				if (isActive == true) {
					int _x = X - 2 + x;
					int _y = Y - 2 + y;

					if (_x >= 0 && _x < GameManager.xCount && _y >= 0 && _y < GameManager.yCount) {
						if (GameplayControl.instance.grids [_x, _y].haveBlockCell == true || GameplayControl.instance.grids [_x, _y].isBlank == true) {
							canCreateCell = false;
							break;
						}

					} else {						
						canCreateCell = false;
						break;
					}
				}
			}
			if (canCreateCell == false) {
				break;
			}
		}
		if (isCheck == true) {
			return new bool[]{ canCreateCell, false };
		}

		bool createCompleted = false;
		int score = 0;
		int comboScore = 0;
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				bool isActive = GameManager.listBlockData [_id].blockGrid [x, y] == 1 ? true : false;
				if (isActive == true) {
					int _x = X - 2 + x;
					int _y = Y - 2 + y;
					if (_x >= 0 && _x < GameManager.xCount && _y >= 0 && _y < GameManager.yCount && GameplayControl.instance.grids [_x, _y].subGrid.activeInHierarchy == false && GameplayControl.instance.grids [_x, _y].isBlank == false) {
						if (canCreateCell == true) {
							if (_createCell == true) {
								createCompleted = true;
								GameplayControl.instance.score++;
								score++;
								Grid _grid = GameplayControl.instance.grids [_x, _y];
								_grid.subRender.sortingOrder = 5;
								_grid.Invoke ("ChangeSortLayer", 1F);
								//
								_grid.subGrid.SetActive (true);
								//_grid.putDownEffect.SetActive (true);
								_grid.haveBlockCell = true;
								_grid.subRender.color = col1;
								_grid.subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
								_grid.gridCol = GameplayControl.instance.moveBlock.blockColor;
								_grid.hintGrid.SetActive (false);
								if (GameplayControl.instance.moveBlock.listBlockCell [x, y].haveFruit == true) {
									_grid.fruitColor = GameplayControl.instance.moveBlock.listBlockCell [x, y].fruitColor;
									_grid.haveFruit = true;
								}
								List<Grid> listGridMatch = GameplayControl.instance.CheckMatch (GameplayControl.instance.grids [_x, _y]);
								if (listGridMatch.Count > 0) {
									
									comboScore += listGridMatch.Count > GameManager.xCount ? 2 : 1;
									for (int i = 0; i < listGridMatch.Count; i++) {
										//listGridMatch [i].subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
										listGridMatch [i].Clear (i * 0.2F);

										if (listGridMatch [i].isBlank == false) {
											AudioManager.instance.PlaySoundTime (AudioClipType.AC_POP, i * 0.2F);
										}
										if (listGridMatch [i].bombType != BombType.NOONE) {
											if (i > 0 && i < 9) {
												if (listGridMatch [i + 1].X == listGridMatch [i].X || listGridMatch [i - 1].X == listGridMatch [i].X) {
													if (listGridMatch [i].bombType == BombType.VERTICAL) {
														listGridMatch [i].bombType = BombType.HORIZONTAL;
													}
												} else if (listGridMatch [i + 1].Y == listGridMatch [i].X || listGridMatch [i - 1].Y == listGridMatch [i].X) {
													if (listGridMatch [i].bombType == BombType.HORIZONTAL) {
														listGridMatch [i].bombType = BombType.VERTICAL;
													}
												}
											}
										}
									}
									listGridMatch.Shuffle ();
									// create coin 
									for (int i = 0; i < 2; i++) {

										Instantiate (GameplayControl.instance.coinPrefap, listGridMatch [i].transform.position, Quaternion.Euler (0, 0, 0));
									}
								}
							} else {
								// neu co tao subGrid
								createCompleted = false;
								if (isCreateHint == false) {
									// neu la ko hint 
									GameplayControl.instance.grids [_x, _y].subGrid.SetActive (true);
									GameplayControl.instance.grids [_x, _y].subRender.color = col2;
									GameplayControl.instance.grids [_x, _y].subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
									if (GameplayControl.instance.moveBlock.listBlockCell [x, y].haveFruit == true) {
										GameplayControl.instance.grids [_x, _y].fruitColor = GameplayControl.instance.moveBlock.listBlockCell [x, y].fruitColor;
										GameplayControl.instance.grids [_x, _y].fruitRender.gameObject.SetActive (true);
									}
									List<Grid> listGridMatch = GameplayControl.instance.CheckMatch (GameplayControl.instance.grids [_x, _y]);
									if (listGridMatch.Count > 0) {
										for (int i = 0; i < listGridMatch.Count; i++) {
											listGridMatch [i].beforeBlockSprite = listGridMatch [i].subRender.sprite;
											//listGridMatch [i].subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
										}
									}
								} else {
									// neu la hint 
									GameplayControl.instance.grids [_x, _y].hintGrid.SetActive (true);
								}
							}
						}
					}
				}

			}
		}

		int scoreAdd = 0;
		switch (comboScore) {
		case 1: 
			scoreAdd = 10;
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_1);
			break;
		case 2: 
			scoreAdd = 30;
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_2);
			break;
		case 3: 
			scoreAdd = 60;
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_3);
			break;
		case 4: 
			scoreAdd = 100;
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_4);
			break;
		case 5: 
			scoreAdd = 150;
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_5);
			break;
		case 6: 
			scoreAdd = 250;
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_5);
			break;
		}
		scoreAdd += score;
		if (comboScore > 1 && UITopControl.instance.target.type == TargetType.TGT_DOUBLE) {
			GameplayControl.instance.targetCount++;
		}
		if (scoreAdd > 0) {
			CreateScoreEffect (scoreAdd);
		}
		// create bomb 
		if (comboScore >= 2) {
			
			GameplayControl.instance.listAllGrids.Shuffle ();
			int k = 0;
			for (int i = 0; i < comboScore - 1; i++) {
				
				for (int j = k; j < GameplayControl.instance.listAllGrids.Count - 1; j++) {
					Grid _grid = GameplayControl.instance.listAllGrids [i];
					if (_grid.X == X || _grid.Y == Y) {
						continue;
					}
					Debug.Log ("Create Bomb");
					_grid.CreateBomb (Random.Range (0, 100) > 50 ? BombType.HORIZONTAL : BombType.VERTICAL);
					k = j + 1;
					break;
				}
			}
		}
		bool haveMatch = comboScore > 0 ? true : false;
		bool[] returnList = new bool[2];
		returnList [0] = createCompleted;
		returnList [1] = haveMatch;
		return returnList;
	}

	public void ChangeSortLayer ()
	{		
		subRender.sortingOrder = SubRenderSoftLayer;
	}

	public void CreateScoreEffect (int scoreAdd)
	{
		GameplayControl.instance.score += scoreAdd;
		if (GameplayControl.instance.tGetScoreEffect.gameObject.activeInHierarchy == true) {
			string scoreStr = GameplayControl.instance.tGetScoreEffect.text.Replace ("+", "");
			GameplayControl.instance.tGetScoreEffect.text = (int.Parse (scoreStr) + scoreAdd).ToString ();
		} else {
			GameplayControl.instance.getScoreEffect.transform.position = transform.position;
			GameplayControl.instance.tGetScoreEffect.text = "+" + scoreAdd;
		}


		GameplayControl.instance.tGetScoreEffect.gameObject.SetActive (true);
	}

	public void AppearEffect ()
	{
		StartCoroutine (AppearEffectCoroutine ());
	}

	IEnumerator AppearEffectCoroutine ()
	{
		Vector3 startScale = new Vector3 (0, 0, 0);
		Vector3 endScale = new Vector3 (1F, 1F, 1F);
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime * 5;
			transform.localScale = Vector3.Lerp (startScale, endScale, t);
			yield return null;
		}
		transform.localScale = endScale;
	}

	public void SetGridLose ()
	{
		StartCoroutine (LoseEffect ());
	}

	IEnumerator LoseEffect ()
	{
		render.sprite = BlockColorDefine.instance.sprCellLose;
		float t = 0;
		Color startCol = new Color (1, 1, 1, 1);
		Color endCol = new Color (1, 1, 1, 0);
		while (t < 1) {
			t += Time.deltaTime;
			if (subRender.gameObject.activeInHierarchy == true) {
				subRender.color = Color.Lerp (startCol, endCol, t);
				yield return null;
			}
		}
	}

	void OnMouseDown ()
	{
		
		if (GameplayControl.instance.isUsingBooster == true) {
			switch (BoosterPopup.instance.boosterUsingType) {
			case BoosterType.BT_CLEAR_ONE_FIELD: 
				if (haveBlockCell == true) {
					Clear (0.1F);
				}
				break;
			case BoosterType.BT_CLEAR_TYPE: 
				for (int i = 0; i < GameplayControl.instance.listAllGrids.Count; i++) {
					Grid _grid = GameplayControl.instance.listAllGrids [i];
					if (_grid.haveBlockCell && _grid.gridCol == gridCol) {
						_grid.Clear (0.1F);
					}
				}
				break;
			case BoosterType.BT_CLEAR_AREA: 
				for (int i = 0; i < GameplayControl.instance.listAllGrids.Count; i++) {
					Grid _grid = GameplayControl.instance.listAllGrids [i];
					if (Mathf.Abs (_grid.X - X) <= 2 && Mathf.Abs (_grid.Y - Y) <= 2) {
						_grid.Clear (0.1F);
					}
				}
				break;
			}
			GameplayControl.instance.coinCount -= BoosterDefine.instance.GetBoosterPrice (BoosterPopup.instance.boosterUsingType);
			GameManager.SaveData ();
			BoosterPopup.instance.isUsingBooster = false;
			GameplayControl.instance.isUsingBooster = false;
			BoosterPopup.instance.Close ();
			Invoke ("CheckHaveStartBlockMatch", 0.2F);
		}
	}

	void CheckHaveStartBlockMatch ()
	{
		GameplayControl.instance.CheckHaveStartBlockMatch ();
	}
}

public enum BombType
{
	NOONE,
	HORIZONTAL,
	VERTICAL,
	SQUAREBOMB
}