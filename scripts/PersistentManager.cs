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
        Debug.Log("changing wind by "+value);
        if ((value<0)?(windLevel > minWind):(windLevel < maxWind))
        {
            windLevel+=value;
<<<<<<< Updated upstream
=======
            changeWindAnimation();
>>>>>>> Stashed changes
            updateText(Wind, windLevel);
        	return true;
        }
        updateText(Wind, windLevel);
        return false;
    }
<<<<<<< Updated upstream
=======
    public void changeWindAnimation()
    {
    	GameObject[] winds = GameObject.FindGameObjectsWithTag("wind");
        Debug.Log("setting wind anim level to "+windLevel);
    	foreach (GameObject win in winds)
    	{
    		Animator winAm = win.GetComponent<Animator>();
    		winAm.SetFloat("windSpeed", windLevel);
    	}
    }

    public void checkTextVis()
    {
    	if (TreasureList.Contains("humidity"))
		{
			Humidity.gameObject.SetActive(true);
	        updateText(Humidity, humidityLevel);
		}

		if (TreasureList.Contains("wind"))
		{
			Wind.gameObject.SetActive(true);
	        updateText(Wind, windLevel);
		}
    }

	public static void GoToScene(int sceneNumber) {
    	SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }
>>>>>>> Stashed changes

    public void updateText(Text textObj, int value)
    {
        textObj.text = value.ToString();
    }

}
