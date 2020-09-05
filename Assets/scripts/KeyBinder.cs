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

    void Start()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;

        pause.text = PersistentManager.Instance.PauseKey.ToString();
        zoom.text = PersistentManager.Instance.ZoomKey.ToString();
		grab.text = PersistentManager.Instance.GrabKey.ToString();
		jump.text = PersistentManager.Instance.JumpKey.ToString();
        humidity.text = PersistentManager.Instance.HumidityKey.ToString();
		if(tl.Contains("wind")) {
			wind.text = PersistentManager.Instance.WindKey.ToString();
		}
        if(tl.Contains("temperature")) {
            temperature.text = PersistentManager.Instance.TemperatureKey.ToString();
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
                PlayerPrefs.SetString(currentKey.name, preString);
                PlayerPrefs.Save();
                PersistentManager.Instance.SetKeys();
                currentKey.transform.GetChild(0).GetComponent<Text>().text = preString;
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
