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
    public float slowVolume;
    public float fastVolume;


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
		if(clampAngle != angle)
		{
			canSee = false; //== true deactivate
        }
        else 
        {
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (playerObject.transform.position - this.transform.position).normalized, vision, PersistentManager.Instance.blocksProjectiles);
			if (hit.transform!=null && hit.transform.CompareTag("Player") && !PersistentManager.Instance.immobile)
			{ //if cansee == false then activates sound
                if (canSee == false)
                {
                    source.volume = fastVolume;
                    source.clip = fastbeep;
                    source.Play();                        
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
            else
            {
                canSee = false;
            }
		}
		if (canSee == false)
		{
            source.volume = slowVolume;
            source.clip = slowbeep;
            if (!source.isPlaying) { source.Play(); }
		}

        if (counter >= rate && canSee)
        {
            if(source) {
                source.PlayOneShot(shoot, 1f);
            }
            Transform childTransform = this.transform.GetChild(0).transform;
        	GameObject projectile = Instantiate(enemyProjectile,
                new Vector2(transform.position.x + childTransform.rotation.x, childTransform.position.y + transform.rotation.y),
                Quaternion.identity);// projectile sound
        	ProjectileActivity pa = projectile.GetComponent<ProjectileActivity>();
        	pa.speed = projectileSpeed;

        	counter = 0;
        }
    }
}
