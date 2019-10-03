using UnityEngine;
using UnityEngine.Events;

public class BrackeysMovement : MonoBehaviour
{

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    [HideInInspector] public bool m_FacingRight = true;  // For determining which way the player is currently facing.
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public Vector3 predictMove(Vector3 move)
    {
        return Vector3.SmoothDamp(m_Rigidbody2D.velocity, move * 10f, ref m_Velocity, m_MovementSmoothing) * Time.fixedDeltaTime;
    }

    public void Move(float move)
    {
        //only control the player if grounded or airControl is turned on

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    public void VerticalMove(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, move * 10f);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }


    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
