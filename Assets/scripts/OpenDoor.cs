using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
	public string doorId;
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;
	public Transform doorObj;
    public SpriteRenderer lightsObj;
    public Color openColor;
    public Color closedColor;
    public float flipped;
    Vector3 doorStartPos;
	Vector3 doorEndPos;
	float doorStartTime;
    Vector3 defaultPos;
    Color startingColor;
    Color desiredColor;
    private AudioSource source;


    void Awake()
    {
    	flipped= this.transform.localScale.y;
        source = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
    	doorStartPos= doorObj.localPosition;
        doorEndPos= doorObj.localPosition;
        defaultPos= doorObj.localPosition;
        desiredColor = lightsObj.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lightsObj.color != desiredColor)
        {

            float timeSinceStarted = Time.time - doorStartTime;
            float percentageComplete = timeSinceStarted / (PersistentManager.Instance.doorMoveSpeed / 30);
            lightsObj.color = Color.Lerp(startingColor, desiredColor, percentageComplete);
            if (percentageComplete >= 1)
            {
                doorStartTime = Time.time;
            }
        }
    	else if (doorObj.localPosition != doorEndPos)
        {
            float journeyLength = Vector3.Distance(doorStartPos, doorEndPos);
            float distCovered = (Time.time - doorStartTime) * PersistentManager.Instance.doorMoveSpeed;
            doorObj.localPosition = Vector3.Lerp(doorStartPos, doorEndPos, distCovered / journeyLength);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            openDoor();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
    	if (col.CompareTag("Player"))
    	{
    		closeDoor();
    	}
    }

    void openDoor()
    {
        source.PlayOneShot(openDoorSound);
        //if (speaker.time == 0.3) { speaker.Stop();}
        doorStartTime = Time.time;
        startingColor = lightsObj.color;
        desiredColor = openColor;
    	doorStartPos = doorObj.localPosition;
    	doorEndPos = defaultPos + new Vector3(PersistentManager.Instance.doorMoveDistance * -1, 0, 0);
    }

    void closeDoor()
    {
        source.PlayOneShot(closeDoorSound);
    	doorStartTime= Time.time;
        startingColor = lightsObj.color;
        desiredColor = closedColor;
        doorStartPos = doorObj.localPosition;
    	doorEndPos= defaultPos;
    }

    public void spawn(GameObject player) {
        player.SetActive(true);
        PersistentManager.Instance.immobile = false;
    }
}
