using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {

    public GameObject player;
    //public float diff;

    void Start()
    {

    }

    void LateUpdate()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        /*
        diff = Mathf.Abs(player.transform.position.x - this.transform.position.x);
        if (diff > screenWidth)
        {
            diff = player.GetComponent<Rigidbody2D>().velocity.x;
            if (Mathf.Abs(diff) > 1)
                GetComponent<Rigidbody2D>().velocity = new Vector2(diff, 0);
            else
            {
                if (diff > 0)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
                else if (diff < 0)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0);
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    */
    }
}
