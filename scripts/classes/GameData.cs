using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public Dictionary<string, bool> playerProgress = new Dictionary<string, bool>();
	public int lastCheckpoint;
	public List<int> Checkpoints = new List<int>();
}
