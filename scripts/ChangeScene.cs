using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
	public int OtherSideOfDoor;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			OpenDoor od = this.GetComponentInParent<OpenDoor>();
			PersistentManager.Instance.lastDoorId = od.doorId;
			PersistentManager.GoToScene(OtherSideOfDoor);
		}
	}
}
