using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class LoadingControl : MonoBehaviour
{

	// Use this for initialization
	public Block moveBlock;
	public Image loadingBar;
	public TextMeshProUGUI text;
	void Start ()
	{
		
		StartCoroutine (LoadingCoroutine ());
//		Admob.instance.ShowBanner ();
	}

	IEnumerator LoadingCoroutine ()
	{
		
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime;
			loadingBar.fillAmount = t;
			text.text = "游戏加载中…" + (t*100).ToString("F0")+"%";

            yield return null;
		}
		GameManager.LoadDataSaved ();
		LoadData ();	
		LoadTexts ();
		TimerControl.instance.OnStart ();
		//LoadLevelData ();
		AudioManager.instance.AS_SOUND.volume = GameManager.dataSave.isSoundOn ? 1 : 0;
		AudioManager.instance.AS_MUSIC.volume = GameManager.dataSave.isSoundOn ? 1 : 0;
		SceneManager.LoadScene ("scn_home");
	}

	void LoadData ()
	{
		TextAsset ta = Resources.Load ("BlockData") as TextAsset;
		//Debug.Log (ta.text);
		GameManager.listBlockData = new List<BlockModule> ();
		BlockData data = JsonUtility.FromJson<BlockData> (ta.text);
		for (int i = 0; i < data.blockData.Count; i++) {
			int[,] blockGrid = new int[5, 5];
			string mnDataStr = data.blockData [i];
			string[] mnDataSplit = mnDataStr.Split (new char[]{ '_' }, 3);
			string shapeSizeStr = mnDataSplit [1];
			string squareSizeStr = mnDataSplit [2];
			string[] mnDataRow = mnDataSplit [0].Split (new char[]{ ',' }, 5);
			string str = "";
			for (int j = 0; j < mnDataRow.Length; j++) {
				for (int k = 0; k < mnDataRow [j].Length; k++) {
					blockGrid [j, k] = int.Parse (mnDataRow [j] [k].ToString ());
					str = str + blockGrid [j, k];
				}
				str = str + "/n";
			}
			//Debug.Log ("str " + i + " : " + str);
			BlockModule block = new BlockModule ();
			block.blockGrid = blockGrid;
			block.shapeSize = int.Parse (shapeSizeStr);
			block.squareSize = int.Parse (squareSizeStr);
			GameManager.listBlockData.Add (block);
		}

	}

	void LoadTexts ()
	{
		TextAsset ta = Resources.Load ("texts") as TextAsset;
		GameText gameText = JsonUtility.FromJson<GameText> (ta.text);
		GameManager.gameTextsDic = new Dictionary<string, GameTextElement> ();
		var fields = typeof(GameText).GetFields ();
		var names = Array.ConvertAll (fields, field => field.Name);
		for (int i = 0; i < names.Length; i++) {
			string n = names [i];
			GameManager.gameTextsDic.Add (n, gameText.GetType ().GetField (names [i]).GetValue (gameText) as GameTextElement);
			Debug.Log (n);
		}

	}




	// load old levels
	/*
	void LoadLevelData ()
	{
		LevelData levelsData = new LevelData ();
		levelsData.data = new List<LevelDataModule> ();
		TextAsset ta = Resources.Load ("levels") as TextAsset;
		JSONObject levelDataObjs = new JSONObject (ta.text);
		for (int k = 0; k < levelDataObjs.Count; k++) {
			JSONObject levelDataObj = levelDataObjs [k];

			LevelDataModule levelData = new LevelDataModule ();
			levelsData.data.Add (levelData);
			levelData.elements = new List<LeveDataElementModule> ();
			JSONObject elementsObj = levelDataObj.GetField ("elements");
			for (int i = 0; i < elementsObj.Count; i++) {
				LeveDataElementModule element = new LeveDataElementModule ();
				levelData.elements.Add (element);
				JSONObject elementObj = elementsObj [i];
				element.type = GameManager.ConvertElementTypeStringToElementType (elementObj.GetField ("type").str);
				element.x = int.Parse (elementObj.GetField ("x").ToString ());
				element.y = int.Parse (elementObj.GetField ("y").ToString ());
				element.color = (BlockColor)(elementObj.GetField ("color") != null ? int.Parse (elementObj.GetField ("color").str) : 0);
				element.fruit = elementObj.GetField ("fruit") != null ? bool.Parse (elementObj.GetField ("fruit").ToString ()) : false;
				element.fruitColor = (BlockColor)(elementObj.GetField ("fruitColor") != null ? int.Parse (elementObj.GetField ("fruitColor").str) : 0);
				element.ice = elementObj.GetField ("ice") != null ? bool.Parse (elementObj.GetField ("ice").ToString ()) : false;
				element.iceType = elementObj.GetField ("iceType") != null ? int.Parse (elementObj.GetField ("iceType").str) : -1;
				element.amount = elementObj.GetField ("amount") != null ? int.Parse (elementObj.GetField ("amount").ToString ()) : -1;
			}
			/// load target 
			levelData.targets = new List<TargetModule> ();
			JSONObject targetsObj = levelDataObj.GetField ("targets");
			for (int i = 0; i < targetsObj.Count; i++) {
				JSONObject targetObj = targetsObj [i];
				TargetModule target = new TargetModule ();
				levelData.targets.Add (target);
				target.type = GameManager.ConvertTargetTypeStringToTargetType (targetObj.GetField ("type").str);
				target.amount = int.Parse (targetObj.GetField ("amount").ToString ());
				target.color = (BlockColor)(targetObj.GetField ("color") != null ? int.Parse (targetObj.GetField ("color").ToString ()) : 0);
			}

			// load moves 
			levelData.moves = int.Parse (levelDataObj.GetField ("moves").ToString ());
			levelData.level = k + 1;
			levelData.fruitMode = levelDataObj.GetField ("fruitMode") != null ? int.Parse (levelDataObj.GetField ("fruitMode").str) : -1;
			levelData.fruitChance = levelDataObj.GetField ("fruitChance") != null ? int.Parse (levelDataObj.GetField ("fruitChance").ToString ()) : -1;
			levelData.iceChance = levelDataObj.GetField ("iceChance") != null ? int.Parse (levelDataObj.GetField ("iceChance").ToString ()) : -1;
			levelData.maxSquareSize = levelDataObj.GetField ("maxSquareSize") != null ? int.Parse (levelDataObj.GetField ("maxSquareSize").ToString ()) : -1;
			levelData.maxShapeSize = levelDataObj.GetField ("maxShapeSize") != null ? int.Parse (levelDataObj.GetField ("maxShapeSize").ToString ()) : -1;
		
			string json = JsonUtility.ToJson (levelData);
			string path = "Assets/Resources/LevelMap/";
			using (FileStream fs = new FileStream (path + "Level_" + levelData.level + ".json", FileMode.Create)) {
				using (StreamWriter writer = new StreamWriter (fs)) {
					writer.Write (json);
				}
			}

		
		}



	}
    */
}

[System.Serializable]
public class BlockData
{
	public List<string> blockData;

}

[System.Serializable]
public class BlockModule
{
	public int[,] blockGrid;
	public int shapeSize;
	public int squareSize;
}