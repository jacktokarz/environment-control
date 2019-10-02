using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeValueZone : MonoBehaviour
{
	public string valueType;
	public int value;
	public int originalValue = 0;

	void Start()
	{
		RoomValues rv = this.GetComponentInParent<RoomValues>();
		if(valueType == "temperature")
		{
			originalValue = rv.startingTemperature;
		}
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
	        if(valueType == "temperature")
	        {
	        	if(!PersistentManager.Instance.TreasureList.Contains("temperature"))
	        	{
	        		EnvironmentEffect.Instance.setTemp(value);
	        	}
	        }
	    }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
    	if(coll.gameObject.CompareTag("Player"))
    	{
    		if(valueType == "temperature")
	        {
	        	if(!PersistentManager.Instance.TreasureList.Contains("temperature"))
	        	{
	        		EnvironmentEffect.Instance.setTemp(originalValue);
	        	}
	        }
    	}
    }
}
