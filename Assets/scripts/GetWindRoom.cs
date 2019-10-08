using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetWindRoom : MonoBehaviour
{
	private int changeDir = 1;
	private int counter = 0;
    private int state;

    void Start()
    {
        state = (SceneManager.GetActiveScene().buildIndex == 5) ? 1 : 2;
        if (state == 2) {
            EnvironmentEffect.Instance.setWind(3);
        }
        else {
            EnvironmentEffect.Instance.setWind(0);
        }
    	PersistentManager.Instance.Wind.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PersistentManager.Instance.TreasureList.Contains("wind")) {
            state = 3;
        }

        if (state == 1)
        {
        	if (counter >= PersistentManager.Instance.getWindRoomChangeDelay)
        	{
	        	changeDir = ((PersistentManager.Instance.windLevel == PersistentManager.Instance.maxWind) || (PersistentManager.Instance.windLevel == PersistentManager.Instance.maxWind)) ? changeDir * -1 : changeDir;
                EnvironmentEffect.Instance.setWind(PersistentManager.Instance.windLevel + changeDir);
	        	counter = 0;
        	}
        	else
        	{
        		counter++;
        	}
        }        
    }
}
