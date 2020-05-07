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
    public Text Humidity, Wind, Temperature;
    public GameObject PauseHumidity, PauseWind;
    public GameObject GameOver;
    public GameObject Message;
    public CanvasGroup blackCover;
    public string difficulty;
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
        //balloon
    public float balloonTravelSpeed;

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

        // internal stuff
    public bool immobile = false;

        // Audio
    private GameObject AudioChild;
    private AudioSource musicPlayer;
    public AudioClip motherPlantSong;
    public AudioClip elevatorSong;
    public AudioClip waterSong;

	public List<string> TreasureList = new List<string>();
	public List<int> Checkpoints = new List<int>();
    public List<int> Collectibles = new List<int>();

	[HideInInspector] 
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode GrabKey = KeyCode.G;
    public KeyCode HumidityKey = KeyCode.Alpha1;


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
        setKeys();
		checkTextVis();
        AudioChild = this.transform.Find("Audio").gameObject;
        musicPlayer = AudioChild.GetComponent<AudioSource>();
        SelectMusic(SceneManager.GetActiveScene().buildIndex);
	}


    public void checkTextVis()
    {
    	bool seeHum = TreasureList.Contains("humidity") ? true : false;
		Humidity.gameObject.SetActive(seeHum);
        updateText(Humidity, humidityLevel);
        PauseHumidity.SetActive(seeHum);

		bool seeWind = TreasureList.Contains("wind") ? true : false;
		Wind.gameObject.SetActive(seeWind);
        updateText(Wind, windLevel);
        PauseWind.SetActive(seeWind);

        bool seeTemp = TreasureList.Contains("temperature") ? true : false;
        Temperature.gameObject.SetActive(seeTemp);
        updateText(Temperature, tempLevel);

        //bool seeToxic = TreasureList.Contains("toxicity") ? true : false;
    }

	public IEnumerator GoToScene(int sceneNumber)
    {
        UIFader uiFader = this.GetComponent(typeof (UIFader)) as UIFader;
        SelectMusic(-1);
        uiFader.fadeIn(blackCover);
        yield return new WaitForSeconds(1);
        SelectMusic(sceneNumber);
        yield return new WaitForSeconds(1);
    	SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
        uiFader.fadeOut(blackCover);
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

    public void setKeys()
    {
        if (PlayerPrefs.GetString("JumpButton").Length > 0)
        {
            JumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpButton", "Space"));
        }
        if (PlayerPrefs.GetString("GrabButton").Length > 0)
        {
            GrabKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("GrabButton", "G"));
        }
        if (PlayerPrefs.GetString("HumidityButton").Length > 0)
        {
            HumidityKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HumidityButton", "Alpha1"));
        }
    }

    public void SelectMusic(int sn)
    {
        Debug.Log("musical scene is "+sn);
        List<int> motherPlantScenes = new List<int>() {0, 1, 2};
        List<int> elevatorScenes = new List<int>() {3, 4, 8};
        List<int> waterScenes = new List<int>() { 5, 6, 7 };
        if (motherPlantScenes.Contains(sn))
        {
            if (musicPlayer.clip == motherPlantSong) { return; }
            musicPlayer.clip = motherPlantSong;
        }
        else if (elevatorScenes.Contains(sn))
        {
            if (musicPlayer.clip == elevatorSong) { return; }
            musicPlayer.clip = elevatorSong;
        }
        else if (waterScenes.Contains(sn))
        {
            if (musicPlayer.clip == waterSong) { return; }
            musicPlayer.clip = waterSong;
        }
        else 
        {
            musicPlayer.clip = null;
            musicPlayer.Stop(); 
            return;
        }
        musicPlayer.Play();
    }


}
