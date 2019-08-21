using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
	public int lastCheckpoint;

	//[HideInInspector] 
	public List<string> TreasureList = new List<string>();
	public List<int> Checkpoints = new List<int>();


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

	public static void GoToScene(int sceneNumber) {
    	SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }

    public void updateText(Text textObj, int value)
    {
        textObj.transform.GetChild(0).GetComponent<Text>().text = value.ToString();
    }

    private GameData createGameData()
    {
    	GameData gd = new GameData();
    	gd.TreasureList = new List<string>(TreasureList);
    	gd.lastCheckpoint = lastCheckpoint;
    	gd.Checkpoints = new List<int>(Checkpoints);
    	return gd;
    }

    public bool Save()
    {
    	GameData data = createGameData();
    	Debug.Log("data to be saved: "+data);
    	Debug.Log("specifically the lastcp: "+data.lastCheckpoint);
	    BinaryFormatter bf = new BinaryFormatter();
	    FileStream file = File.Create (Application.persistentDataPath + "/save.gd");
	    bf.Serialize(file, data);
	    file.Close();
	    return true;
    }


}
