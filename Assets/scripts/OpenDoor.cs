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
    public int flipped;
    Vector3 doorStartPos;
	Vector3 doorEndPos;
	float doorStartTime;
    Vector3 defaultPos;
    Color startingColor;
    Color desiredColor;
    private AudioSource source;


    void Awake()
    {
    	flipped= this.transform.rotation.eulerAngles.z > 180 ? -1 : 1;
        source = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
    	doorStartPos= doorObj.localPosition;
        doorEndPos= doorObj.localPosition;
        defaultPos= doorObj.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lightsObj.color != desiredColor)
        {

            float timeSinceStarted = Time.time - doorStartTime;
            float percentageComplete = timeSinceStarted / (PersistentManager.Instance.doorMoveSpeed / 4);
        Debug.Log("changing light "+percentageComplete);
            lightsObj.color = Color.Lerp(startingColor, desiredColor, percentageComplete);
            if (percentageComplete >= 95)
            {
                doorStartTime = Time.time;
            }
        }
    	else if (doorObj.localPosition != doorEndPos)
        {
            float journeyLength = Vector3.Distance(doorStartPos, doorEndPos);
            float distCovered = (Time.time - doorStartTime) * PersistentManager.Instance.doorMoveSpeed;
        Debug.Log("changing door "+distCovered);        
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
        Debug.Log("open it");
        source.PlayOneShot(openDoorSound);
        //if (speaker.time == 0.3) { speaker.Stop();}
        doorStartTime = Time.time;
        startingColor = lightsObj.color;
        desiredColor = openColor;
    	doorStartPos = doorObj.localPosition;
    	doorEndPos = doorObj.localPosition + new Vector3(PersistentManager.Instance.doorMoveDistance * flipped, 0, 0);
       // if (childObj.localPosition == doorEndPos) { speaker.Stop(); }
    }

    void closeDoor()
    {
        source.PlayOneShot(closeDoorSound);
    	doorStartTime= Time.time;
        startingColor = lightsObj.color;
        desiredColor = openColor;
        doorStartPos = doorObj.localPosition;
    	doorEndPos= defaultPos;
    }

    public void spawn(GameObject player) {
        player.SetActive(true);
        PersistentManager.Instance.immobile = false;
        Debug.Log("door makes player");

    }
}
