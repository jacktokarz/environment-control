using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TimelineActivity : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
        	if (PersistentManager.Instance.lastDoorId == "NewGame") {
        		return;
        	}
        }

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
