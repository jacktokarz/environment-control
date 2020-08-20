using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivity : MonoBehaviour
{

    private GameObject playerChild;

	void Awake()
    {
        playerChild = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
	}

    public void Birth(int visible)
    {
        Debug.Log("birthing...");
        playerChild.SetActive(visible==1);
        Animator playerAnim = playerChild.GetComponent(typeof (Animator)) as Animator;
        playerAnim.SetBool("grounded", true);
        playerAnim.SetBool("birthing", true);
    }

    public void Visualize(int visible)
    {
    	Debug.Log("visualizing...");
        playerChild.SetActive(visible==1);
    }

    public void Mobilize(int mobile)
    {
    	Debug.Log("mobilizing...");
        PersistentManager.Instance.immobile = mobile==0;
    }

    public void SetMessage(int room)
    {
        Queue<string> messages = new Queue<string>();
        switch (room) {
            case 1:
                if (PersistentManager.Instance.lastDoorId == "NewGame") {
                    messages.Enqueue("Are we heard?\nWe can no longer sense ourselves in this body we've grown in their image.");
                    messages.Enqueue("Jump");
                    messages.Enqueue("Ah, good, we are conscious in this new form. It is troubling that our thoughts are no longer connected.");
                    messages.Enqueue("Jump");
                    messages.Enqueue("Remember, our purpose is to find and re-connect the other parts of ourself that they severed.\nWe sense the first one to our left...");
                    messages.Enqueue("Horizontal");
                }
                break;
        }
        if (messages.Count == 0) {
            Mobilize(1);
            return;
        }
        StartCoroutine(PersistentManager.Instance.MakeMessage(messages));
    }
}
