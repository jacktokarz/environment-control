using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

	bool active = false;
	Text GOtext;
	Color GOcolor;

    void OnEnable()
    {
    	Debug.Log("setting active");
    	active = true;
        Time.timeScale = 0;
        GOtext = GetComponentInChildren<Text>();
        GOcolor = GOtext.color;
        GOtext.color = new Color (GOcolor.r, GOcolor.g, GOcolor.b, 0.0f);
        GOtext.gameObject.SetActive(true);
    }

    void OnDisable()
    {
    	Debug.Log("bye bye");
    	active = false;
    	Time.timeScale = 1;
    	foreach (Transform child in transform)
    	{
    		child.gameObject.SetActive(false);
    	}
    	GOtext.gameObject.SetActive(true);
    }

    void Update()
    {
    	if(active)
    	{
	    	if(GOtext.color.a < 1.0f)
	        {
	            GOtext.color = new Color (GOcolor.r, GOcolor.g, GOcolor.b, GOtext.color.a + 0.01f);
	        }
	        else
	        {
	        	foreach (Transform child in transform)
	        	{
	        		child.gameObject.SetActive(true);
	        	}
		        active = false;
	        }
    	}
    }

    public void Respawn()
    {
    	PersistentManager.Instance.lastDoorId = "Respawn";
        PersistentManager.GoToScene(PersistentManager.Instance.lastCheckpoint);
	    this.gameObject.SetActive(false);
    }
    public void Quit()
    {
    	PersistentManager.Instance.lastDoorId = "Respawn";
        PersistentManager.GoToScene(0);
        this.gameObject.SetActive(false);
    }
}
