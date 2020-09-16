using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{

	public float restartPoint;
	public float movementRate;
    void Update()
    {
    	Vector3 pos = this.transform.position;
        if (pos.y >= restartPoint)
        {
        	Destroy(this.gameObject);
        }
        else
        {
        	transform.position = new Vector3(pos.x, pos.y + movementRate, pos.z);
        }
    }
}
