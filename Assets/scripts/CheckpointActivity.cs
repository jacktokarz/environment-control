﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointActivity : MonoBehaviour
{

	private Animator flowerGrowAnim;
	public bool alreadyChecked;

    void Start()
    {
    	Debug.Log("this scene is: "+SceneManager.GetActiveScene().buildIndex);
        flowerGrowAnim = this.transform.GetChild(0).GetComponent(typeof (Animator)) as Animator;
    	if( PersistentManager.Instance.Checkpoints.Contains(SceneManager.GetActiveScene().buildIndex))
    	{
    		alreadyChecked = true;
    		flowerGrowAnim.SetBool("grown", true);
    	}

    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.CompareTag("Player"))
    	{
            if (!alreadyChecked) {
        		activateCheckpoint();
            }
            else {
                setCheckpoint();
            }
    	}
    }

    void activateCheckpoint()
    {
		flowerGrowAnim.SetBool("grow", true);
		PersistentManager.Instance.Checkpoints.Add(SceneManager.GetActiveScene().buildIndex);
		PersistentManager.Instance.lastCheckpoint = SceneManager.GetActiveScene().buildIndex;
		bool saved = PersistentManager.Instance.Save();
		Debug.Log("saved ? "+saved);
		alreadyChecked = true;
    }

    void setCheckpoint() {
        PersistentManager.Instance.lastCheckpoint = SceneManager.GetActiveScene().buildIndex;
        bool saved = PersistentManager.Instance.Save();
    }

}