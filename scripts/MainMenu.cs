using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour
{
	private Transform PersistentUI;
	private GameData loadGame;

    void Start()
    {
        loadGame= Load();
        if (loadGame == null)
        {
        	GameObject loadButton = GameObject.FindWithTag("load");
        	loadButton.SetActive(false);
        }

        PersistentUI = PersistentManager.Instance.transform.GetChild(0);
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
    	putLoadInManager();
    	PersistentManager.GoToScene(PersistentManager.Instance.lastCheckpoint);
    	PersistentUI.gameObject.SetActive(true);
    }

    public void newClicked()
    {
    	PersistentUI.gameObject.SetActive(true);
    	PersistentManager.GoToScene(1);
    }

    void putLoadInManager()
    {
        PersistentManager.Instance.playerProgress = new Dictionary<string, bool>(loadGame.playerProgress);
        PersistentManager.Instance.lastCheckpoint = loadGame.lastCheckpoint;
        PersistentManager.Instance.Checkpoints = new List<int>(loadGame.Checkpoints);
        PersistentManager.Instance.lastDoorId = "Respawn";
    }
}
