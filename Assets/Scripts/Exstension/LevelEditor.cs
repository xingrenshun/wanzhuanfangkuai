using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class LevelEditor : EditorWindow
{
	public Sprite cell;
	public Sprite sprSelect;
	GameObject blockColorDefine;
	public GameObject blockColorDefinePrefap;
	GridEditor[,] grids;
	GridEditor selectedGrid;
	public int level;
	public TargetType target;
	public int targetAmount;
	public int stepCount;
	public BlockColor targetColor;
	float infoWidth = 300;
	float infoStart = 500;
	public int maxShapeSize, maxSquareSize, iceChance, fruitChance, fruitMode;

	[MenuItem ("Level Editor/Show Level Editor")]
	public static void ShowWindow ()
	{
		GetWindow<LevelEditor> ("Level Editor").Show (true);

	}

	public void Awake ()
	{
		this.maxSize = new Vector2 (800F, 500F);
		this.minSize = this.maxSize;
		blockColorDefine = (GameObject)Instantiate (blockColorDefinePrefap, new Vector3 (0, 0, 0), Quaternion.Euler (0, 0, 0));
		grids = new GridEditor[10, 10];
		for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 10; y++) {
				grids [x, y] = new GridEditor ();
				grids [x, y].x = x;
				grids [x, y].y = y;
				grids [x, y].sprCell = grids [x, y].sprGridCell;
			}
		}
		level = 1;
		LoadLevel ();
	}

	void OnDestroy ()
	{
		DestroyImmediate (blockColorDefine);
	}

	void DrawSprite (Rect pos, Sprite spr)
	{
		float _x = spr.rect.x / 2048;
		float _y = spr.rect.y / 2043;
		float _w = spr.rect.width / 2048;
		float _h = spr.rect.height / 2043;
		GUI.DrawTextureWithTexCoords (pos, spr.texture, new Rect (_x, _y, _w, _h));
	}

	void OnGUI ()
	{
		

		// create button 
		for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 10; y++) {
				if (GUI.Button (new Rect (50 * x, 50 * y, 48, 48), "")) {
					selectedGrid = grids [x, y];

				}

			}
		}

		// draw texture 

		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 20, infoWidth, 20), "Level", level.ToString ()), out level)) {
			Repaint ();
		} else if (level < 1) {
			level = 1;
			Repaint ();
		}

		if (GUI.Button (new Rect (infoStart, 40, infoWidth / 3, 20), "Load Level")) {
			LoadLevel ();
		}
		if (GUI.Button (new Rect (infoStart + infoWidth / 3, 40, infoWidth / 3, 20), "Build Level")) {
			BuildLevel ();
		}
		if (GUI.Button (new Rect (infoStart + infoWidth * 2 / 3, 40, infoWidth / 3, 20), "Clear")) {
			Clear ();
		}

		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 60, infoWidth, 20), "Move Count", stepCount.ToString ()), out stepCount)) {
			Repaint ();
		} else if (stepCount < 1) {
			stepCount = 30;
			Repaint ();
		}

		GUI.Label (new Rect (infoStart, 80, infoWidth, 40), "----- Target ------------------------------------------------");

		target = (TargetType)EditorGUI.EnumPopup (new Rect (infoStart, 100, infoWidth, 20), "Select Target Type", target);
		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 120, infoWidth, 20), "Target Amount", targetAmount.ToString ()), out targetAmount)) {
			Repaint ();
		} else if (targetAmount < 1) {
			targetAmount = 1;
			Repaint ();
		}
		if (target == TargetType.TGT_COLOR | target == TargetType.TGT_FRUIT) {
			targetColor = (BlockColor)EditorGUI.EnumPopup (new Rect (infoStart, 140, infoWidth, 20), "Target Color", targetColor);
		}

		// draw grid 
		for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 10; y++) {				
				DrawSprite (new Rect (50 * x, 50 * y, 48, 48), grids [x, y].sprCell);
				if (grids [x, y].haveColor == true) {					
					DrawSprite (new Rect (50 * x + 4, 50 * y + 4, 40, 40), grids [x, y].sprBlockCell);
				}
				if (grids [x, y].haveFruit == true) {					
					DrawSprite (new Rect (50 * x + 9, 50 * y + 9, 30, 30), grids [x, y].sprFruit);
				}
				if (grids [x, y].iceType != IceTypeEditor.NO_ICE) {					
					DrawSprite (new Rect (50 * x, 50 * y, 48, 48), grids [x, y].sprIce);
				}


			}
		}

		GUI.Label (new Rect (infoStart, 160, infoWidth, 40), "----- Cell Info --------------------------------------------");
		GUI.Label (new Rect (infoStart, 320, infoWidth, 40), "----- Other Info (-1 = no use) ---------------------------------------");

		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 340, infoWidth, 20), "Max Shape Size", "-1"), out maxShapeSize)) {
			Repaint ();
		} else if (maxShapeSize < -1 || maxShapeSize > 2) {
			maxShapeSize = -1;
		}
		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 360, infoWidth, 20), "Max Square Size", "-1"), out maxSquareSize)) {
			Repaint ();
		} else if (maxSquareSize < -1 || maxSquareSize > 2) {
			maxSquareSize = -1;
		}
		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 380, infoWidth, 20), "Fruit Chance", "-1"), out fruitChance)) {
			Repaint ();
		} else if (fruitChance < -1) {
			fruitChance = -1;
		}
		/*
		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 400, infoWidth, 20), "Ice Chance", "-1"), out iceChance)) {
			Repaint ();
		} else if (iceChance < -1) {
			iceChance = -1;
		}
		
		if (!int.TryParse (EditorGUI.TextField (new Rect (infoStart, 420, infoWidth, 20), "Fruit Mode", "-1"), out fruitMode)) {
			Repaint ();
		} else if (fruitMode != 2 || fruitMode != 3) {
			fruitMode = -1;
		}
		*/
		// draw selected 
		if (selectedGrid != null) {
			GUI.DrawTexture (new Rect (50 * selectedGrid.x, 50 * selectedGrid.y, 48, 48), sprSelect.texture);
		}
		// draw element 
		EditorGUI.BeginChangeCheck ();
		if (selectedGrid != null) {

			// dirt 

			selectedGrid.type = (ElementType)EditorGUI.EnumPopup (new Rect (infoStart, 180, infoWidth, 20), "Select Type", selectedGrid.type);
			if (selectedGrid.type == ElementType.ET_FILL) {
				selectedGrid.isBlank = false;
				selectedGrid.dirtType = DirtTypeEditor.NO_DIRT;

				// color
				selectedGrid.haveColor = GUI.Toggle (new Rect (infoStart, 200, infoWidth, 20), selectedGrid.haveColor, "Have Jelly");
				selectedGrid.bColor = (BlockColor)EditorGUI.EnumPopup (new Rect (infoStart, 220, infoWidth, 20), "Select Color", selectedGrid.bColor);
				// fruit
				selectedGrid.haveFruit = GUI.Toggle (new Rect (infoStart, 240, infoWidth, 20), selectedGrid.haveFruit, "Have Fruit");
				selectedGrid.fruitColor = (BlockColor)EditorGUI.EnumPopup (new Rect (infoStart, 260, infoWidth, 20), "Select Fruit Color", selectedGrid.fruitColor);
				if (selectedGrid.haveFruit == true) {
					selectedGrid.haveColor = true;
				}

				// Ice
				selectedGrid.iceType = (IceTypeEditor)EditorGUI.EnumPopup (new Rect (infoStart, 280, infoWidth, 20), "Select Ice Type", selectedGrid.iceType);
				if (selectedGrid.iceType != IceTypeEditor.NO_ICE) {
					selectedGrid.haveColor = true;
				}
			} else if (selectedGrid.type == ElementType.ET_DIRT) {
				selectedGrid.isBlank = false;
				selectedGrid.haveColor = false;
				selectedGrid.haveFruit = false;
				selectedGrid.iceType = IceTypeEditor.NO_ICE;
				selectedGrid.dirtType = (DirtTypeEditor)EditorGUI.EnumPopup (new Rect (infoStart, 200, infoWidth, 20), "Select Dirt", selectedGrid.dirtType);
			} else if (selectedGrid.type == ElementType.ET_BLANK) {
				selectedGrid.haveColor = false;
				selectedGrid.haveFruit = false;
				selectedGrid.iceType = IceTypeEditor.NO_ICE;
				selectedGrid.dirtType = DirtTypeEditor.NO_DIRT;
				selectedGrid.isBlank = true;
			} else if (selectedGrid.type == ElementType.ET_NOONE) {
				selectedGrid.haveColor = false;
				selectedGrid.haveFruit = false;
				selectedGrid.iceType = IceTypeEditor.NO_ICE;
				selectedGrid.isBlank = false;
				selectedGrid.dirtType = DirtTypeEditor.NO_DIRT;
				
			}


		}
		if (EditorGUI.EndChangeCheck ()) {
			if (GUI.changed) {
				Debug.Log ("Gui Change");
				Repaint ();
			}
		}

	}

	public void BuildLevel ()
	{
		LevelDataModule levelData = new LevelDataModule ();
		levelData.level = level;
		levelData.fruitChance = fruitChance;
		levelData.fruitMode = fruitMode;
		levelData.iceChance = iceChance;
		levelData.maxShapeSize = maxShapeSize;
		levelData.maxSquareSize = maxSquareSize;
		levelData.moves = stepCount;
		levelData.targets = new List<TargetModule> ();
		TargetModule _target = new TargetModule ();
		_target.type = target;
		_target.amount = targetAmount;
		_target.color = targetColor;
		levelData.targets.Add (_target);

		levelData.elements = new List<LeveDataElementModule> ();
		for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 10; y++) {
				if (grids [x, y].type != ElementType.ET_NOONE) {
					LeveDataElementModule _element = new LeveDataElementModule ();
					levelData.elements.Add (_element);
					_element.type = grids [x, y].type;
					_element.color = grids [x, y].bColor;
					_element.fruitColor = grids [x, y].fruitColor;
					_element.fruit = grids [x, y].haveFruit;
					_element.ice = grids [x, y].iceType == IceTypeEditor.NO_ICE ? false : true;
					_element.iceType = (int)grids [x, y].iceType > 0 ? 4 - (int)grids [x, y].iceType : -1;
					_element.x = x;
					_element.y = y;
					_element.amount = (int)grids [x, y].dirtType;


				}
			}
		}
		//Debug.Log ("element count : " + levelData.elements.Count);
		string json = JsonUtility.ToJson (levelData);
		string path = "Assets/Resources/LevelMap/";
		using (FileStream fs = new FileStream (path + "Level_" + level + ".json", FileMode.Create)) {
			using (StreamWriter writer = new StreamWriter (fs)) {
				writer.Write (json);
			}
		}
		AssetDatabase.Refresh ();
		//Debug.Log ("Build Success");
		EditorUtility.DisplayDialog ("Succeeded", "Build Level " + level + " completed with result of succeeded", "ok");
	}

	public void LoadLevel ()
	{
		
		TextAsset ta = Resources.Load ("LevelMap/Level_" + level) as TextAsset;
		if (ta == null) {
			EditorUtility.DisplayDialog ("Error", "Level does not exist", "Ok");
			return;
		}
		Clear ();
		LevelDataModule levelData = JsonUtility.FromJson<LevelDataModule> (ta.text);
		for (int i = 0; i < levelData.elements.Count; i++) {
			LeveDataElementModule element = levelData.elements [i];
			int x = element.x;
			int y = element.y;
			grids [x, y].type = element.type;
			if (element.type == ElementType.ET_FILL) {
				grids [x, y].bColor = element.color;
				grids [x, y].fruitColor = element.fruitColor;
				grids [x, y].haveFruit = element.fruit;
				grids [x, y].haveColor = element.type == ElementType.ET_FILL ? true : false;
				grids [x, y].iceType = element.ice == false ? IceTypeEditor.NO_ICE : element.iceType <= 1 ? IceTypeEditor.LEVEL_3 : element.iceType == 2 ? IceTypeEditor.LEVEL_2 : element.iceType == 3 ? IceTypeEditor.LEVEL_1 : IceTypeEditor.NO_ICE;
				//Debug.Log (element.iceType);
			} else if (element.type == ElementType.ET_DIRT) {
				grids [x, y].dirtType = element.type != ElementType.ET_DIRT ? DirtTypeEditor.NO_DIRT : (DirtTypeEditor)element.amount;
			} else if (element.type == ElementType.ET_BLANK) {
				grids [x, y].isBlank = element.type == ElementType.ET_BLANK ? true : false;
			}
		}
		target = levelData.targets [0].type;
		targetAmount = levelData.targets [0].amount;
		targetColor = levelData.targets [0].color;
		stepCount = levelData.moves;
		fruitChance = levelData.fruitChance;
		fruitMode = levelData.fruitMode;
		iceChance = levelData.iceChance;
		maxShapeSize = levelData.maxShapeSize;
		maxSquareSize = levelData.maxSquareSize;
		Repaint ();
	}

	void Clear ()
	{
		for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 10; y++) {
				grids [x, y].haveColor = false;
				grids [x, y].haveFruit = false;
				grids [x, y].iceType = IceTypeEditor.NO_ICE;
				grids [x, y].isBlank = false;
				grids [x, y].dirtType = DirtTypeEditor.NO_DIRT;
				grids [x, y].type = ElementType.ET_NOONE;

			}
		}
	}
}

[System.Serializable]
public class GridEditor
{
	public Sprite sprCell, sprBlockCell, sprFruit, sprIce;
	BlockColor _bColor = BlockColor.BC_0, _fruitColor = BlockColor.BC_0;
	public ElementType type;

	public BlockColor bColor {
		get { return _bColor; }
		set {
			_bColor = value;
			sprBlockCell = BlockColorDefine.instance.GetBlockSpriteByColor (value);
		}
	}

	public BlockColor fruitColor {
		get { return _fruitColor; }
		set {
			_fruitColor = value;
			sprFruit = BlockColorDefine.instance.GetFruitSpriteByColor (value);
		}
	}

	DirtTypeEditor _dirtType;

	public DirtTypeEditor dirtType {
		get { return _dirtType; }
		set {
			_dirtType = value;
			if (value == DirtTypeEditor.DIRT2) {
				sprCell = BlockColorDefine.instance.sprGridDirt2;
			} else if (value == DirtTypeEditor.DIRT1) {
				sprCell = BlockColorDefine.instance.sprGridDirt;
			} else {				
				sprCell = sprGridCell;
			}
		}
	}

	IceTypeEditor _iceType;

	public IceTypeEditor iceType {
		get { return _iceType; }
		set {
			_iceType = value;
			switch (value) {
			case IceTypeEditor.NO_ICE: 
				
				break;		
			case IceTypeEditor.LEVEL_1: 
				
				sprIce = BlockColorDefine.instance.sprIce1;
				break;
			case IceTypeEditor.LEVEL_2: 				
				sprIce = BlockColorDefine.instance.sprIce2;
				break;
			case IceTypeEditor.LEVEL_3: 				
				sprIce = BlockColorDefine.instance.sprIce3;
				break;
			}
		}
	}

	bool _isBlank;

	public bool isBlank {
		get { return _isBlank; }
		set {
			_isBlank = value;
			sprCell = value == true ? BlockColorDefine.instance.sprGridBlank : sprGridCell;
		}
	}

	public bool haveColor = false, haveFruit = false;
	public int x, y;

	public Sprite sprGridCell {
		get {
			return (11 * x + y) % 2 == 0 ? BlockColorDefine.instance.sprGrid1 : BlockColorDefine.instance.sprGrid2;
		}
	}

	public GridEditor ()
	{
		type = ElementType.ET_NOONE;
		bColor = BlockColor.BC_0;
		sprBlockCell = BlockColorDefine.instance.GetBlockSpriteByColor (bColor);


	}
}

public enum DirtTypeEditor
{
	NO_DIRT,
	DIRT1,
	DIRT2
}

public enum IceTypeEditor
{
	NO_ICE,
	LEVEL_1,
	LEVEL_2,
	LEVEL_3
}

#endif
		

