using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterlineActivity : MonoBehaviour
{
	public LayerMask justWater;
	public AudioClip waterBubbles;
	GameObject playerObject;
	private AudioSource source;
	private CapsuleCollider2D thisColl;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("watery start "+playerObject);
        source = GetComponent<AudioSource>();
        thisColl = GetComponent<CapsuleCollider2D>();
        source.clip = waterBubbles;
    }

    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
		if(coll.gameObject == playerObject)
		{	
	    	source.Play();
	    }
	}

    void OnTriggerStay2D(Collider2D coll)
    {
    	if(coll.gameObject == playerObject)
    	{
    		float halfWaterWidth = thisColl.size.x/2 - 10;
    		float halfWaterHeight = thisColl.size.y/2 - 6;
    		Vector2 thisPos = this.transform.position;
    		Vector2 playPos = playerObject.transform.position;
    		float xDiff = Mathf.Abs(playPos.x - thisPos.x);
    		float yDiff = Mathf.Abs(playPos.y - thisPos.y);
    		if(xDiff <= halfWaterWidth+2.5f && yDiff <= halfWaterHeight+2.5f)
    		{
    			source.volume = 1;
    		}
    		else
    		{
    			source.volume = Mathf.Min(Mathf.Abs(thisColl.size.x/2 - xDiff) / 7.5f, Mathf.Abs(thisColl.size.y/2 - yDiff) / 3.5f);
    		}
    	}
    }

    void OnTriggerExit2D(Collider2D coll)
    {
    	source.Stop();
    }
}
