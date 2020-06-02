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

    public void SetFirstCutsceneMessage()
    {
        Queue<string> firstCutsceneMessages = new Queue<string>();
        firstCutsceneMessages.Enqueue("I had something really good here... I swear.");
        firstCutsceneMessages.Enqueue("Jump");
        firstCutsceneMessages.Enqueue("");
        firstCutsceneMessages.Enqueue("Another great message.");
        firstCutsceneMessages.Enqueue("Horizontal");
        firstCutsceneMessages.Enqueue("");
        StartCoroutine(PersistentManager.Instance.MakeMessage(firstCutsceneMessages));
    }
}
