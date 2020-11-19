using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CollectibleActivity : MonoBehaviour
{
    public int loreID;
    public AudioClip coinsound;
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        Debug.Log("this scene is: "+SceneManager.GetActiveScene().buildIndex);
    	if( PersistentManager.Instance.Collectibles.Contains(SceneManager.GetActiveScene().buildIndex))
    	{
    		this.gameObject.SetActive(false);
    	}

    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if (col.CompareTag("Player"))
    	{
            AudioSource.PlayClipAtPoint(coinsound, this.gameObject.transform.position, 0.4f);
            Collect();
        }
    }

    void Collect()
    {
		PersistentManager.Instance.Collectibles.Add(loreID);
        Pause.Instance.loreList.transform.GetChild(loreID - 1).gameObject.SetActive(true);
		this.gameObject.SetActive(false);
        while(Pause.Instance.activeScreen!=Pause.Instance.lorePage)
        {
            Pause.Instance.ScreenSwitch(1);
        }
        Pause.Instance.ActivatePause();
        Pause.Instance.OpenLore(loreID);
    }
}
