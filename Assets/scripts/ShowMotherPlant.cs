using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMotherPlant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PersistentManager.Instance.TreasureList.Contains("wind"))
        {
        	this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
