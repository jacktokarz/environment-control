using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Cinemachine;

public class PersistentManager : MonoBehaviour
{
	public static PersistentManager Instance { get; private set; }

        //objects?
    public GameObject playerPollen;
		//UI
    public Text Humidity, Wind, Temperature, CollectibleCount;
    public GameObject PauseHumidity, PauseWind, PauseTemperature;
    public GameObject GameOver;
    public GameObject Message;
    public CanvasGroup blackCover;
    public string difficulty;
    	//Camera
    public CinemachineVirtualCamera vcam;
    public List<float> zoomOptions = new List<float>() {6.5f, 8f, 5f};
    [HideInInspector] 
    public float currentZoom;
    public bool zooming = false;
    public float openingCutsceneLength = 13f;

    public KeyCode PauseKey = KeyCode.P;
    public KeyCode ZoomKey = KeyCode.Z;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode GrabKey = KeyCode.RightShift;
    public KeyCode HumidityKey = KeyCode.Alpha1;
    public KeyCode WindKey = KeyCode.Alpha2;
    public KeyCode TemperatureKey = KeyCode.Alpha3;
    public float fadeSpeed = .5f;

    
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
    [HideInInspector] 
    public List<int> motherPlantSongScenes = new List<int>() {0, 1, 2};
    public List<int> elevatorSongScenes = new List<int>() {3, 4, 8};
    public List<int> windSongScenes = new List<int>() { 5, 6, 7 };
    public AudioSource musicPlayer;
    public AudioClip motherPlantSong;
    public AudioClip elevatorSong;
    public AudioClip windSong;
    public float standardMusicVolume;
    public float quietMusicVolume;

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
        GetCameraAndZoom();
        SetKeys();
	}

	private void Start()
	{
		CheckTextVis();
        SelectMusic(SceneManager.GetActiveScene().buildIndex);
	}


    public void CheckTextVis()
    {
        updateText(CollectibleCount, Collectibles.Count);

    	bool seeHum = TreasureList.Contains("humidity") ? true : false;
		Humidity.gameObject.SetActive(seeHum);
        updateText(Humidity, humidityLevel);
        PauseHumidity.SetActive(seeHum);

		bool seeWind = TreasureList.Contains("windVision") ? true : false;
		Wind.gameObject.SetActive(seeWind);
        updateText(Wind, windLevel);
        PauseWind.SetActive(seeWind);

        bool seeTemp = TreasureList.Contains("temperatureVision") ? true : false;
        Temperature.gameObject.SetActive(seeTemp);
        updateText(Temperature, tempLevel);
        PauseTemperature.SetActive(seeTemp);

        //bool seeToxic = TreasureList.Contains("toxicity") ? true : false;
    }

    public IEnumerator WaitToActivate(GameObject go, bool setting, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        go.SetActive(setting);
    }

    public IEnumerator WaitToPlay(AudioSource source, AudioClip clip, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        source.PlayOneShot(clip);
    }    

	public IEnumerator GoToScene(int sceneNumber)
    {
        Debug.Log("going to scene "+sceneNumber);
        if (!getSongList(SceneManager.GetActiveScene().buildIndex).Contains(sceneNumber)) {
            SelectMusic(-1);
        }
        UIFader.Instance.fadeIn(blackCover);
        yield return new WaitForSeconds(0.75f);
        SelectMusic(sceneNumber);
        yield return new WaitForSeconds(0.75f);
    	SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
        if (GameOver.activeSelf)
        {
            GameOver.SetActive(false);
        }
        UIFader.Instance.fadeOut(blackCover);
    }

    // re-using fadeSpeed here, might not be good?
    public IEnumerator ChangeZoom(float end, float lerpTime)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        float start = vcam.m_Lens.OrthographicSize;

        zooming = true;
        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            vcam.m_Lens.OrthographicSize = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
        zooming = false;
        currentZoom = vcam.m_Lens.OrthographicSize;
    }

    public void updateText(Text textObj, int value)
    {
        textObj.transform.GetChild(0).GetComponent<Text>().text = value.ToString();
    }

    public IEnumerator MakeMessage(Queue<string> nextMessages)
    {
        immobile = true;
        Message.GetComponent<Text>().text = nextMessages.Dequeue();
        MessageScript ms = Message.GetComponent(typeof (MessageScript)) as MessageScript;
        UIFader.Instance.fadeIn(ms.cg);

        yield return new WaitForSeconds(fadeSpeed);
        ms.escapeKey = nextMessages.Dequeue();

        ms.nextMessages = nextMessages;
    }

    private GameData CreateGameData()
    {
    	GameData gd = new GameData();
    	gd.TreasureList = new List<string>(TreasureList);
    	gd.lastCheckpoint = lastCheckpoint;
    	gd.Checkpoints = new List<int>(Checkpoints);
        gd.Collectibles = new List<int>(Collectibles);
    	return gd;
    }

    public bool Save()
    {
    	GameData data = CreateGameData();
	    BinaryFormatter bf = new BinaryFormatter();
	    FileStream file = File.Create (Application.persistentDataPath + "/save.gd");
	    bf.Serialize(file, data);
	    file.Close();
	    return true;
    }

    // public void ResetKeys()
    // {
    //     PlayerPrefs.SetString("JumpButton", );
    //     PlayerPrefs.SetString("GrabButton", "G");
    //     PlayerPrefs.SetString("HumidityButton", "Alpha1");
    //     PlayerPrefs.SetString("WindButton", "Alpha2");
    // }

    public void SetKeys()
    {
        string[] controllers = Input.GetJoystickNames();
        bool xBox = false;
        for (int x = 0; x < controllers.Length; x++)
        {
            if (controllers[x].Contains("Xbox One"))
            {
                xBox = true;
            }
        }

        PauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PauseButton", xBox ? "JoystickButton7" : "P"));
        ZoomKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ZoomButton", xBox ? "JoystickButton8" : "Z"));
        JumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpButton", xBox ? "JoystickButton5" : "Space"));
        GrabKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("GrabButton", xBox ? "JoystickButton4" : "RightShift"));
        HumidityKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HumidityButton", xBox ? "JoystickButton1" : "Alpha1"));
        WindKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("WindButton", xBox ? "JoystickButton3" : "Alpha2"));
        TemperatureKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TemperatureButton", xBox ? "JoystickButton2" : "Alpha3"));
    }

    public List<int> getSongList(int sn)
    {
        if (motherPlantSongScenes.Contains(sn))
        {
            return motherPlantSongScenes;
        }
        else if (elevatorSongScenes.Contains(sn))
        {
            return elevatorSongScenes;
        }
        else if (windSongScenes.Contains(sn))
        {
            return windSongScenes;
        }

        return new List<int>();
    }

    public void SelectMusic(int sn)
    {
        StartCoroutine(LerpVolume(1, standardMusicVolume));
        List<int> songList = getSongList(sn);
        if (motherPlantSongScenes == songList)
        {
            if (musicPlayer.clip == motherPlantSong) { return; }
            musicPlayer.clip = motherPlantSong;
        }
        else if (elevatorSongScenes == songList)
        {
            if (musicPlayer.clip == elevatorSong) { return; }
            musicPlayer.clip = elevatorSong;
        }
        else if (windSongScenes == songList)
        {
            if (musicPlayer.clip == windSong) { return; }
            musicPlayer.clip = windSong;
        }
        else 
        {
            musicPlayer.clip = null;
            musicPlayer.Stop(); 
            return;
        }
        musicPlayer.Play();
    }

    public IEnumerator LerpVolume(float lerpTime, float end)
    {
        float _timeStartedLerping = Time.time;
        float start = PersistentManager.Instance.musicPlayer.volume;
        float timeSinceStarted = 0.0f;
        float percentageComplete = 0.0f;
        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;
            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            musicPlayer.volume = currentValue;

            if (percentageComplete >= 1) break;
            yield return new WaitForFixedUpdate();
        }
    }

    public void GetCameraAndZoom()
    {
        if (GameObject.FindGameObjectsWithTag("primaryVirtualCamera").Length > 0) {
            vcam = GameObject.FindGameObjectsWithTag("primaryVirtualCamera")[0].GetComponent<CinemachineVirtualCamera>();
            currentZoom = vcam.m_Lens.OrthographicSize;
            if (currentZoom!=zoomOptions[0])
            {
                StartCoroutine(ChangeZoom(zoomOptions[0], fadeSpeed));
            }
        }
        else {
            Debug.Log("no primary camera");
        }
    }

    public void SeePollen()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        playerPollen.transform.position = new Vector3(playerPos.x, (playerPos.y + 0.6f), 0);
        playerPollen.SetActive(true);
        StartCoroutine(WaitToActivate(playerPollen, false, 6f));
    }

    // public Transform FindChildObjectByTag(Transform obj, string _tag)
    // {
    //     for (int i = 0; i < obj.childCount; i++)
    //     {
    //         Transform child = obj.GetChild(i);
    //         Debug.Log("tag is "+child.tag);
    //         if (child.tag == _tag)
    //         {
    //             Debug.Log("found it");
    //             return child;
    //         }
    //     }
    //     Debug.Log("not found");
    //     return null;
    // }

}
