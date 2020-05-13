using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour
{
	private Transform PersistentUI;
	private GameData loadGame;
    private UIFader uiFader;

    public CanvasGroup blackBG;

    void Start()
    {
        loadGame= Load();
        if (loadGame == null)
        {
        	GameObject loadButton = GameObject.FindWithTag("load");
        	loadButton.SetActive(false);
        }

        PersistentUI = PersistentManager.Instance.transform.GetChild(0);
        PersistentUI.gameObject.SetActive(false);

        uiFader = this.GetComponent(typeof (UIFader)) as UIFader;
    }

    public GameData Load() 
    {
    	Debug.Log("loading...");
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
    	Debug.Log("clicked load game");
        PersistentManager.Instance.TreasureList = new List<string>(loadGame.TreasureList);
        PersistentManager.Instance.checkTextVis();
        PersistentManager.Instance.lastCheckpoint = loadGame.lastCheckpoint;
        PersistentManager.Instance.Checkpoints = new List<int>(loadGame.Checkpoints);
        PersistentManager.Instance.lastDoorId = "Respawn";
        GameObject propertyValues = PersistentUI.Find("PropertyValues").gameObject;
        propertyValues.SetActive(false);
        PersistentUI.gameObject.SetActive(true);
        StartCoroutine(PersistentManager.Instance.GoToScene(PersistentManager.Instance.lastCheckpoint));
    	StartCoroutine(PersistentManager.Instance.WaitToActivate(propertyValues, true, PersistentManager.Instance.fadeSpeed * 1.5f));
    }

    public void newClicked(string difficulty)
    {
        Debug.Log("clicked new with "+difficulty);
    	PersistentUI.gameObject.SetActive(true);
        PersistentManager.Instance.difficulty=difficulty;
        PersistentManager.Instance.lastDoorId="NewGame";
    	StartCoroutine(PersistentManager.Instance.GoToScene(1));
    }
}
