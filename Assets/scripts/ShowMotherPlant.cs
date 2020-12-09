using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMotherPlant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	this.transform.GetChild(0).gameObject.SetActive(PersistentManager.Instance.TreasureList.Contains("wind"));
    }
}
