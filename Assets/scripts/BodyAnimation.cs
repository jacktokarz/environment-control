using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
	Animator playerAnim;

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
}
