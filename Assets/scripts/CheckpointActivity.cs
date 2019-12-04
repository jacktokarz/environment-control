using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointActivity : MonoBehaviour
{
    private AudioSource source;
    private Animator spawnAnim;
	private Animator flowerGrowAnim;
	public bool alreadyChecked;
    public AudioClip flowerGrowSound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        spawnAnim = this.transform.GetChild(0).GetComponent(typeof (Animator)) as Animator;
        spawnAnim.SetBool("active", false);
        flowerGrowAnim = this.transform.GetChild(1).GetComponent(typeof (Animator)) as Animator;
    }

    void Start()
    {
        Debug.Log("this scene is: "+SceneManager.GetActiveScene().buildIndex);
    	if( PersistentManager.Instance.Checkpoints.Contains(SceneManager.GetActiveScene().buildIndex))
    	{
    		alreadyChecked = true;
    		flowerGrowAnim.SetBool("grown", true);
            if (PersistentManager.Instance.lastCheckpoint == SceneManager.GetActiveScene().buildIndex)
            {
                spawnAnim.SetBool("active", true);
            }
    	}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.CompareTag("Player"))
    	{
            if (!alreadyChecked) {
        		activateCheckpoint();
            }
            else {
                setCheckpoint();
            }
    	}
    }

    void activateCheckpoint()
    {
		flowerGrowAnim.SetBool("grow", true);
        source.PlayOneShot(flowerGrowSound);
		PersistentManager.Instance.Checkpoints.Add(SceneManager.GetActiveScene().buildIndex);
		setCheckpoint();
		alreadyChecked = true;
    }

    void setCheckpoint() {
        PersistentManager.Instance.lastCheckpoint = SceneManager.GetActiveScene().buildIndex;
        bool saved = PersistentManager.Instance.Save();
        spawnAnim.SetBool("active", true);
    }

    public void spawn(GameObject player)
    {
        Debug.Log("checkpoint makes player");
        spawnAnim.SetBool("active", true);
        spawnAnim.SetTrigger("Spawn");
        StartCoroutine(stallPlayerActivation(player, 4.5f));
    }

    IEnumerator stallPlayerActivation(GameObject obj, float sec)
    {
        PersistentManager.Instance.immobile = true;
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(sec);
        obj.SetActive(true);
        PersistentManager.Instance.immobile = false;
    }

}
