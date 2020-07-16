using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBaseActivity : MonoBehaviour
{
    public AudioClip vineGrowSound;
    private AudioSource source;
    private Animator baseAnim;
    private Transform sibling;
    private LineGrow siblingScript;
    void Start()
    {
        baseAnim = this.transform.GetComponent(typeof (Animator)) as Animator;        
    	sibling = this.transform.parent.GetChild(this.transform.GetSiblingIndex() + 1);
    	siblingScript = sibling.GetComponent(typeof (LineGrow)) as LineGrow;
        source = GetComponent<AudioSource>();
        source.clip = vineGrowSound;
    }

    void Update()
    {
    	float vineHeight = siblingScript.currentHeight;
    	if ( vineHeight < 0.75f )
    	{
	        baseAnim.SetBool("open", false);
	        if (source.isPlaying) { source.Stop(); }
    	}
    	
    	if ( PersistentManager.Instance.humidityLevel > 0 )
    	{
        	baseAnim.SetBool("open", true);
            if (PersistentManager.Instance.humidityLevel == 1) { source.pitch = 1.0F; }
            if (PersistentManager.Instance.humidityLevel == 2) { source.pitch = 1.2F; }
            if (PersistentManager.Instance.humidityLevel == 3) { source.pitch = 1.4F; }
            if (PersistentManager.Instance.humidityLevel == -1 ) {source.pitch = 0.75F;}
            if (PersistentManager.Instance.humidityLevel == -2) { source.pitch = 0.55F; }
            if (PersistentManager.Instance.humidityLevel == -3) { source.pitch = 0.35F; }
            if (!source.isPlaying) { source.Play(); }
    	}
    	else {
    		if (source.isPlaying) { source.Stop(); }
    	}
    }
}
