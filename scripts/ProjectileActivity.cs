﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileActivity : MonoBehaviour
{
	public float speed;
	private Rigidbody2D rigid;
	private GameObject playerObject;

	void Start()
    {
    	playerObject = GameObject.FindGameObjectWithTag("Player"); 
		rigid = this.GetComponent<Rigidbody2D>();
		rigid.velocity = (playerObject.transform.position - this.transform.position).normalized * speed;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
    	Debug.Log("hitting");
    	if(coll.gameObject.CompareTag("wind"))
    	{
    		Debug.Log("hitting wind");
    		WindDirection wd = coll.gameObject.GetComponent<WindDirection>();
            float xVal = wd.direction.x == 0 ? rigid.velocity.x : wd.direction.x * PersistentManager.Instance.windLevel;
    		float yVal = wd.direction.y == 0 ? rigid.velocity.y : wd.direction.y * PersistentManager.Instance.windLevel;
    		Vector3 blowing = new Vector3(xVal, yVal, 0);
    		rigid.velocity = Vector3.Lerp(rigid.velocity, blowing, 0.1f);
    		return;
    	}
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
		Destroy(this.gameObject);
    }

 }
