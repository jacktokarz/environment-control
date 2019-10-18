﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
	public string doorId;

	public int flipped;
    Vector3 doorStartPos;
	Vector3 doorEndPos;
	Transform childObj;
	float doorStartTime;
    Vector3 defaultPos;

    void Awake()
    {
    	flipped= this.transform.rotation.eulerAngles.z > 180 ? -1 : 1;
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
    	doorStartPos= childObj.localPosition;
    	doorEndPos= childObj.localPosition + new Vector3(PersistentManager.Instance.doorMoveDistance * flipped, 0, 0);
    	doorStartTime= Time.time;
    }

    void closeDoor()
    {
    	doorStartPos= childObj.localPosition;
    	doorEndPos= defaultPos;
    	doorStartTime= Time.time;
    }
}