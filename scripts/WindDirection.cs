using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDirection : MonoBehaviour
{

	public int direction;

	void Awake()
    {
    	float zRot = this.transform.parent.transform.rotation.eulerAngles.z;
    	if (zRot == 0)
    	{
    		direction = 1;
    	}
    	else if (zRot == 180)
    	{
    		direction = -1;
    	}
    	else
    	{
    		direction = 0;
    	}
    }
}
