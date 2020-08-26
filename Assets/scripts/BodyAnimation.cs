using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
	private Animator playerAnim;
    private GameObject pollen;

    void Start()
    {
        playerAnim = this.GetComponent<Animator>();
        Debug.Log("this is "+this.name);
        pollen = this.transform.Find("playerPollen").gameObject;
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
        pollen.SetActive(true);
        StartCoroutine(PersistentManager.Instance.WaitToActivate(pollen, false, 6f));
    }
}
