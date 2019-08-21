using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureActivity : MonoBehaviour
{
	public string treasureId;
	private Animator openChestAnim;
	public bool alreadyGotten;

    void Start()
    {
        openChestAnim = this.GetComponent(typeof (Animator)) as Animator;
    	if( PersistentManager.Instance.TreasureList.Contains(treasureId))
    	{
    		alreadyGotten = true;
    		openChestAnim.SetBool("opened", true);
    	}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.CompareTag("Player") && !alreadyGotten)
    	{
    		getTreasure();
    	}
    }

    void getTreasure()
    {
		openChestAnim.SetBool("opening", true);
		PersistentManager.Instance.TreasureList.Add(treasureId);
		switch (treasureId)
		{
		case "humidity":
			PersistentManager.Instance.Humidity.gameObject.SetActive(true);
			break;
		case "wind":
			PersistentManager.Instance.Wind.gameObject.SetActive(true);
			break;
		}

		bool saved = PersistentManager.Instance.Save();
		Debug.Log("saved ? "+saved);
		alreadyGotten = true;
    }

}
