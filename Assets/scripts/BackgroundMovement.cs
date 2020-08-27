using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{

	public float restartPoint;
	public float startPoint;
	public float movementRate;
    void Update()
    {
    	Vector3 pos = this.transform.position;
        if (pos.y >= restartPoint)
        {
        	transform.position = new Vector3(pos.x, startPoint, pos.z);
        }
        else
        {
        	transform.position = new Vector3(pos.x, pos.y + movementRate, pos.z);
        }
    }
}
