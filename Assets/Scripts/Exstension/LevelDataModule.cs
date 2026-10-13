using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelData
{
	public List<LevelDataModule> data;
}

[System.Serializable]
public class LevelDataModule
{
	public List<LeveDataElementModule> elements;
	public List<TargetModule> targets;
	public int moves;
	public int maxShapeSize, maxSquareSize;
	public int iceChance, fruitChance, fruitMode;
	public int level;
}

[System.Serializable]
public class LeveDataElementModule
{
	public ElementType type;
	public int x, y;
	public BlockColor color;
	public bool fruit;
	public BlockColor fruitColor;
	public bool ice;
	public int iceType;
	public int amount;
}

[System.Serializable]
public class TargetModule
{
	public TargetType type;
	public int amount;
	public BlockColor color;
}

[System.Serializable]
public enum ElementType
{
	ET_BLANK,
	ET_FILL,
	ET_DIRT,
	ET_NOONE
}

[System.Serializable]
public enum TargetType
{
	TGT_COMBO,
	TGT_FRUIT,
	TGT_DIRT,
	TGT_DOUBLE,
	TGT_SCORE,
	TGT_ICE,
	TGT_COLOR
}