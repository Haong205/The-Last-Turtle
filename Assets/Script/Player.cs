using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float XSpeed;
    public float XaccelStrength;
    private float xMovement;
    public Rigidbody2D rb;
    private Vector2 BottomLeftCorner;
    public Animator animator;
    bool isColliding = false;
    bool startup = true;
    public float dragSpeed;
    public float boostSpeed;
    public float dragAccel;
    public float boostAccel;
    public float velPower;
    float speedDif = 0;
    float xSpeedDif = 0;
    private AudioSource BoostAudio;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        BottomLeftCorner = Camera.main.ScreenToWorldPoint(Vector2.zero);
        BottomLeftCorner += new Vector2(0.5f, 0.5f);
    }

    private void Start()
    {
        BoostAudio = GetComponent<AudioSource>();
        animator.Play("Turtle_Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Turtle_Boost"))
        {
            //rb.velocity = Vector2.up * JumpVelocity;
            animator.Play("Turtle_Boost");
            BoostAudio.Play();
        }

        
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, BottomLeftCorner.x, -BottomLeftCorner.x), Mathf.Clamp(transform.position.y, BottomLeftCorner.y, -BottomLeftCorner.y));
        if (Mathf.Abs(transform.position.x) == Mathf.Abs(BottomLeftCorner.x))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        //Message.MessageSent3?.Invoke("Initialz: " + InitialDeviceZ.ToString() + " movement: " + movement.ToString());
        //Message.MessageSent4?.Invoke(Input.acceleration.x.ToString());
    }

    private void FixedUpdate()
    {

        Vector2 force;
        xMovement = Input.acceleration.x;
        xSpeedDif = XSpeed*Mathf.Sign(xMovement) - rb.velocity.x;
        force.x = Mathf.Pow(Mathf.Abs(xSpeedDif * xMovement * XaccelStrength) , velPower) * Mathf.Sign(xMovement);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Turtle_Boost"))
        {
            speedDif = dragSpeed - rb.velocity.y;
            force.y = Mathf.Pow(Mathf.Abs(speedDif) * dragAccel, velPower) * Mathf.Sign(speedDif);
        }
        else
        {

            speedDif = boostSpeed - rb.velocity.y;
            force.y = Mathf.Pow(Mathf.Abs(speedDif) * boostAccel, velPower) * Mathf.Sign(speedDif);
        }
        rb.AddForce(force);

        //speedDif = dragSpeed - rb.velocity.y;
        //yMovement = Mathf.Pow(Mathf.Abs(speedDif) * dragAccel, velPower) * Mathf.Sign(speedDif);
        //rb.AddForce(yMovement * Vector2.up);


        //Message.MessageSent3?.Invoke("yovement: " + yMovement);



    }

    private void OnEnable()
    {
        //rb.velocity = new Vector2(0, 1);
        if(!startup)
            animator.Play("Turtle_Boost");
        Debug.Log("player enabled");
        animator.speed = 1;
    }

    private void OnDisable()
    {
        startup = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<GameManager>().GameOver();
            Debug.Log("obstacle collided");
        }
        else if (collision.gameObject.tag == "Scoring")
        {
            Destroy(collision.gameObject);
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }

}
