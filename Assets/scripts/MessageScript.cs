using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScript : MonoBehaviour
{
	[HideInInspector] public string escapeKey;
	[HideInInspector] public string secondaryKey;

	void Start()
	{
		escapeKey = null;
		secondaryKey = null;
	}

    void FixedUpdate()
    {
        if(escapeKey != null)
        {
        	if(Input.GetKey(escapeKey))
        	{
        		if(secondaryKey != null)
        		{
        			if(Input.GetKey(secondaryKey))
        			{
        				disappear();
        			}
        		}
        		else
        		{
        			disappear();
        		}
        	}
        }
    }

    void disappear()
    {
		this.GetComponent<Animator>().SetBool("visible", false);
		escapeKey = null;
		secondaryKey = null;
    }
}
