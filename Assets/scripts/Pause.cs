using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
	public GameObject screens;
	public Button initialButton;
	private int activeScreen = 0;

	void Start () {
		Time.timeScale = 0;
		ActivatePause();
		ScreenSwitch(0);
	}

	void Update ()
	{
		//uses the p button to pause and unpause the game
		if (Input.GetKeyDown(PersistentManager.Instance.PauseKey))
		{
			Debug.Log("Time scale is "+Time.timeScale);
			ActivatePause();
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
		bool paused = Time.timeScale==0;
		if (!paused)
		{
			initialButton.Select();
			initialButton.OnSelect(null);
		}
		for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
		{
			this.transform.GetChild(childIndex).gameObject.SetActive(!paused);
		}
		Time.timeScale = paused ? 1 : 0;
	}
}
