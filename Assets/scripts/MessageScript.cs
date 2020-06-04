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
        	if(Input.GetButton(escapeKey))
        	{
        		if(secondaryKey != null && secondaryKey != "")
        		{
        			if(Input.GetButton(secondaryKey))
        			{
        				disappear();
        			}
        		}
        		else
        		{
        			Debug.Log("calling prim dis");
        			disappear();
        		}
        	}
        }
    }

    public void disappear()
    {
    	escapeKey = null;
		secondaryKey = null;
		PersistentManager.Instance.uiFader.fadeOut(cg);
    	if(nextMessages.Count==0)
    	{
    		PersistentManager.Instance.immobile=false;
    	}
    	else
    	{
    		StartCoroutine(WaitForTheMessage());
    	}
    }

    IEnumerator WaitForTheMessage() {
    	Debug.Log("waiting");
    	yield return new WaitForSeconds(PersistentManager.Instance.fadeSpeed);
        Debug.Log("messaging");
        StartCoroutine(PersistentManager.Instance.MakeMessage(nextMessages));
    }
}
