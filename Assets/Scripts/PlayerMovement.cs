using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    public struct MovementSpeed
    {
        public static MovementSpeed Slow = new MovementSpeed(2f);
        public static MovementSpeed Medium = new MovementSpeed(2.5f);
        public static MovementSpeed Fast = new MovementSpeed(3f);
        public static MovementSpeed VeryFast = new MovementSpeed(3.5f);

        public float Speed;

        public MovementSpeed(float speed)
        {
            Speed = speed;
        }

        public static implicit operator float (MovementSpeed speed)
        {
            return speed.Speed;
        }
    }

    public float Speed = MovementSpeed.Slow;
    public float JumpHeight = 1f;
    public float JumpRotationSpeed = 900f;

    private new Rigidbody2D rigidbody;
    private float rotation = 0f;

    public bool Grounded
    {
        get;
        private set;
    }

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        JumpHeight = Mathf.Sqrt(JumpHeight * Physics2D.gravity.y * -1f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var item in collision.contacts)
        {
            if (collision.collider.bounds.min.x > transform.position.x + 0.24f || collision.gameObject.tag == "Destructive")
            {
                this.enabled = false;
                return;
            }
        }

        Grounded = true;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Grounded = false;
    }

    void Update()
    {
        // Jump Animation

        var angles = transform.GetChild(0).eulerAngles;
        
        if (!Grounded)
        {
            angles.z += JumpRotationSpeed * Time.deltaTime;
        }
        else
        {
            var mod = angles.z % 90;
            angles.z += mod > 44 ? mod : -mod;
        }

        transform.GetChild(0).eulerAngles = angles;
    }

    void LateUpdate()
    {
        var vel = rigidbody.velocity;

        if (Input.GetAxis("Jump") == 1 && Grounded)
        {
            vel.y = JumpHeight;
        }

        vel.x = Speed * Time.deltaTime * 100;
        rigidbody.velocity = vel;
    }
}
