using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProgress
{
	public bool hasHum;
    public bool hasHumVision;
    public bool hasWind;
    public bool hasWindVision;
    public int dnaStrands;

    public PlayerProgress()
    {
		hasHum = false;
        hasHumVision = false;
		hasWind = false;
        hasWindVision = false;
		dnaStrands = 0;    	
    }

    public PlayerProgress(PlayerProgress pp)
    {
    	hasHum = pp.hasHum;
        hasHumVision = pp.hasHumVision;
    	hasWind = pp.hasWind;
        hasWindVision = pp.hasWindVision;
    	dnaStrands = pp.dnaStrands;
    }

    public PlayerProgress(bool hum, bool humVision, bool wind, bool windVision, int strands)
    {
    	hasHum = hum;
        hasHumVision = humVision;
    	hasWind = wind;
        hasWindVision = windVision;
    	dnaStrands = strands;
    }
}
