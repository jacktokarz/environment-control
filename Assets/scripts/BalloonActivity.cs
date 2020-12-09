using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonActivity : MonoBehaviour
{
	public float m_MovementSmoothing;
	
	private float startX;
	private float move;
	private Rigidbody2D rigid;
	private Vector3 m_Velocity = Vector3.zero;

	private bool tethered = true;

    void Start()
    {
    	rigid = GetComponent<Rigidbody2D>();
        startX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
    	Collider2D[] overlapper= Physics2D.OverlapBoxAll(this.transform.position,
    		new Vector2(3.5f * this.transform.localScale.x, 2.5f * this.transform.localScale.y),
    		this.transform.rotation.eulerAngles.z);
        if(overlapper.Length > 0)
        {
        	foreach (Collider2D coll in overlapper)
        	{
        		if(coll.CompareTag("wind"))
        		{
        			getBlown(coll.transform.parent.transform.rotation.eulerAngles.z);
        		}
        	}
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
    	if(coll.gameObject.CompareTag("Player"))
		{
			tethered=false;
		}
    }

    void getBlown(float windAngle)
    {
    	if(tethered) {
    		return;
    	}

    	if (PersistentManager.Instance.windLevel > 0)
		{
			Vector3 blow = Vector3.zero;
			if (windAngle == 180)
			{
				blow.x = -1 * (PersistentManager.Instance.balloonTravelSpeed + PersistentManager.Instance.windLevel);
			}
			else if (windAngle == 0)
			{
				blow.x = PersistentManager.Instance.balloonTravelSpeed + PersistentManager.Instance.windLevel;
			}
			else if (windAngle == 90)
			{
				blow.y = PersistentManager.Instance.balloonTravelSpeed + PersistentManager.Instance.windLevel;
			}
	        // And then smoothing it out and applying it to the character
	        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, blow, ref m_Velocity, m_MovementSmoothing);
		}
    }
}
