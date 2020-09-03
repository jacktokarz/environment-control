using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
	private Animator playerAnim;

    void Start()
    {
        playerAnim = this.GetComponent<Animator>();
        Debug.Log("this is "+this.name);
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
}
