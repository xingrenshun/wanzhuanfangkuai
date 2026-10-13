using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayControl : MonoBehaviour
{
	public bool isUsingBooster = false;
	public GameObject targetPopup;
	public GameObject bottomPoint, blockBgPos, blockBg;
	int _score;
	public SpriteRenderer textEffectRender;
	public GameObject coinPrefap;
	public bool isFailed = false, isWin = false;
	public List<Grid> listAllGrids;
	int _comboMatchCount;
	public GameObject getScoreEffect;
	public Text tGetScoreEffect;
	public GameObject board;
	int _coinCount;
	[HideInInspector]
	public int moveCountRemain;

	public int coinCount {
		get { return _coinCount; }
		set {
			_coinCount = value;
			UITopControl.instance.tCoin.text = value.ToString ();
			GameManager.SaveData ();
		}
	}

	int _moveCount;
	public GameObject losePopup, winPopup;

	public int moveCount {
		get { return _moveCount; }
		set {
			_moveCount = value;
			UITopControl.instance.tMoveCount.text = value.ToString ();
			if (value <= 0) {
				StartCoroutine (LoseCoroutine ());
			}
		}
	}

	IEnumerator LoseCoroutine ()
	{
		yield return new WaitForSecondsRealtime (1F);
		ClearEffect[] clearEffets = GameObject.FindObjectsOfType<ClearEffect> ();
		int c = 0;
		while (clearEffets.Length > 0) {
			yield return new WaitForSecondsRealtime (0.1F);
			clearEffets = GameObject.FindObjectsOfType<ClearEffect> ();
			c++;
			if (c >= 10) {
				break;
			}
		}
		if (isWin == true) {
			yield break;
		}
		if (isFailed == false && isWin == false) {
			isFailed = true;
			Debug.Log ("Thua cuoc");
			//admob
			//Admob.instance.ShowIads ();
			losePopup.SetActive (true);
		}
	}

	int _targetCount;

	public int targetCount {
		get { return _targetCount; }
		set {
			_targetCount = value;
			UITopControl.instance.tTargetCount.text = value + "/" + UITopControl.instance.target.amount;
			if (value >= UITopControl.instance.target.amount && UITopControl.instance.target.amount > 0 && isWin == false) {
				isWin = true;
				StartCoroutine (WinCoroutine ());
			}
		}
	}

	IEnumerator WinCoroutine ()
	{
		listAllGrids.Shuffle ();
		int i = 0;
		List<Grid> listBombGrids = new List<Grid> ();
		moveCountRemain = moveCount;
		while (moveCount > 0) {
			
			if (listAllGrids [i].haveBlockCell == true) {
				moveCount--;
				AudioManager.instance.PlaySound (AudioClipType.AC_BONUS);
				listAllGrids [i].CreateBomb (BombType.SQUAREBOMB);
				yield return new WaitForSecondsRealtime (0.2F);
				listBombGrids.Add (listAllGrids [i]);
			
			}
			i++;
			if (i >= listAllGrids.Count) {
				break;
			}

		}
		foreach (Grid _gridBomb in listBombGrids) {
			_gridBomb.Clear (0.2F);
		}
		yield return new WaitForSecondsRealtime (3F);
		winPopup.SetActive (true);
		//Admob.instance.ShowIads ();
	}

	public int fruitChance;

	public int comboMatchCount {
		get {
			return _comboMatchCount;
		}
		set {
			_comboMatchCount = value;
			GameManager.dataSave.comboMatchCount = value;
			if (UITopControl.instance.target.type == TargetType.TGT_COMBO) {
				targetCount = value;
			}
		}
	}

	public int score {
		get { return _score; }
		set {
			_score = value;
			if (UITopControl.instance.target.type == TargetType.TGT_SCORE) {
				targetCount = value;
			}
			//UITopControl.instance.tScore.text = value.ToString ();
			if (bestScore < value) {
				bestScore = value;
			}
		}
	}

	int _bestScore;

	public int bestScore {
		get { return _bestScore; }
		set {
			_bestScore = value;
			//UITopControl.instance.tBest.text = value.ToString ();
		}
	}

	public Block moveBlock;
	Block _startBlock;
	public List<Block> listStartBlock;

	public Block startBlock {
		get { return _startBlock; }
		set {
			if (value != null) {
				_startBlock = value;
				moveBlockId = value.id;
				if (blockMoveOutCoroutine == null) {
					blockMoveOutCoroutine = MoveBlockMoveOut ();
					StartCoroutine (blockMoveOutCoroutine);
				}
			}
		}
	}

	public GameObject gridPrefap;
	public Grid[,] grids;
	int _moveBlockId;
	public static GameplayControl instance;

	public int moveBlockId {
		get { return _moveBlockId; }
		set {
			
			_moveBlockId = value;

		}
	}

	bool canMoveBlock = false;

	void Awake ()
	{
		Application.targetFrameRate = 60;
		instance = this;
	}

	public void CreateBoard ()
	{
		if (GameManager.dataSave.heart > 0) {
			if (GameManager.dataSave.heart == 5) {
				TimerControl.instance.InitNextHeartTime ();
			}
			GameManager.dataSave.heart--;

		} else {
			LoadSceneFading.LoadScene ("scn_selectLevel");
		}
		grids = new Grid[GameManager.xCount, GameManager.yCount];
		listAllGrids = new List<Grid> ();
		score = GameManager.dataSave.currentScore;
		bestScore = GameManager.dataSave.bestScore;
		comboMatchCount = GameManager.dataSave.comboMatchCount;
		for (int x = 0; x < GameManager.xCount; x++) {
			for (int y = 0; y < GameManager.yCount; y++) {
				GameObject gridObj = (GameObject)Instantiate (gridPrefap, new Vector3 (board.transform.position.x - (float)GameManager.xCount / 2 + 0.5F + x, board.transform.position.y + (float)GameManager.yCount / 2 - 0.5F - y, 0), Quaternion.Euler (0, 0, 0));
				gridObj.transform.parent = board.transform;
				Grid _grid = gridObj.GetComponent<Grid> ();
				listAllGrids.Add (_grid);
				_grid.X = x;
				_grid.Y = y;
				grids [x, y] = _grid;
				_grid.render.sprite = _grid.sprCell;
			}
		}
		moveBlock.gameObject.SetActive (false);

		isFailed = false;
		score = 0;



		// init data from file 
		if (!GameManager.levelsDic.ContainsKey (GameManager.currentLevel)) {
			GameManager.LoadLevel (GameManager.currentLevel);
		}

		LevelDataModule levelData = GameManager.levelsDic [GameManager.currentLevel];
		moveCount = levelData.moves;
		UITopControl.instance.Init ();
		// set grid 
		for (int i = 0; i < levelData.elements.Count; i++) {
			LeveDataElementModule elementModule = levelData.elements [i];
			grids [elementModule.x, elementModule.y].SetGrid (elementModule.type, elementModule.color, elementModule.fruitColor, elementModule.fruit, elementModule.ice, elementModule.iceType, elementModule.amount);
		}
		// init new start block
		for (int i = 0; i < 3; i++) {
			int rnd = GameManager.GetRandomBlockId ();
			listStartBlock [i].InitBlock (rnd);
			listStartBlock [i].blockColor = (BlockColor)Random.Range (1, BlockColorDefine.instance.listBlockColorStruct.Count + 1);
			listStartBlock [i].haveMatchPos = true;
			listStartBlock [i].gameObject.SetActive (true);
			listStartBlock [i].NewBlockEffect ();
		}
		coinCount = GameManager.dataSave.coin;
		fruitChance = levelData.fruitChance;
		targetCount = 0;


	}

	public void CreateBoardEffect ()
	{
		StartCoroutine (CreateBoardEffectCoroutine ());
	}

	IEnumerator CreateBoardEffectCoroutine ()
	{
		for (int x = 0; x < GameManager.xCount; x++) {
			for (int y = 0; y < GameManager.yCount; y++) {
				grids [x, y].AppearEffect ();
				grids [GameManager.xCount - 1 - x, GameManager.yCount - 1 - y].AppearEffect ();
				grids [x, y].subGrid.SetActive (false);
				grids [x, y].haveBlockCell = false;
				//grids [x, y].render.sprite = BlockColorDefine.instance.sprGrid;
				grids [x, y].subRender.color = new Color (1, 1, 1, 1);
				grids [x, y].hintGrid.SetActive (false);
				//yield return null;
			}
		}
		yield return null;
	}

	void ClearDataSave ()
	{
		GameManager.dataSave.currentScore = 0;
		for (int i = 0; i < GameManager.xCount * GameManager.yCount; i++) {
			GameManager.dataSave.board [i] = 0;
			GameManager.dataSave.boardColor [i] = 0;
			if (i < 3) {
				GameManager.dataSave.startIds [i] = -1;
				GameManager.dataSave.startBlockColor [i] = -1;
				GameManager.dataSave.startBlockVisible [i] = 1;
			}
		}
		GameManager.SaveData ();
	}

	void Start ()
	{
		
		InitCamera ();
		CreateBoard ();
		//SUGame.Get<SUAdmob> ().ShowBanner ();
		StartCoroutine (InitCameraCoroutine ());

	}

	void OnApplicationFocus (bool hasFocus)
	{
		if (hasFocus == true) {
			//SUGame.Get<SUAdmob> ().ShowIads ();
		}
	}

	IEnumerator InitCameraCoroutine ()
	{
		yield return null;
		InitCamera ();
		yield return null;
		InitCamera ();
	}

	Grid lastNearlestGrid = null;
	Grid nearlestGrid = null;

	void ClearHighLight ()
	{
		for (int x = 0; x < GameManager.xCount; x++) {
			for (int y = 0; y < GameManager.yCount; y++) {				
				if (GameplayControl.instance.grids [x, y].beforeBlockSprite != null) {
					GameplayControl.instance.grids [x, y].subRender.sprite = GameplayControl.instance.grids [x, y].beforeBlockSprite;
					GameplayControl.instance.grids [x, y].beforeBlockSprite = null;
				}
			}
		}
	}

	void Update ()
	{
		
		if (Input.GetKeyUp (KeyCode.Escape)) {
			System.Diagnostics.Process.GetCurrentProcess ().Kill ();
		}
		if (moveBlock.gameObject.activeInHierarchy == true && Input.GetMouseButton (0) && canMoveBlock == true) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			moveBlock.gameObject.transform.position = mousePos + new Vector3 (0, 3, 10);
			//moveBlock.gameObject.transform.localScale = new Vector3 (1, 1, 1);
			float minDistance = GetNearlestGrid ();
			if (minDistance > 1) {
				ClearHighLight ();
				if (lastNearlestGrid != null) {
					lastNearlestGrid.ClearInit ();
				}
			} else {
				if (nearlestGrid != lastNearlestGrid) {
					if (lastNearlestGrid != null) {
						lastNearlestGrid.ClearInit ();
					}
					lastNearlestGrid = nearlestGrid;
					ClearHighLight ();
					nearlestGrid.Init (moveBlock.id, false, false, false);
				}
			}
			if (TutorialControl.instance.isShowingTutorial == true) {
				if (TutorialControl.instance.tutorialHand.gameObject.activeInHierarchy == true) {
					TutorialControl.instance.tutorialHand.render.enabled = false;
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			if (TutorialControl.instance.isShowingTutorial == true) {
				if (TutorialControl.instance.tutorialHand.gameObject.activeInHierarchy == true) {
					TutorialControl.instance.tutorialHand.render.enabled = true;
				}
			}
			if (startBlock != null) {
				if (canMoveBlock == true) {
					ClearHighLight ();
					//Debug.Log ("set grid");
					moveBlock.gameObject.SetActive (false);
					canMoveBlock = false;
					AudioManager.instance.PlaySound (AudioClipType.AC_PUT_BLOCK);
					StartCoroutine (OnMouseRelease ());
				} else {
					ClearHighLight ();
					moveBlock.gameObject.SetActive (false);
					if (blockMoveOutCoroutine != null) {
						StopCoroutine (blockMoveOutCoroutine);
						blockMoveOutCoroutine = null;
						startBlock.gameObject.SetActive (true);
					}
					startBlock = null;
				}
			}
		}
	}



	IEnumerator OnMouseRelease ()
	{
		float minDistance = GetNearlestGrid ();
		nearlestGrid.ClearInit ();

		if (minDistance <= 1) {	
			
			bool[] createBlock = nearlestGrid.Init (moveBlock.id, true, false, false);
			if (createBlock [0] == false) {
				// neu khong dat xuong duoc
				startBlock.gameObject.SetActive (true);

			} else {
				// neu dat xuong duoc 

				moveCount--;
				if (createBlock [1] == true) {
						
					// neu dat xuong duoc va co an ngang hoac doc 
					comboMatchCount++;
					/*
						if (GameplayControl.instance.comboMatchCount == 2) {
							AudioManager.instance.PlaySound (AudioClipType.AC_COOL);
							var spr = TextEffectDefine.instance.GetTextEffectSpriteByType (TextEffectType.TE_COOL);
							if (spr != null) {
								textEffectRender.gameObject.SetActive (true);
								textEffectRender.sprite = spr;
							}
						} else if (GameplayControl.instance.comboMatchCount == 3) {
							AudioManager.instance.PlaySound (AudioClipType.AC_GOOD);
							var spr = TextEffectDefine.instance.GetTextEffectSpriteByType (TextEffectType.TE_GOOD);
							if (spr != null) {
								textEffectRender.gameObject.SetActive (true);
								textEffectRender.sprite = spr;
							}
						} else if (GameplayControl.instance.comboMatchCount >= 4) {
							AudioManager.instance.PlaySound (AudioClipType.AC_EXCELENT);
							var spr = TextEffectDefine.instance.GetTextEffectSpriteByType (TextEffectType.TE_EXCELENT);
							if (spr != null) {
								textEffectRender.gameObject.SetActive (true);
								textEffectRender.sprite = spr;
							}
						}
						*/

				} else {
					comboMatchCount = 0;
				}
			}
		
		
		
		} else {
			startBlock.gameObject.SetActive (true);
		}

		startBlock = null;
		CreateNewStartBlock ();
		yield return new WaitForSeconds (0.05F * 9);
		bool isAllStartBlockVisible = true;
		for (int i = 0; i < listStartBlock.Count; i++) {
			if (listStartBlock [i].gameObject.activeInHierarchy == true) {
				isAllStartBlockVisible = false;
			}
		}
		if (isAllStartBlockVisible == false && moveBlock.gameObject.activeInHierarchy == false) {
			CheckHaveStartBlockMatch ();
			yield return null;
			// save data 
			//InitDataAndSave ();
		}

	}

	public void InitNewGame ()
	{
		CreateBoardEffect ();
		isFailed = false;
		score = 0;

		for (int i = 0; i < 3; i++) {
			int rnd = GameManager.GetRandomBlockId ();
			listStartBlock [i].InitBlock (rnd);
			listStartBlock [i].blockColor = (BlockColor)Random.Range (1, BlockColorDefine.instance.listBlockColorStruct.Count + 1);
			listStartBlock [i].haveMatchPos = true;
			listStartBlock [i].gameObject.SetActive (true);
			listStartBlock [i].NewBlockEffect ();
		}

		//if (GameManager.dataSave.bestScore == 0) {
		//TutorialControl.instance.ShowTutorial ();
		//}


	}


	void InitDataAndSave ()
	{
		if (isFailed == true) {
			//ClearDataSave ();
			return;
		}
		GameManager.dataSave.currentScore = score;
		GameManager.dataSave.bestScore = bestScore;
		for (int x = 0; x < GameManager.xCount; x++) {
			for (int y = 0; y < GameManager.yCount; y++) {
				GameManager.dataSave.board [x * GameManager.xCount + y] = grids [x, y].haveBlockCell == true ? 1 : 0;
				GameManager.dataSave.boardColor [x * GameManager.xCount + y] = (int)grids [x, y].gridCol;
			}
		}
		for (int i = 0; i < 3; i++) {
			GameManager.dataSave.startIds [i] = listStartBlock [i].id;
			GameManager.dataSave.startBlockColor [i] = (int)listStartBlock [i].blockColor;
			GameManager.dataSave.startBlockVisible [i] = listStartBlock [i].gameObject.activeInHierarchy == true ? 1 : 0;
		}
		GameManager.SaveData ();
	}

	List<Grid> listMatchH = new List<Grid> ();
	List<Grid> listMatchV = new List<Grid> ();

	public List<Grid> CheckMatch (Grid _grid)
	{
		// check ngang 
		listMatchH.Clear ();
		listMatchV.Clear ();
		for (int x = 0; x < GameManager.xCount; x++) {
			if (grids [x, _grid.Y].subGrid.activeInHierarchy == true || grids [x, _grid.Y].isBlank == true) {
				listMatchH.Add (grids [x, _grid.Y]);
			}
		}
		for (int y = 0; y < GameManager.yCount; y++) {
			if (grids [_grid.X, y].subGrid.activeInHierarchy == true || grids [_grid.X, y].isBlank == true) {
				listMatchV.Add (grids [_grid.X, y]);
			}
		}

		if (listMatchH.Count == GameManager.xCount && listMatchV.Count == GameManager.yCount) {
			for (int i = 0; i < listMatchH.Count; i++) {
				if (listMatchH [i] != _grid) {
					listMatchV.Add (listMatchH [i]);
				}
			}
			listMatchV.Sort ((a, b) => (Mathf.Abs ((a.Y + a.X) - (_grid.Y + _grid.X)).CompareTo (Mathf.Abs ((b.Y + b.X) - (_grid.Y + _grid.X)))));
			return listMatchV;
		}
		if (listMatchH.Count == GameManager.xCount) {
			listMatchH.Sort ((a, b) => (Mathf.Abs (a.X - _grid.X)).CompareTo (Mathf.Abs (b.X - _grid.X)));
			return listMatchH;
		}
		if (listMatchV.Count == GameManager.yCount) {
			listMatchV.Sort ((a, b) => (Mathf.Abs (a.Y - _grid.Y)).CompareTo (Mathf.Abs (b.Y - _grid.Y)));
			return listMatchV;
		}

		listMatchH.Clear ();
		return listMatchH;
	}

	public void CheckHaveStartBlockMatch ()
	{
		for (int i = 0; i < listStartBlock.Count; i++) {
			
			Block _startBlock = listStartBlock [i];
			_startBlock.haveMatchPos = true;
			_startBlock.listGridsCanMatch.Clear ();
			bool haveMatchPos = false;
			int gridMatchCount = 0;
			for (int x = 0; x < GameManager.xCount; x++) {
				for (int y = 0; y < GameManager.yCount; y++) {
					if (grids [x, y].Init (_startBlock.id, true, true, false) [0] == true) {
						haveMatchPos = true;
						_startBlock.listGridsCanMatch.Add (grids [x, y]);
						gridMatchCount++;
						if (gridMatchCount > 2) {
							break;
						}
					}
				}
				if (gridMatchCount > 2) {
					break;
				}
			}
			_startBlock.haveMatchPos = haveMatchPos;

		}



		// check Lost 

		bool isLose = true;
		int startBlockShowCount = 0;
		int blockShowOnlyPos = 0;
		for (int i = 0; i < listStartBlock.Count; i++) {
			if (listStartBlock [i].gameObject.activeInHierarchy == true && listStartBlock [i].haveMatchPos == true) {
				isLose = false;
				startBlockShowCount++;
				blockShowOnlyPos = i;
			}
		}

		if (startBlockShowCount == 1) {
			Block blockShowOnly = listStartBlock [blockShowOnlyPos];
			if (blockShowOnly.listGridsCanMatch.Count == 1) {
				blockShowOnly.listGridsCanMatch [0].Init (blockShowOnly.id, false, false, true);
			}
		}

		if (isLose == true && isFailed == false) {
			// thua 
			//isFailed = true;
			Debug.Log ("Thua cuoc");
			StartCoroutine (LoseCoroutine ());
			//ShowBooster (true);
			//ClearDataSave ();
		}
	}

	public void ShowBooster (bool isShowWhenLose)
	{
		UITopControl.instance.ShowBoosterPopup ();
	}


	void CreateNewStartBlock ()
	{
		bool allStartBlockIsDisable = true;
		for (int i = 0; i < listStartBlock.Count; i++) {
			Block _startBlock = listStartBlock [i];
			if (_startBlock.gameObject.activeInHierarchy == true) {
				allStartBlockIsDisable = false;
				break;
			}
		}

		if (allStartBlockIsDisable == true) {
			List<BlockCell> listBlockCellVisible = new List<BlockCell> ();
			for (int i = 0; i < listStartBlock.Count; i++) {
				Block _startBlock = listStartBlock [i];
				_startBlock.gameObject.SetActive (true);
				_startBlock.InitBlock (GameManager.GetRandomBlockId ());
				_startBlock.NewBlockEffect ();
				for (int _x = 0; _x < 5; _x++) {
					for (int _y = 0; _y < 5; _y++) {
						if (_startBlock.listBlockCell [_x, _y].gameObject.activeInHierarchy == true) {
							listBlockCellVisible.Add (_startBlock.listBlockCell [_x, _y]);
						}
					}
				}


			}
			if (UITopControl.instance.target.type == TargetType.TGT_FRUIT && fruitChance > 0) {
				fruitChance--;
				listBlockCellVisible.Shuffle ();
				listBlockCellVisible [0].haveFruit = true;
			}


		}

	}


	float  GetNearlestGrid ()
	{
		float minDistance = 100;
		for (int x = 0; x < GameManager.xCount; x++) {
			for (int y = 0; y < GameManager.yCount; y++) {
				float distance = (moveBlock.transform.position - grids [x, y].transform.position).magnitude;
				if (distance < minDistance) {
					minDistance = distance;
					nearlestGrid = grids [x, y];
				}
			}
		}
		return minDistance;
	}

	public IEnumerator blockMoveOutCoroutine;

	IEnumerator MoveBlockMoveOut ()
	{
		moveBlock.gameObject.SetActive (true);
		//Debug.Log ("id is : " + startBlock.id);
		moveBlock.InitBlock (startBlock.id);
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				moveBlock.listBlockCell [x, y].haveFruit = startBlock.listBlockCell [x, y].haveFruit;
			}
		}
		moveBlock.blockColor = startBlock.blockColor;
		Vector3 startScale = new Vector3 (0.5F, 0.5F, 1);
		Vector3 endScale = new Vector3 (0.9F, 0.9F, 1);
		Vector3 startPos = startBlock.transform.position;
		float t = 0; 
		moveBlock.gameObject.transform.position = startPos;
		moveBlock.gameObject.transform.localScale = startScale;
		while (t < 1) {
			yield return null;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3 (0, 3, 10);
			t += Time.deltaTime * 20;
			moveBlock.gameObject.transform.position = Vector3.Lerp (startPos, mousePos, t);
			moveBlock.gameObject.transform.localScale = Vector3.Lerp (startScale, endScale, t);
		}

		moveBlock.gameObject.transform.localScale = endScale;
		canMoveBlock = true;
		blockMoveOutCoroutine = null;
	}

	void InitCamera ()
	{
		//float standard = (720F / 1280F) * 8.4F;
		float defaultCameraSize = 9.6F;
		float standard = (720F / 1280F) * defaultCameraSize;
		float aspect = (float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight;
		float size = standard / aspect; 
		if (size < 7.2F) {
			size = 7.2F;
		}
		/*

		if (size < 8.2F) {
			size = 8.2F;
		}
		*/
		Camera.main.orthographicSize = size;

		// 
		//bottomPoint

		//RectTransform blockBgRect = blockBgPos.GetComponent<RectTransform> ();
		//Debug.Log (blockBgRect.transform.position.y + "_" + blockBg.transform.position.y);
		//int adsize = SUGame.Get<SUAdmob> ().GetBannerHeight ();
		//int adsize = 90;
		//Debug.Log ("adsize : " + adsize);
		//blockBgRect.anchoredPosition = new Vector2 (0, adsize + 100);
		//blockBg.transform.position = blockBgRect.transform.position;

		//Debug.Log (blockBgRect.transform.position.y + "_" + blockBg.transform.position.y);

	}

	public void StartNewGame ()
	{
		StartCoroutine (NewGameAnimation ());
	}

	IEnumerator NewGameAnimation ()
	{
		

		Vector3 bottomStartPos = blockBg.transform.position;
		Vector3 bottomEndPos = blockBgPos.transform.position;
		Vector3 boardStartScle = new Vector3 (0, 0, 0);
		Vector3 boardEndScale = new Vector3 (1, 1, 1);
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime * 3;
			yield return null;
			blockBg.transform.position = Vector3.Lerp (bottomStartPos, bottomEndPos, t);
			board.transform.localScale = Vector3.Lerp (boardStartScle, boardEndScale, t);
		}
		InitNewGame ();
	}

	IEnumerator LoseAnimation ()
	{
		

		Vector3 bottomStartPos = blockBg.transform.position;
		Vector3 bottomEndPos = new Vector3 (0, blockBg.transform.position.y - 6, 0);
		Vector3 boardStartScle = new Vector3 (1, 1, 1);
		Vector3 boardEndScale = new Vector3 (0, 0, 0);
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime * 3;
			yield return null;
			blockBg.transform.position = Vector3.Lerp (bottomStartPos, bottomEndPos, t);
			board.transform.localScale = Vector3.Lerp (boardStartScle, boardEndScale, t);
		}
		AudioManager.instance.PlaySound (AudioClipType.AC_LOSE);
		//LosePopup.SetActive (true);

	}

}
