using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBaseActivity : MonoBehaviour
{
    private Animator baseAnim;
    private Transform sibling;
    private LineGrow siblingScript;
    void Start()
    {
        baseAnim = this.transform.GetComponent(typeof (Animator)) as Animator;        
    	sibling = this.transform.parent.GetChild(this.transform.GetSiblingIndex() + 1);
    	siblingScript = sibling.GetComponent(typeof (LineGrow)) as LineGrow;
    }

    void Update()
    {
    	float vineHeight = siblingScript.currentHeight;
    	if ( PersistentManager.Instance.humidityLevel > 0 )
    	{
        	baseAnim.SetBool("open", true);
    	}

    	if ( vineHeight < 0.2 )
    	{
	        baseAnim.SetBool("open", false);
    	}
    }
}
