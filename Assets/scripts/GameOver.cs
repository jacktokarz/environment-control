using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Button initialButton;
	bool active = false;
	Text GOtext;
	Color GOcolor;

    void OnEnable()
    {
    	active = true;
        GOtext = GetComponentInChildren<Text>();
        GOcolor = GOtext.color;
        GOtext.color = new Color (GOcolor.r, GOcolor.g, GOcolor.b, 0.0f);
        GOtext.gameObject.SetActive(true);
        initialButton.Select();
        initialButton.OnSelect(null);
    }

    void OnDisable()
    {
    	active = false;
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
	            GOtext.color = new Color (GOcolor.r, GOcolor.g, GOcolor.b, GOtext.color.a + 0.004f);
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
        StartCoroutine(PersistentManager.Instance.GoToScene(PersistentManager.Instance.lastCheckpoint));
    }
    public void Quit()
    {
    	PersistentManager.Instance.lastDoorId = "Respawn";
        StartCoroutine(PersistentManager.Instance.GoToScene(0));
    }
}
