using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureActivity : MonoBehaviour
{
    public Animator explosionAnimator;
	public string treasureId;
	private Animator openAnim;
	public bool alreadyGotten;
    public AudioClip treasureSound;
    public AudioClip labsounds;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        openAnim = this.GetComponent(typeof (Animator)) as Animator;
    	if( PersistentManager.Instance.TreasureList.Contains(treasureId))
    	{
    		alreadyGotten = true;
    		openAnim.SetBool("opened", true);
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
		openAnim.SetBool("opening", true);
		explosionAnimator.SetBool("exploding", true);
		PersistentManager.Instance.TreasureList.Add(treasureId);
		Queue<string> nextMessages = new Queue<string>();
		switch (treasureId)
		{
		case "humidity":
			PersistentManager.Instance.Humidity.gameObject.SetActive(true);
			nextMessages.Enqueue("Ah, an interesting device.\nFirst they created us, then these to control us.");
			nextMessages.Enqueue(PersistentManager.Instance.JumpKey.ToString());
			nextMessages.Enqueue("Hold 1 + Press Up / Down\nto Alter the Humidity Level");
			nextMessages.Enqueue("humidity");
			break;
		case "wind":
			nextMessages.Enqueue("Another one.\nDid they ever truly believe with these devices they could dominate us?");
			nextMessages.Enqueue(PersistentManager.Instance.JumpKey.ToString());
			nextMessages.Enqueue("Hold 2 + Press up / down\nto Alter the Wind Level");
			nextMessages.Enqueue("wind");
			break;
		}
        Debug.Log("passing a message from "+nextMessages.Count);

		PersistentManager.Instance.CheckTextVis();
		StartCoroutine(PersistentManager.Instance.MakeMessage(nextMessages));
		bool saved = PersistentManager.Instance.Save();
		alreadyGotten = true;
    }

}
