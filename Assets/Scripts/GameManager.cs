using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	public static List<BlockModule> listBlockData;
	public static DataSaveModule dataSave;
	public static Dictionary<string,GameTextElement> gameTextsDic;
	//public static List<TutorialModule> listTutorial;
	public static Dictionary<int,LevelDataModule> levelsDic;
	public static int xCount = 10, yCount = 10;
	public static int currentLevel = 1;

	public static bool isNetworkConnected {
		get {
			return Application.internetReachability == NetworkReachability.NotReachable ? false : true;
		}
	}

	public static void LoadLevel (int level)
	{
		
		if (!levelsDic.ContainsKey (level)) {
			TextAsset ta = Resources.Load ("LevelMap/Level_" + level) as TextAsset;
			LevelDataModule levelData = JsonUtility.FromJson<LevelDataModule> (ta.text);
			levelsDic.Add (level, levelData);
		}
	}

	public static ElementType ConvertElementTypeStringToElementType (string str)
	{
		switch (str) {
		case "blank":
			return ElementType.ET_BLANK;
		case "dirt":
			return ElementType.ET_DIRT;
		case "fill":
			return ElementType.ET_FILL;
		default :
			return ElementType.ET_BLANK;
		}
	}

	public static TargetType ConvertTargetTypeStringToTargetType (string str)
	{
		switch (str) {
		case "color": 
			return TargetType.TGT_COLOR;
		case "combo": 
			return TargetType.TGT_COMBO;
		case "dirt": 
			return TargetType.TGT_DIRT;
		case "double": 
			return TargetType.TGT_DOUBLE;
		case "fruit": 
			return TargetType.TGT_FRUIT;
		case "ice": 
			return TargetType.TGT_ICE;
		case "score": 
			return TargetType.TGT_SCORE;
		default :
			return TargetType.TGT_SCORE;
		}
	}


	public static void LoadDataSaved ()
	{
		levelsDic = new Dictionary<int, LevelDataModule> ();
		string str = PlayerPrefs.GetString ("fuckyou");
		//str = "";
		if (str != null && str != "") {
			//Debug.Log (str);
			dataSave = JsonUtility.FromJson<DataSaveModule> (str);

		} else {
			dataSave = new DataSaveModule ();
			dataSave.language = GameConfig.default_language;
			dataSave.levelMax = 1;
			dataSave.lastLevel = 1;
			dataSave.coin = 0;
			dataSave.bestScore = 0;
			dataSave.currentScore = 0;
			dataSave.board = new List<int> ();
			dataSave.boardColor = new List<int> ();
			dataSave.startIds = new List<int> ();
			dataSave.isMusicOn = true;
			dataSave.isSoundOn = true;
			dataSave.startBlockColor = new List<int> ();
			for (int i = 0; i < xCount * yCount; i++) {
				dataSave.board.Add (0);
				dataSave.boardColor.Add (0);
				if (i < 3) {
					dataSave.startIds.Add (-1);
					dataSave.startBlockColor.Add (-1);
				}
			}
			dataSave.nextHeartTime = System.DateTime.Now.Ticks;
			dataSave.heart = 5;
			dataSave.stagesList = new List<StageModule> ();
			dataSave.startBlockVisible = new int[]{ 1, 1, 1 };
			for (int i = 0; i < 100; i++) {
				StageModule stage = new StageModule ();
				dataSave.stagesList.Add (stage);				
			}
			SaveData ();
		}

		//dataSave.coin = 10000;
	}

	public static int GetRandomBlockId ()
	{
		LevelDataModule levelData = levelsDic [currentLevel];
		int rnd = Random.Range (0, listBlockData.Count);
		if (levelData.maxShapeSize > 0 || levelData.maxSquareSize > 0) {
			int shapeSize = listBlockData [rnd].shapeSize;
			int squareSize = listBlockData [rnd].squareSize;
			while ((levelData.maxShapeSize > 0 && shapeSize > levelData.maxShapeSize) || (levelData.maxSquareSize > 0 && squareSize > levelData.maxSquareSize)) {
				rnd = Random.Range (0, listBlockData.Count);
				shapeSize = listBlockData [rnd].shapeSize;
				squareSize = listBlockData [rnd].squareSize;
			}

		}
		return rnd;
	}

	public static void SaveData ()
	{
		string str = JsonUtility.ToJson (dataSave);
//		Debug.Log ("str save" + str);
		PlayerPrefs.SetString ("fuckyou", str);
		PlayerPrefs.Save ();
	}

	/// <summary>
	/// 获取总通关星数
	/// </summary>
	/// <returns>总星数</returns>
	public static int GetTotalStars()
	{
		int totalStars = 0;
		foreach (StageModule stage in dataSave.stagesList)
		{
			totalStars += stage.stars;
		}
		return totalStars;
	}
}

