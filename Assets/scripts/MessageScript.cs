using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScript : MonoBehaviour
{
	[HideInInspector] public string escapeKey;
	[HideInInspector] public string secondaryKey;
	public Queue<string> nextMessages;
	public CanvasGroup cg;

	void Start()
	{
		escapeKey = null;
		secondaryKey = null;
		nextMessages = new Queue<string>();
		cg = this.GetComponent<CanvasGroup>();
	}

    void FixedUpdate()
    {
        if(escapeKey != null)
        {
        	if(Input.GetKey(escapeKey))
        	{
        		if(secondaryKey != null && secondaryKey != "")
        		{
        			if(Input.GetKey(secondaryKey))
        			{
        				StartCoroutine(disappear());
        			}
        		}
        		else
        		{
        			StartCoroutine(disappear());
        		}
        	}
        }
    }

    public IEnumerator disappear()
    {
		PersistentManager.Instance.uiFader.fadeOut(cg);
    	if(nextMessages.Count==0)
    	{
    		PersistentManager.Instance.immobile=false;
			escapeKey = null;
			secondaryKey = null;
    	}
    	else
    	{
	    	yield return new WaitForSeconds(PersistentManager.Instance.fadeSpeed);

	        StartCoroutine(PersistentManager.Instance.MakeMessage(nextMessages));
    	}
    }
}
