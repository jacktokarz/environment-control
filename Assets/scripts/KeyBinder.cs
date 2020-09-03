using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinder : MonoBehaviour
{
	public Pause pauseScript;
	public Text pause, jump, grab, humidity, wind, temperature;

	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
	private GameObject currentKey;
	private Color32 normal = new Color32(255, 255, 255, 255);
	private Color32 selected = new Color32(100, 255, 255, 255);

    void Start()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;
        
        string[] controllers = Input.GetJoystickNames();
        bool xBox = false;
        for (int x = 0; x < controllers.Length; x++)
        {
            Debug.Log("controller "+controllers[x]);
            if (controllers[x].Contains("Xbox One"))
            {
                xBox = true;
            }
        }
        Debug.Log("xbox? "+xBox);

		keys.Add("PauseButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PauseButton", xBox ? "JoystickButton7" : "P")));
        pause.text = keys["PauseButton"].ToString();
        keys.Add("GrabButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("GrabButton", xBox ? "Joystick1Button5" : "G")));
		grab.text = keys["GrabButton"].ToString();
		keys.Add("JumpButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpButton", xBox ? "Joystick1Button4" : "Space")));
		jump.text = keys["JumpButton"].ToString();
		keys.Add("HumidityButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HumidityButton", xBox ? "JoystickButton19" : "Alpha1")));
        humidity.text = keys["HumidityButton"].ToString();
		if(tl.Contains("wind")) {
			keys.Add("WindButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("WindButton", xBox ? "JoystickButton17" : "Alpha2")));
			wind.text = keys["WindButton"].ToString();			
		}
        if(tl.Contains("temperature")) {
            keys.Add("TemperatureButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TemperatureButton", xBox ? "JoystickButton18" : "Alpha3")));
            temperature.text = keys["TemperatureButton"].ToString();          
        }

    }


    void OnGUI()
    {
    	if (currentKey != null)
    	{
            // KeyCode pressed = (KeyCode)System.Enum.Parse(typeof(KeyCode), "None");
            // Debug.Log("pressed "+pressed.ToString());
            // foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
            //      if(Input.GetKey(vKey)){
            //         Debug.Log("GUI "+vKey);
            //         pressed = vKey;
            //         break;
            //     }
            // }
    		Event e = Event.current;
            Debug.Log("player pressed "+e.keyCode.ToString());
    		if (e.isKey && e.keyCode.ToString()!="Return")
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
    }
}
