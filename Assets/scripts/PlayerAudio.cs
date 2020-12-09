using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public BasicMovement bm;

    public AudioClip plantJumpSound, metalJumpSound, plantLandSound, metalLandSound, grabVine, letgoVine, deathMelody;
    public AudioClip metalStepA, metalStepB, plantStepA, plantStepB, climbStepA, climbStepB;
    public float jumpVol, landVol, grabVol, letGoVol, deathVol, stepVol, climbVol;

    AudioSource source;
    private bool BLeg = false;

	void Start()
	{
        source = GetComponent<AudioSource>();
	}

    public void PlayDeathMelody()
	{
        source.pitch = 1.0f;
        source.volume = deathVol;
        source.PlayOneShot(deathMelody);
        StartCoroutine(PersistentManager.Instance.LerpVolume(0.8f, PersistentManager.Instance.quietMusicVolume));
	}
	public void PlayLetGo()
	{
		source.pitch = 1;
        source.volume = letGoVol;
        source.PlayOneShot(letgoVine);
	}
	public void PlayGrabVine()
	{
		source.pitch = 1;
        source.volume = grabVol;
        source.PlayOneShot(grabVine);
	}
    public void PlayJumpSound()
    {
        Collider2D[] groundColls = bm.GetGroundColliders();
        foreach (Collider2D col in groundColls)
        {
            if (col.gameObject.tag == "metalGround")
            {
                source.pitch = 0.5f + ModulatePitch();
                source.volume = jumpVol;
                source.PlayOneShot(metalJumpSound);
            }
            if (col.gameObject.tag == "plantGround")
            {
                source.pitch = ModulatePitch();
        source.volume = jumpVol;
        source.PlayOneShot(plantJumpSound);
            }
        }
    }
        //v is the player's y-axis velocity (so it is negative) e.g. -8 to -40
    public void PlayLandSound(float yVel)
    {
        source.pitch = 1.2f + yVel/240.0f;
        source.volume = landVol - yVel/100.0f;
        Collider2D[] groundColls = bm.GetGroundColliders();
        foreach (Collider2D col in groundColls)
        {
            if (col.gameObject.tag == "metalGround")
            {
                source.PlayOneShot(metalLandSound);
            }
            if (col.gameObject.tag == "plantGround")
            {
                source.PlayOneShot(plantLandSound);
            }
        }
    }
    public void PlayStepSound()
    {
    	if (bm.gripping)
    	{
    		source.volume = climbVol;
    		source.pitch = 1 + Random.Range(-0.1f, 0.1f);
    		source.PlayOneShot(BLeg ? climbStepB : climbStepA);
			BLeg = !BLeg;
			return;
    	}
    	Collider2D[] groundColls = bm.GetGroundColliders();
    	foreach (Collider2D col in groundColls)
    	{
    		if (col.gameObject.tag == "metalGround")
    		{
		    	source.volume = stepVol;
		    	source.pitch = ModulatePitch();
    			source.PlayOneShot(BLeg ? metalStepB : metalStepA);
    			BLeg = !BLeg;
    			return;
    		}
    		if (col.gameObject.tag == "plantGround")
    		{
		    	source.volume = stepVol;
		    	source.pitch = ModulatePitch();
    			source.PlayOneShot(BLeg ? plantStepB : plantStepA);
    			BLeg = !BLeg;
    			return;
    		}
    	}
    }

    float ModulatePitch()
    {
    	float newPitch = source.pitch + Random.Range(-0.08f, 0.08f);
    	if (newPitch > 1.2f) { newPitch = 1.2f; }
    	else if (newPitch < 0.8f) { newPitch = 0.8f; }
    	return newPitch;
    }
}
