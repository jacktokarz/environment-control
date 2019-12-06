using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureActivity : MonoBehaviour
{
    public AudioClip treasureSound;
	public string treasureId;
	private Animator openChestAnim;
	public bool alreadyGotten;
    private AudioSource source;
    public AudioClip labsounds;

    void Start()
    {
        source = GetComponent<AudioSource>();
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
        source.PlayOneShot(treasureSound, 2.0f);
		openChestAnim.SetBool("opening", true);
		string messageText = "";
		MessageScript ms = PersistentManager.Instance.Message.GetComponent(typeof (MessageScript)) as MessageScript;
		PersistentManager.Instance.TreasureList.Add(treasureId);
		switch (treasureId)
		{
		case "humidity":
			PersistentManager.Instance.Humidity.gameObject.SetActive(true);
			messageText = "Hold 1 + Press Up / Down\nto Alter the Humidity Level";
			ms.escapeKey = "1";
			ms.secondaryKey = "up";
			break;
		case "wind":
			PersistentManager.Instance.Wind.gameObject.SetActive(true);
			messageText = "Hold 2 + Press up / down\nto Alter the Wind Level";
			ms.escapeKey = "2";
			ms.secondaryKey = "up";
			break;
		}
		PersistentManager.Instance.Message.GetComponent<Text>().text = messageText;
		PersistentManager.Instance.Message.GetComponent<Animator>().SetBool("visible", true);
		bool saved = PersistentManager.Instance.Save();
		Debug.Log("saved ? "+saved);
		alreadyGotten = true;
    }

}
