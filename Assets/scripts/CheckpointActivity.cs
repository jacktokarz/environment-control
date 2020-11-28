using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointActivity : MonoBehaviour
{
    private AudioSource source;
    private GameObject petalObj;
	private Animator flowerGrowAnim;
    private Animator petalAnim;
    private Animator playerAnim;
	public bool alreadyChecked;
    public AudioClip flowerGrowSound;
    public AudioClip spawnsound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        flowerGrowAnim = this.transform.GetChild(0).GetComponent(typeof (Animator)) as Animator;
        petalObj = this.transform.GetChild(1).gameObject;
        petalAnim = petalObj.GetComponent(typeof (Animator)) as Animator;
        playerAnim = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent(typeof (Animator)) as Animator;
    }

    void Start()
    {
        Debug.Log("this scene is: "+SceneManager.GetActiveScene().buildIndex);
    	if( PersistentManager.Instance.Checkpoints.Contains(SceneManager.GetActiveScene().buildIndex))
    	{
    		alreadyChecked = true;
    		flowerGrowAnim.SetBool("grown", true);
    	}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.CompareTag("Player"))
    	{
            Debug.Log("checkpoint hit player");
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
        playerAnim.SetBool("activating", true);
        PersistentManager.Instance.immobile = true;
        source.volume = 0.9f;
        source.PlayOneShot(flowerGrowSound);
		PersistentManager.Instance.Checkpoints.Add(SceneManager.GetActiveScene().buildIndex);
		setCheckpoint();
		alreadyChecked = true;
    }

    void setCheckpoint() {
        PersistentManager.Instance.lastCheckpoint = SceneManager.GetActiveScene().buildIndex;
        bool saved = PersistentManager.Instance.Save();
    }

    public void spawn()
    {
        Debug.Log("checkpoint makes player");
        petalObj.SetActive(true);
        petalAnim.SetBool("active", true);
        petalAnim.SetTrigger("spawn");
        flowerGrowAnim.SetTrigger("spawn");
        source.volume = 0.7f;
        source.PlayOneShot(spawnsound);
    }
}
