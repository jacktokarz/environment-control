using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileActivity : MonoBehaviour
{
	public float speed;
	private Rigidbody2D rigid;
	private GameObject playerObject;

	void Start()
    {
    	playerObject = GameObject.FindGameObjectWithTag("Player"); 
		rigid = this.GetComponent<Rigidbody2D>();
		rigid.velocity = (playerObject.transform.position - this.transform.position).normalized * speed;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
    	Debug.Log("hitting");
    	if(coll.gameObject.CompareTag("wind"))
    	{
    		Debug.Log("hitting wind");
    		WindDirection wd = coll.gameObject.GetComponent<WindDirection>();
    		float xVal = wd.direction == 0 ? rigid.velocity.x : wd.direction + PersistentManager.Instance.windLevel;
    		float yVal = wd.direction == 0 ? (int)transform.parent.transform.rotation.eulerAngles.z/90 + PersistentManager.Instance.windLevel : rigid.velocity.y;
    		Vector3 blowing = new Vector3(xVal, yVal, 0);
    		rigid.velocity = Vector3.Lerp(rigid.velocity, blowing, 0.1f);
    		return;
    	}
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
		Destroy(this.gameObject);
    }

 }
