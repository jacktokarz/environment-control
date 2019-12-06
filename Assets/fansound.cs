using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fansound : MonoBehaviour
{
    private AudioSource source;
    public AudioClip fanwhir;
    // Start is called before the first frame update
    void Awake()
    {
        
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void playsound()
    {
        
        source.pitch = UnityEngine.Random.Range(0.15F, 3.25F);
        source.Play();
        

    }



    
}
