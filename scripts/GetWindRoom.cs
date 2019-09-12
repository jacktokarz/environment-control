using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWindRoom : MonoBehaviour
{
	private int changeDir = 1;
	private int counter = 0;
    private int state;

    void Start()
    {
        state = 1;
    	PersistentManager.Instance.changeWind(0);
    	PersistentManager.Instance.Wind.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		List<string> tl = PersistentManager.Instance.TreasureList;
        if (tl.Contains("wind")) {
            state = 3;
        }

        if (state == 1)
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

    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.CompareTag("Player")) {
            state = 2;

            PersistentManager.Instance.changeWind(PersistentManager.Instance.maxWind - PersistentManager.Instance.windLevel);
        }
    }
}
