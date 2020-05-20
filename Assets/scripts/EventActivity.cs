using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivity : MonoBehaviour
{

    private GameObject player;


	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}


    public void visualize(int visible)
    {
    	Debug.Log("visualizing...");
        player.transform.GetChild(0).gameObject.SetActive(visible==1);
    }

    public void mobilize(int mobile)
    {
    	Debug.Log("mobilizing...");
        PersistentManager.Instance.immobile = mobile==0;
    }
}
