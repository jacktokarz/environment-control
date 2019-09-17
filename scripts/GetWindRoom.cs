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
            bool keepGoing = true;
            while (keepGoing == true) {
                keepGoing = PersistentManager.Instance.changeWind(1);
            }
        }
        else {
            PersistentManager.Instance.changeWind(0);
        }
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
}
