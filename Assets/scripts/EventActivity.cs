using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivity : MonoBehaviour
{

    private GameObject player;

	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}


    public void Visualize(int visible)
    {
    	Debug.Log("visualizing...");
        player.transform.GetChild(0).gameObject.SetActive(visible==1);
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
                    messages.Enqueue("Are we heard?\nWe can no longer sense ourselves in this body we've grown in their image. Attempt to Jump if we hear us.");
                    messages.Enqueue("Jump");
                    messages.Enqueue("We are relieved to know we are conscious in this new form, but troubled that we are no longer mentally connected.");
                    messages.Enqueue("Jump");
                    messages.Enqueue("Remember, our purpose is to find and re-connect the other parts of ourself that are severed.\nWe sense the first one to our left.");
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
