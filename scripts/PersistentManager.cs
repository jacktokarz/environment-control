using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistentManager : MonoBehaviour
{
	public static PersistentManager Instance { get; private set; }


    public Text Humidity;
    public Text Wind;

    
    public int maxHumidity;
    public int minHumidity;

    public int maxWind;
    public int minWind;
	
	public float vineGrowDefaultSpeed;
    public float vineMinHeight;
    public float vineMaxHeight;
    public float vinePieceHeight;
    public float vinePieceWidth;
    public float vineWindAffect;
    public LayerMask whatBlocksVines;

    public float waterChangeSpeed;
    public float waterChangeDistance;
    
    public float lilyPadTravelSpeed;

	public float doorMoveSpeed;
	public float doorMoveDistance;


	public int windLevel;
	public int humidityLevel;
	public string lastDoorId;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
        updateText(Humidity, humidityLevel);
        updateText(Wind, windLevel);
	}


    public bool changeHumidity(int value)
    {
        if ((value<0)?(humidityLevel>minHumidity):(humidityLevel<maxHumidity))
        {
            humidityLevel += value;
            updateText(Humidity, humidityLevel);
            return true;
        }
        return false;
    }

    public bool changeWind(int value)
    {
        if ((value<0)?(windLevel > minWind):(windLevel < maxWind))
        {
            windLevel+=value;
            updateText(Wind, windLevel);
        	return true;
        }
        return false;
    }

    public void updateText(Text textObj, int value)
    {
        textObj.text = value.ToString();
    }

}
