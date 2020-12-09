using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour
{
	private Transform PersistentUI;
	private GameData loadGame;
    public GameObject firstScreen;
    public CanvasGroup firstCanvas;
    public GameObject difficultyScreen;
    public CanvasGroup difficultyCanvas;
    public GameObject setKeysScreen;
    public CanvasGroup setKeysCanvas;
    public GameObject background;
    public Vector2 bgResetSpot;
    public Button defaultDifficulty, newButton, pauseButton;
    public Text pauseText, zoomText, jumpText, grabText;
    
    public List<Sprite> bgList = new List<Sprite>();
    public List<Sprite> availableBgs = new List<Sprite>();

    void Start()
    {
        FillAvailableBgs();
        loadGame= Load();
        if (loadGame == null)
        {
        	GameObject loadButton = GameObject.FindWithTag("load");
        	loadButton.SetActive(false);
        }

        PersistentUI = PersistentManager.Instance.transform.GetChild(0);
        PersistentUI.gameObject.SetActive(false);
    }

    void Update()
    {
        Collider2D platCollider = Physics2D.OverlapCircle(bgResetSpot, 0.1f);
        if (platCollider==null)
        {
            GameObject newBg = Instantiate(background, transform.position, Quaternion.identity);
            SpriteRenderer sr = newBg.GetComponent<SpriteRenderer>();
            int rando = UnityEngine.Random.Range(0, availableBgs.Count);
            sr.sprite = availableBgs[rando];
            availableBgs.Remove(availableBgs[rando]);
            if (availableBgs.Count==0)
            {
                FillAvailableBgs();
            }
            Vector2 srSize = sr.bounds.size;
            newBg.transform.position = new Vector3(newBg.transform.position.x,
                newBg.transform.position.y - (srSize.y/2) + bgResetSpot.y + 0.25f,
                newBg.transform.position.z);
            BoxCollider2D bc = newBg.GetComponent<BoxCollider2D>();
            bc.size = srSize;
        }
    }

    void FillAvailableBgs()
    {
        if (PersistentManager.Instance.Checkpoints.Count==0)
        {
            availableBgs.Add(bgList[0]);
        }
        else
        {
            for(var i = 0; i<PersistentManager.Instance.Checkpoints.Count; i++)
            {
                availableBgs.Add(bgList[i]);
            }
        }
    }

    public GameData Load() 
    {
        if(File.Exists(Application.persistentDataPath + "/save.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.gd", FileMode.Open);
            GameData gd = (GameData)bf.Deserialize(file);
            file.Close();
            return gd;
        }
        else
        {
        	return null;
        }
    }

    public void loadClicked()
    {
        PersistentManager.Instance.TreasureList = new List<string>(loadGame.TreasureList);
        PersistentManager.Instance.CheckTextVis();
        PersistentManager.Instance.lastCheckpoint = loadGame.lastCheckpoint;
        PersistentManager.Instance.Checkpoints = new List<int>(loadGame.Checkpoints);
        PersistentManager.Instance.lastDoorId = "Respawn";
        GameObject propertyValues = PersistentUI.Find("PropertyValues").gameObject;
        propertyValues.SetActive(false);
        PersistentUI.gameObject.SetActive(true);
        StartCoroutine(PersistentManager.Instance.GoToScene(PersistentManager.Instance.lastCheckpoint));
    	StartCoroutine(PersistentManager.Instance.WaitToActivate(propertyValues, true, PersistentManager.Instance.fadeSpeed * 1.5f));
    }

    public void newClicked()
    {
        PlayerPrefs.DeleteAll();
        PersistentManager.Instance.SetKeys();
        pauseText.text = PersistentManager.Instance.PauseKey.ToString();
        zoomText.text = PersistentManager.Instance.ZoomKey.ToString();
        grabText.text = PersistentManager.Instance.GrabKey.ToString();
        jumpText.text = PersistentManager.Instance.JumpKey.ToString();

        UIFader.Instance.fadeIn(setKeysCanvas);
        firstScreen.SetActive(false);
        pauseButton.Select();
        pauseButton.OnSelect(null);
    }

    public void saveKeysClicked()
    {
        UIFader.Instance.fadeIn(difficultyCanvas);
        setKeysScreen.SetActive(false);
        defaultDifficulty.Select();
        defaultDifficulty.OnSelect(null);
    }

    public void backSelected()
    {
        UIFader.Instance.fadeIn(firstCanvas);
        difficultyScreen.SetActive(false);
        newButton.Select();
        newButton.OnSelect(null);
    }

    public void difficultyClicked(string difficulty)
    {
    	PersistentUI.gameObject.SetActive(true);
        PersistentManager.Instance.difficulty=difficulty;
        PersistentManager.Instance.lastDoorId="NewGame";
    	StartCoroutine(PersistentManager.Instance.GoToScene(1));
    }
}
