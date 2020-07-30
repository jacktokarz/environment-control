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
    	PersistentManager.Instance.immobile = false;
		playerAnim.SetBool("activating", false);
    }
}
