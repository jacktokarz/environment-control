﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSprite : MonoBehaviour
{
	SpriteRenderer sr;
	void Start() 
	{
		sr = this.GetComponent<SpriteRenderer>();
	}
	void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.CompareTag("Player"))
    	{
    		Debug.Log("setting color");
    		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0.2f);
    		Debug.Log("color is "+sr.color);
    	}
    }

    void OnTriggerExit2D(Collider2D col)
    {
    	if(col.CompareTag("Player"))
    	{
    		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
    	}
    }
}
