using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CollectibleActivity : MonoBehaviour
{
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
            collect();
        }
    }

    void collect()
    {
        
		PersistentManager.Instance.Collectibles.Add(SceneManager.GetActiveScene().buildIndex);
		this.gameObject.SetActive(false);
    }
}
