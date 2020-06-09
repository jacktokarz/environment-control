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
		PersistentManager.Instance.TreasureList.Add(treasureId);
		Queue<string> nextMessages = new Queue<string>();
		switch (treasureId)
		{
		case "humidity":
			PersistentManager.Instance.Humidity.gameObject.SetActive(true);
			nextMessages.Enqueue("Hold 1 + Press Up / Down\nto Alter the Humidity Level");
			nextMessages.Enqueue("humidity");
			break;
		case "wind":
			nextMessages.Enqueue("Hold 2 + Press up / down\nto Alter the Wind Level");
			nextMessages.Enqueue("wind");
			break;
		}
        Debug.Log("passing a message from "+nextMessages.Count);

		StartCoroutine(PersistentManager.Instance.MakeMessage(nextMessages));
		bool saved = PersistentManager.Instance.Save();
		alreadyGotten = true;
    }

}
