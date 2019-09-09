using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbEnemyActivity : MonoBehaviour
{
	public GameObject dumbEnemyProjectile;

	private GameObject playerObject;
	private int counter;
	public float originalRotation;
	public bool canSee;
    void Start()
    {
    	originalRotation = this.transform.rotation.eulerAngles.z > 180 ? this.transform.rotation.eulerAngles.z - 360 : this.transform.rotation.eulerAngles.z;
    	Debug.Log("first rot "+originalRotation);
        counter = (int)PersistentManager.Instance.dumbEnemyFireRate / 2;
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Vector3 vectorToTarget = playerObject.transform.position - this.transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
		Debug.Log("angle is: "+angle);
		float clampAngle = Mathf.Clamp(angle, originalRotation - 90, originalRotation + 90);
		Debug.Log("clamp angle is: "+clampAngle);
		if(clampAngle == angle)
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (playerObject.transform.position - this.transform.position).normalized, PersistentManager.Instance.dumbEnemyVision, PersistentManager.Instance.blocksProjectiles);
			if (hit.transform!=null && hit.transform.CompareTag("Player"))
			{
				canSee = true;
				Quaternion q = Quaternion.AngleAxis(clampAngle, Vector3.forward);
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, Time.deltaTime * 2);
		    	counter ++;
			}
		}
		else
		{
			canSee = false;
		}

        if (counter >= PersistentManager.Instance.dumbEnemyFireRate && canSee)
        {
        	GameObject projectile = Instantiate(dumbEnemyProjectile, transform.position, Quaternion.identity);
        	ProjectileActivity pa = projectile.GetComponent<ProjectileActivity>();
        	pa.speed = PersistentManager.Instance.dumbProjectileSpeed;

        	counter = 0;
        }
    }
}
