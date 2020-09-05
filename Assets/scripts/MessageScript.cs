using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScript : MonoBehaviour
{
	[HideInInspector] public string escapeKey;
	public Queue<string> nextMessages;
	public CanvasGroup cg;

	void Start()
	{
		escapeKey = null;
		nextMessages = new Queue<string>();
	}

    void FixedUpdate()
    {
    	bool doIt = false;
        if(escapeKey != null)
        {
        	if(escapeKey == "humidity")
        	{
        		if(PersistentManager.Instance.humidityLevel > 0)
        		{
    				doIt = true;
        		}
        	}
        	else if(escapeKey == "wind")
        	{
        		if(PersistentManager.Instance.windLevel < 1)
        		{
        			doIt = true;
        		}
        	}
        	else if(Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), escapeKey)))
        	{
    			doIt = true;
        	}
        }
        if (doIt)
        {
        	StartCoroutine(disappear());
        }
    }

    public IEnumerator disappear()
    {
    	escapeKey = null;
		PersistentManager.Instance.uiFader.fadeOut(cg);
    	if(nextMessages.Count==0)
    	{
    		PersistentManager.Instance.immobile=false;
    	}
    	else
    	{
	    	yield return new WaitForSeconds(PersistentManager.Instance.fadeSpeed + 0.5f);
	        StartCoroutine(PersistentManager.Instance.MakeMessage(nextMessages));
    	}
    }
}
