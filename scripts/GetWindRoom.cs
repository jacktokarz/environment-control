using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWindRoom : MonoBehaviour
{
	private int changeDir = 1;
	private int counter = 0;

    void Start()
    {
    	PersistentManager.Instance.changeWind(0);
    	PersistentManager.Instance.Wind.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		List<string> tl = PersistentManager.Instance.TreasureList;
        if (!tl.Contains("wind"))
        {
        	if (counter >= PersistentManager.Instance.getWindRoomChangeDelay)
        	{
	        	bool changed = PersistentManager.Instance.changeWind(changeDir);
	        	if (changed == false)
	        	{
	        		changeDir = changeDir * -1;
	        		PersistentManager.Instance.changeWind(changeDir);
	        	}
	        	counter = 0;
        	}
        	else
        	{
        		counter++;
        	}
        }        
    }
}
