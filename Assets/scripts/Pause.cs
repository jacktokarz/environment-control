using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
	
	void Start () {
		Time.timeScale = 1;
		hidePaused();
	}

	void Update ()
	{

		//uses the p button to pause and unpause the game
		if (Input.GetButtonDown("Pause"))
		{
			Debug.Log("Time scale is "+Time.timeScale);
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				showPaused();
			} else if (Time.timeScale == 0){
				Time.timeScale = 1;
				hidePaused();
			}
		}
	}

	//shows objects with ShowOnPause tag
	public void showPaused(){
		for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
		{
			this.transform.GetChild(childIndex).gameObject.SetActive(true);
		}
	}

	//hides objects with ShowOnPause tag
	public void hidePaused(){
		for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
		{
			this.transform.GetChild(childIndex).gameObject.SetActive(false);
		}
	}
}
