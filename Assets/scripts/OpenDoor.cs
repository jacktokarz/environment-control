using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
	public string doorId;
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    public int flipped;
    Vector3 doorStartPos;
	Vector3 doorEndPos;
	Transform childObj;
	float doorStartTime;
    Vector3 defaultPos;
    private AudioSource source;


    void Awake()
    {
    	flipped= this.transform.rotation.eulerAngles.z > 180 ? -1 : 1;
        source = GetComponent<AudioSource>();

    }
    // Start is called before the first frame update
    void Start()
    {
        childObj= this.transform.GetChild(0);
    	doorStartPos= childObj.localPosition;
        doorEndPos= childObj.localPosition;
        defaultPos= childObj.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	if (childObj.localPosition != doorEndPos)
        {
            float journeyLength = Vector3.Distance(doorStartPos, doorEndPos);
            float distCovered = (Time.time - doorStartTime) * PersistentManager.Instance.doorMoveSpeed;
            childObj.localPosition = Vector3.Lerp(doorStartPos, doorEndPos, distCovered / journeyLength);
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
    	doorStartPos = childObj.localPosition;
    	doorEndPos= childObj.localPosition + new Vector3(PersistentManager.Instance.doorMoveDistance * flipped, 0, 0);
    	doorStartTime= Time.time;
       // if (childObj.localPosition == doorEndPos) { speaker.Stop(); }
    }

    void closeDoor()
    {
        source.PlayOneShot(closeDoorSound);
        doorStartPos = childObj.localPosition;
    	doorEndPos= defaultPos;
    	doorStartTime= Time.time;
    }
}
