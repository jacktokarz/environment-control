using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
	public GameObject screens;
	private int activeScreen = 0;

	void Start () {
		Time.timeScale = 1;
		UnPause();
		ScreenSwitch(0);
	}

	void Update ()
	{
		//uses the p button to pause and unpause the game
		if (Input.GetButtonDown("Pause"))
		{
			Debug.Log("Time scale is "+Time.timeScale);
			if(Time.timeScale == 1)
			{
				ActivatePause();
			} else if (Time.timeScale == 0){
				UnPause();
			}
		}
	}

	public void ScreenSwitch(int changeValue)
	{
		int screenCount = screens.transform.childCount;
		int newScreen = activeScreen + changeValue;
		if (newScreen >= screenCount)
		{
			newScreen = newScreen - screenCount;
		}
		if (newScreen < 0)
		{
			newScreen = newScreen + screenCount;
		}
		screens.transform.GetChild(activeScreen).gameObject.SetActive(false);
		screens.transform.GetChild(newScreen).gameObject.SetActive(true);
		activeScreen = newScreen;
	}

	//shows objects with ShowOnPause tag
	public void ActivatePause()
	{
		Time.timeScale = 0;
		for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
		{
			this.transform.GetChild(childIndex).gameObject.SetActive(true);
		}
	}

	//hides objects with ShowOnPause tag
	public void UnPause()
	{
		Time.timeScale = 1;
		for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
		{
			this.transform.GetChild(childIndex).gameObject.SetActive(false);
		}
	}
}
