using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinder : MonoBehaviour
{
	public Pause pauseScript;
	public Text left, right, jump, grab, humidity, wind;

	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
	private GameObject currentKey;
	private Color32 normal = new Color32(255, 255, 255, 255);
	private Color32 selected = new Color32(100, 255, 255, 255);

    void Start()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;

		// keys.Add("LeftButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton", "LeftArrow")));
		// // left.text = keys["LeftButton"].ToString();
		// keys.Add("RightButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton", "RightArrow")));
		// // right.text = keys["RightButton"].ToString();
		keys.Add("GrabButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("GrabButton", "G")));
		grab.text = keys["GrabButton"].ToString();
		keys.Add("JumpButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpButton", "Space")));
		jump.text = keys["JumpButton"].ToString();
		if(tl.Contains("humidity")) {
			keys.Add("HumidityButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HumidityButton", "1")));
			humidity.text = keys["HumidityButton"].ToString();
		}
		if(tl.Contains("wind")) {
			keys.Add("WindButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("WindButton", "2")));
			wind.text = keys["WindButton"].ToString();			
		}
    }


    void OnGUI()
    {
    	if (currentKey != null)
    	{
    		Event e = Event.current;
    		if (e.isKey)
    		{
    			keys[currentKey.name] = e.keyCode;
    			currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
    			currentKey.GetComponent<Image>().color = normal;
    			currentKey = null;
    		}
    	}
    }

    public void ChangeKey(GameObject clicked)
    {
    	Debug.Log("clicked on "+clicked);
     	if (currentKey != null)
    	{
    		currentKey.GetComponent<Image>().color = normal;
		}
		currentKey = clicked;
		currentKey.GetComponent<Image>().color = selected;
}

    public void SaveKeys()
    {
    	foreach (var key in keys)
    	{
    		PlayerPrefs.SetString(key.Key, key.Value.ToString());
    	}
    	PlayerPrefs.Save();
    	PersistentManager.Instance.SetKeys();
    	pauseScript.UnPause();
    }
}
