using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
	public int OtherSideOfDoor;
    public SpriteMask lightsCover;
    GameObject player;
    BrackeysMovement bm;
    Rigidbody2D rigid;

    void Start()
    {
    	player = GameObject.FindGameObjectWithTag("Player");
    	bm = player.GetComponent<BrackeysMovement>();
    	rigid = player.GetComponent<Rigidbody2D>();
    }

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			OpenDoor od = this.GetComponentInParent<OpenDoor>();
			PersistentManager.Instance.lastDoorId = od.doorId;
			PersistentManager.Instance.immobile = true;
			StartCoroutine(ChangeRooms());
		}
	}

	IEnumerator ChangeRooms()
	{
			StartCoroutine(MoveLightsCover());
			yield return new WaitForSeconds(1.1f);
			StartCoroutine(PersistentManager.Instance.GoToScene(OtherSideOfDoor));
	}

	IEnumerator MoveLightsCover() {
		float lerpTime = 1.5f;
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        float start = lightsCover.transform.position.x;

        while (true)
        {
        	float direction = (bm.m_FacingRight ? 1 : -1);
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;
            Debug.Log("percentage light moving "+percentageComplete);

            float currentValue = Mathf.Lerp(start, start + (direction*10f), percentageComplete);

            lightsCover.transform.position = new Vector2(currentValue, lightsCover.transform.position.y);
            rigid.velocity = new Vector2(direction * 6f, -3f);

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }

	}
}
