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

		//UI
    public Text Humidity;
    public Text Wind;
    public Text Temperature;
    public GameObject GameOver;
    public GameObject Message;
    	//humidity
    public int maxHumidity;
    public int minHumidity;
    	//wind
    public int maxWind;
    public int minWind;
        // temp
    public int maxTemp;
    public int minTemp;
    	//get wind room
    public int getWindRoomChangeDelay;
		//vines
	public float vineGrowDefaultSpeed;
    public float vineMinHeight;
    public float vineMaxHeight;
    public float vinePieceHeight;
    public float vinePieceWidth;
    public float vineWindAffect;
    public LayerMask whatBlocksVines;
    	//water
    public float waterChangeSpeed;
    public float waterChangeDistance;
    	//lily pad
    public float lilyPadTravelSpeed;
    	//enemies & projectiles
            //dumb
    public float dumbEnemyFireRate;
    public float dumbEnemyVision;
    public float dumbProjectileSpeed;
            //ice
    public float iceEnemyFireRate;
    public float iceEnemyVision;
    public float iceProjectileSpeed;
    public LayerMask blocksProjectiles;
    	//door
	public float doorMoveSpeed;
	public float doorMoveDistance;

		//changes in gameplay
	public int windLevel;
	public int humidityLevel;
    public int tempLevel;
    public int toxicLevel;
	public string lastDoorId;
	public int lastCheckpoint;

	//[HideInInspector] 
	public List<string> TreasureList = new List<string>();
	public List<int> Checkpoints = new List<int>();
    public List<int> Collectibles = new List<int>();


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
		checkTextVis();
	}


    public void checkTextVis()
    {
    	bool seeHum = TreasureList.Contains("humidity") ? true : false;
		Humidity.gameObject.SetActive(seeHum);
        updateText(Humidity, humidityLevel);

		bool seeWind = TreasureList.Contains("wind") ? true : false;
		Wind.gameObject.SetActive(seeWind);
        updateText(Wind, windLevel);

        bool seeTemp = TreasureList.Contains("temperature") ? true : false;
        Temperature.gameObject.SetActive(seeTemp);
        updateText(Temperature, tempLevel);

        //bool seeToxic = TreasureList.Contains("toxicity") ? true : false;
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
