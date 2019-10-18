using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetWindRoom : MonoBehaviour
{
	private int changeDir = 1;
	private int counter = 0;

    void Start()
    {
    	PersistentManager.Instance.Wind.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PersistentManager.Instance.TreasureList.Contains("wind"))
        {
        	if (counter >= PersistentManager.Instance.getWindRoomChangeDelay)
        	{
                int wl = PersistentManager.Instance.windLevel;
	        	changeDir = ((wl == PersistentManager.Instance.maxWind) || (wl == PersistentManager.Instance.minWind)) ? changeDir * -1 : changeDir;
                EnvironmentEffect.Instance.setWind(wl + changeDir);
	        	counter = 0;
        	}
        	else
        	{
        		counter++;
        	}
        }        
    }
}
