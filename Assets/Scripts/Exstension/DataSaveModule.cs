using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSaveModule
{
	public string language = "en";
	public int currentScore;
	public int bestScore;
	public int coin;
	public int heart;
	public List<StageModule> stagesList;
	public List<int> board;
	public List<int> boardColor;
	public List<int> startIds;
	public List<int> startBlockColor;
	public int[] startBlockVisible;
	public bool isSoundOn, isMusicOn;
	public int comboMatchCount;
	public bool isLoginedGoogle = false;
	public bool isRate = false;
	public int lastLevel, levelMax;
	public long nextHeartTime;
}

[System.Serializable]
public class StageModule
{
	public int level;
	public int score;
	public int bestScore;
	public int stars;
}