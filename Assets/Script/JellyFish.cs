using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
    public float dragSpeed;
    public float boostSpeed;
    public float dragAccel;
    public float boostAccel;
    public float velPower;
    float speedDif = 0;
    public float AnimationTime;
    private float timer;

    public Rigidbody2D rb;
    private float BottomEdge;
    Animator animator;



    private void Start()
    {
        BottomEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).y - 1f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.Play("JellyFish_Idle");
    }

    private void Update()
    {
        if (transform.position.y < BottomEdge)
        {
            Destroy(gameObject);
        }

        if (timer > Random.Range(AnimationTime * 0.8f, AnimationTime * 1.2f))
        {
            animator.Play("JellyFish_Boost");
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    private void OnDisable()
    {
        animator.speed = -1;
        rb.velocity = new Vector2(0, 0);
    }

    private void OnEnable()
    {
        animator.speed = 1;
    }

    private void FixedUpdate()
    {
        float yMovement;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JellyFish_Boost"))
        {
            speedDif = dragSpeed - rb.velocity.y;
            yMovement = Mathf.Pow(Mathf.Abs(speedDif) * dragAccel, velPower) * Mathf.Sign(speedDif);
        }
        else
        {
            speedDif = boostSpeed - rb.velocity.y;
            yMovement = Mathf.Pow(Mathf.Abs(speedDif) * boostAccel, velPower) * Mathf.Sign(speedDif);
        }

        rb.AddForce(yMovement * Vector2.up);

        Message.MessageSent3?.Invoke("yovement: " + yMovement);



    }
}
