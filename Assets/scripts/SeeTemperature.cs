using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeTemperature : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D coll)
    {
    	if(coll.gameObject.CompareTag("Player"))
    	{
    		PersistentManager.Instance.Temperature.gameObject.SetActive(true);
    	}
    }
}
