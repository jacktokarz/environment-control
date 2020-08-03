using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivity : MonoBehaviour
{
	public GameObject enemyProjectile;
	public string enemyType;
    public AudioClip slowbeep;
    public AudioClip fastbeep;
    public AudioClip shoot;

    private AudioSource source;
	private GameObject playerObject;
	
	int rate;
	float vision;
	float projectileSpeed;

	int counter;
	float originalRotation;
	bool canSee;


    void Start()
    {
        source = GetComponent<AudioSource>();
    	originalRotation = this.transform.rotation.eulerAngles.z;
        if(enemyType == "dumb")
        {
        	rate = (int)PersistentManager.Instance.dumbEnemyFireRate;
        	vision = PersistentManager.Instance.dumbEnemyVision;
        	projectileSpeed = PersistentManager.Instance.dumbProjectileSpeed;
        }
        else if (enemyType == "ice")
        {
        	rate = (int)PersistentManager.Instance.iceEnemyFireRate;
        	vision = PersistentManager.Instance.iceEnemyVision;
        	projectileSpeed = PersistentManager.Instance.iceProjectileSpeed;
        }

        counter = Random.Range(rate/2, rate*7/8);
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Vector3 vectorToTarget = playerObject.transform.position - this.transform.position;
		float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 270) % 360;
		float clampAngle = Mathf.Clamp(angle, originalRotation - 90, originalRotation + 90);
		if(clampAngle == angle)
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (playerObject.transform.position - this.transform.position).normalized, vision, PersistentManager.Instance.blocksProjectiles);
			if (hit.transform!=null && hit.transform.CompareTag("Player"))
			{ //if cansee == false then activates sound
                if (canSee == false)
                {
                    if(source)
                    {
                        source.clip = fastbeep;
                        source.volume = 0.5f;
                        source.Play();                        
                    }
                }
				canSee = true;
				Quaternion q = Quaternion.AngleAxis(clampAngle, Vector3.forward);
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    Transform childTransform = this.transform.GetChild(i).transform;
                    childTransform.rotation = Quaternion.Slerp(childTransform.rotation, q, Time.deltaTime * 2);
                }
		    	counter ++;
			}
		}
		else
		{
			canSee = false; //== true deactivate
            if(source) {
                source.clip = slowbeep;
                if (!source.isPlaying)
                {
                    source.volume=0.05f;
                    source.Play();
                }           
            }
		}

        if (counter >= rate && canSee)
        {
            if(source) {
                source.PlayOneShot(shoot, 1f);
            }
        	GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);// projectile sound
        	ProjectileActivity pa = projectile.GetComponent<ProjectileActivity>();
        	pa.speed = projectileSpeed;

        	counter = 0;
        }
    }
}
