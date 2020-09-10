using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinder : MonoBehaviour
{
	public Pause pauseScript;
	public Text pause, zoom, jump, grab, humidity, wind, temperature;

	private GameObject currentKey;
	private Color32 normal = new Color32(255, 255, 255, 255);
	private Color32 selected = new Color32(100, 255, 255, 255);
    private Dictionary<string, string> textDisplay = new Dictionary<string, string>();
    
    void Awake()
    {
        textDisplay.Add("Alpha1", "1");
        textDisplay.Add("Alpha2", "2");
        textDisplay.Add("Alpha3", "3");
        textDisplay.Add("Alpha4", "4");
        textDisplay.Add("Alpha5", "5");
        textDisplay.Add("JoystickButton4", "LB");
        textDisplay.Add("JoystickButton5", "RB");
        textDisplay.Add("JoystickButton7", "Start");
        textDisplay.Add("JoystickButton8", "L3");
        textDisplay.Add("JoystickButton1", "B");
        textDisplay.Add("JoystickButton2", "X");
        textDisplay.Add("JoystickButton3", "Y");
    }

    void Start()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;
        
        string decrypted = "oops";
        pause.text = textDisplay.TryGetValue(PersistentManager.Instance.PauseKey.ToString(), out decrypted) ?
            textDisplay[PersistentManager.Instance.PauseKey.ToString()] :
            PersistentManager.Instance.PauseKey.ToString();
        zoom.text = textDisplay.TryGetValue(PersistentManager.Instance.ZoomKey.ToString(), out decrypted) ?
            textDisplay[PersistentManager.Instance.ZoomKey.ToString()] :
            PersistentManager.Instance.ZoomKey.ToString();
        grab.text = textDisplay.TryGetValue(PersistentManager.Instance.GrabKey.ToString(), out decrypted) ?
            textDisplay[PersistentManager.Instance.GrabKey.ToString()] :
            PersistentManager.Instance.GrabKey.ToString();
        jump.text = textDisplay.TryGetValue(PersistentManager.Instance.JumpKey.ToString(), out decrypted) ?
            textDisplay[PersistentManager.Instance.JumpKey.ToString()] :
            PersistentManager.Instance.JumpKey.ToString();
        humidity.text = textDisplay.TryGetValue(PersistentManager.Instance.HumidityKey.ToString(), out decrypted) ?
            textDisplay[PersistentManager.Instance.HumidityKey.ToString()] :
            PersistentManager.Instance.HumidityKey.ToString();
		if(tl.Contains("wind")) {
			wind.text = textDisplay.TryGetValue(PersistentManager.Instance.WindKey.ToString(), out decrypted) ?
                textDisplay[PersistentManager.Instance.WindKey.ToString()] :
                PersistentManager.Instance.WindKey.ToString();
		}
        if(tl.Contains("temperature")) {
            temperature.text = textDisplay.TryGetValue(PersistentManager.Instance.TemperatureKey.ToString(), out decrypted) ?
                textDisplay[PersistentManager.Instance.TemperatureKey.ToString()] :
                PersistentManager.Instance.TemperatureKey.ToString();
        }
    }

    void Update()
    {
        if (currentKey != null)
        {
            string preString = "";
            // Debug.Log("pressed "+pressed.ToString());
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
                 if(Input.GetKeyDown(vKey)){
                    Debug.Log("GUI "+vKey);
                    preString = vKey.ToString();
                    break;
                }
            }
            if (preString!="" && preString!="Return" && preString!="JoystickButton0")
            {
                string whatever = "oops";
                PlayerPrefs.SetString(currentKey.name, preString);
                PlayerPrefs.Save();
                PersistentManager.Instance.SetKeys();
                currentKey.transform.GetChild(0).GetComponent<Text>().text =
                    textDisplay.TryGetValue(preString, out whatever) ?
                        textDisplay[preString] : preString;
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

}
