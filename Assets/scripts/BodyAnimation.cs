using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
    public AudioClip landingSound, metalStepA, metalStepB, plantStepA, plantStepB;
    public BasicMovement bm;
	private Animator playerAnim;
	private bool BLeg = false;
    private AudioSource source;

    void Start()
    {
        playerAnim = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void endActivationAnimation()
    {
    	Debug.Log("freeing body");
    	PersistentManager.Instance.immobile = false;
		playerAnim.SetBool("activating", false);
    }

    void CreatePollen()
    {
        Debug.Log("pollenating");
        PersistentManager.Instance.SeePollen();
    }

    void PlayLandingSound()
    {
        source.PlayOneShot(landingSound);
    }

    void PlayStepSound()
    {
    	Collider2D[] groundColls = bm.GetGroundColliders();
    	foreach (Collider2D col in groundColls)
    	{
    		if (col.gameObject.tag == "metal")
    		{
    			source.PlayOneShot(BLeg ? metalStepB : metalStepA);
    			BLeg = !BLeg;
    			return;
    		}
    		if (col.gameObject.tag == "plant")
    		{
    			source.PlayOneShot(BLeg ? plantStepB : plantStepA);
    			BLeg = !BLeg;
    			return;
    		}
    	}
    }
}

