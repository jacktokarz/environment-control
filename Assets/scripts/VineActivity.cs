using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineActivity : MonoBehaviour
{

	public int activeChildCount;
	Vector2 vineSize;

    public AudioClip vineGrowSound;
    //public AudioClip vineShrinksound;
    private AudioSource source;
    private Animator baseAnim;
    void Start()
    {
        vineSize = new Vector2(PersistentManager.Instance.vinePieceWidth, PersistentManager.Instance.vinePieceHeight);
        source = GetComponent<AudioSource>();
        source.clip = vineGrowSound;
        baseAnim = this.transform.GetChild(0).GetComponent(typeof (Animator)) as Animator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeVine() {
        activeChildCount = 0;
        for(int i = 0; i < this.transform.childCount; i++)
        {
        	Transform piece = this.transform.GetChild(i);
        	if(piece.gameObject.activeSelf) {activeChildCount++;}
        }
        Transform topVinePiece = this.transform.GetChild(activeChildCount-1);
        if (PersistentManager.Instance.humidityLevel > 0)
        {
            if (activeChildCount < PersistentManager.Instance.vineMaxHeight && PersistentManager.Instance.tempLevel < 2)
            {
                growVine(topVinePiece, activeChildCount);
                // !! Justin put sound triggers here...
                if (PersistentManager.Instance.humidityLevel == 1) { source.pitch = 1.0F; }
                if (PersistentManager.Instance.humidityLevel == 2) { source.pitch = 1.2F; }
                if (PersistentManager.Instance.humidityLevel == 3) { source.pitch = 1.4F; }
                if (source.isPlaying != true) { source.Play(); }
                //source.enabled = true;
                //source.loop = true;
            }
            else { source.Stop(); }
            
        }
        if (PersistentManager.Instance.humidityLevel < 0 || PersistentManager.Instance.tempLevel == 2)
        {
            if (activeChildCount > PersistentManager.Instance.vineMinHeight)
            {
                shrinkVine(topVinePiece);
                // and HERE :)

                if (PersistentManager.Instance.humidityLevel == -1 ) {source.pitch = 0.75F;}
                if (PersistentManager.Instance.humidityLevel == -2) { source.pitch = 0.55F; }
                if (PersistentManager.Instance.humidityLevel == -3) { source.pitch = 0.35F; }
                if (source.isPlaying != true) { source.Play(); }
                
            }
            else { source.Stop(); }

        }
        else if (PersistentManager.Instance.humidityLevel == 0) { source.Stop(); }
    }

    void growVine(Transform topVinePiece, int activeChildCount) {
        if (activeChildCount == PersistentManager.Instance.vineMinHeight) {
            baseAnim.SetBool("open", true);
        }

        Vector2 newPos = new Vector2();
        float zRot = this.transform.rotation.eulerAngles.z;
        if (zRot == 90) {
            newPos= new Vector2(topVinePiece.position.x - PersistentManager.Instance.vinePieceHeight, topVinePiece.position.y);
        }
        else if (zRot == 270) {
            newPos= new Vector2(topVinePiece.position.x + PersistentManager.Instance.vinePieceHeight, topVinePiece.position.y);
        }
        else if (zRot == 180) {
            newPos= new Vector2(topVinePiece.position.x, topVinePiece.position.y - PersistentManager.Instance.vinePieceHeight);
        }
        else {
            newPos= new Vector2(topVinePiece.position.x, topVinePiece.position.y + PersistentManager.Instance.vinePieceHeight);
        }
        bool blocked = false;
        Collider2D[] overlapper= Physics2D.OverlapBoxAll(newPos, vineSize, this.transform.rotation.eulerAngles.z, PersistentManager.Instance.whatBlocksVines);
        if(overlapper.Length > 0)
        {
        	foreach (Collider2D overlap in overlapper)
        	{
	            if (overlap.CompareTag("wind"))
	            {
	                WindDirection wd = overlap.gameObject.GetComponent(typeof(WindDirection)) as WindDirection;
	                if(wd.direction != new Vector2(0,0))
	                {
	                    newPos = newPos + (PersistentManager.Instance.windLevel * PersistentManager.Instance.vineWindAffect * wd.direction);
	                }
	            }
	            else
	            {
                    if(!overlap.CompareTag("vine") && (!overlap.CompareTag("vinePiece"))) {
    	            	blocked = true;
    	            	break;
                    }
	            }
        	}
        }
        if (blocked) {return;}
        Transform newPiece = this.transform.GetChild(activeChildCount);
        newPiece.position = newPos;
        newPiece.gameObject.SetActive(true);
    }
    void shrinkVine(Transform topVinePiece) {
        if (activeChildCount == PersistentManager.Instance.vineMinHeight + 1) {
            baseAnim.SetBool("open", false);
        }
        topVinePiece.gameObject.SetActive(false);
    }
}
