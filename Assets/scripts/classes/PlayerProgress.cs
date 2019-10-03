using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProgress
{
	public bool hasHum;
    public bool hasWind;
    public int dnaStrands;

    public PlayerProgress()
    {
		hasHum = true;
		hasWind = false;
		dnaStrands = 0;    	
    }

    public PlayerProgress(PlayerProgress pp)
    {
    	hasHum = pp.hasHum;
    	hasWind = pp.hasWind;
    	dnaStrands = pp.dnaStrands;
    }

    public PlayerProgress(bool hum, bool wind, int strands)
    {
    	hasHum = hum;
    	hasWind = wind;
    	dnaStrands = strands;
    }
}
