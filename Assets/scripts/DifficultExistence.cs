using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultExistence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PersistentManager.Instance.difficulty != "challenging")
        {
        	this.gameObject.SetActive(false);
        }
    }
}
