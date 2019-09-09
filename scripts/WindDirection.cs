using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDirection : MonoBehaviour
{

	public Vector2 direction;

	void Awake()
    {
    	float zRot = this.transform.parent.transform.rotation.eulerAngles.z;
        Debug.Log("z rot is "+zRot);
    	if (zRot == 0)
    	{
    		direction = new Vector2(1, 0);
    	}
    	else if (zRot == 180)
    	{
    		direction = new Vector2(-1, 0);
    	}
    	else if (zRot == 90)
    	{
    		direction = new Vector2(0, 1);
    	}
        else if (zRot == 270)
        {
            Debug.Log("facing down");
            direction = new Vector2(0, -1);
        }
        else
        {
            direction = new Vector2(0,0);
        }
    }
}
