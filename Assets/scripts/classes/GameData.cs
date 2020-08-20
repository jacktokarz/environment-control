using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public List<string> TreasureList = new List<string>();
	public int lastCheckpoint;
	public List<int> Checkpoints = new List<int>();
	public List<int> Collectibles = new List<int>();
}
