using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPadActivity : MonoBehaviour
{
	private float inFan;
	private float startX;
	private float move;
	private Rigidbody2D rigid;
	public Vector3 velo;

	private Vector3 m_Velocity = Vector3.zero;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;


    void Start()
    {
    	rigid = GetComponent<Rigidbody2D>();
        startX = this.transform.position.x;
    }

    void Update()
    {
    	float vMove = rigid.velocity.y;
    	float currX = this.transform.position.x;
    	move = 0;
    	if (Mathf.Abs(currX - startX) > 0.1)
		{
			move = (PersistentManager.Instance.lilyPadTravelSpeed + 1.5f) * (currX > startX ? -1 : 1);
		}

    	if(inFan != Mathf.PI)
    	{
    		Debug.Log("in fan is: "+inFan);
	    	if (PersistentManager.Instance.windLevel > 0)
			{
				if (inFan == 180)
				{
					move = -1 * (PersistentManager.Instance.lilyPadTravelSpeed + PersistentManager.Instance.windLevel);
				}
				else if (inFan == 0)
				{
					move = PersistentManager.Instance.lilyPadTravelSpeed + PersistentManager.Instance.windLevel;
				}
				else if (inFan == 90)
				{
					vMove = PersistentManager.Instance.lilyPadTravelSpeed + PersistentManager.Instance.windLevel;
				}
			}
		}
		Debug.Log("creating move: "+move);
		Vector3 targetVelocity = new Vector2(move, vMove);
        // And then smoothing it out and applying it to the character
        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        velo = rigid.velocity;
    }

    void OnTriggerStay2D(Collider2D col)
    {
    	if (col.CompareTag("wind"))
    	{
			inFan = col.transform.parent.transform.rotation.eulerAngles.z;
    	}
    }

    void OnTriggerExit2D(Collider2D col)
    {
    	Debug.Log("left");
    	if (col.CompareTag("wind"))
    	{
			inFan = Mathf.PI;
    	}
    }
}
