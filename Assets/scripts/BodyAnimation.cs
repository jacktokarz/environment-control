using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
    public PlayerAudio pa;
	private Animator playerAnim;

    void Start()
    {
        playerAnim = this.GetComponent<Animator>();
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

    void PlayLandingSound(float yVel)
    {
        pa.PlayLandSound(yVel);
    }

    void PlayStepSound()
    {
    	pa.PlayStepSound();
    }
}

