using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private TrajectoryLine lineTrajectory;

    [SerializeField]
    private float launchSpeed;
    [SerializeField]
    private Vector2 collisionDamping;

    private Vector2 direction;
    private Vector2 speed;
    private Vector2 lastVelocity;

    private bool isLaunched;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineTrajectory = GetComponent<TrajectoryLine>();
    }

    // Update is called once per frame
    void Update()
    {
        LaunchBall();
    }
    private void FixedUpdate()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = pos - transform.position;
        lastVelocity = rb.velocity;
        if (!isLaunched)
        {
            lineTrajectory.UpdateLine(distance.normalized * launchSpeed);
        }
    }
    private void LaunchBall()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = pos - transform.position;

        
        if (Input.GetButtonDown("Fire1"))
        {
            if (isLaunched) { return; }

            
            rb.velocity = distance.normalized * launchSpeed;
            isLaunched = true;
            GameManager.instance.hasGameStarted = true;
           // trajectory.HideTrejectory();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Vector2 velocity;
            velocity.x = other.collider.attachedRigidbody.velocity.x + (lastVelocity.x * collisionDamping.x);
            velocity.y = -lastVelocity.y;

            rb.velocity = velocity;
        }
        else
        {
            direction = Vector2.Reflect(lastVelocity.normalized, other.GetContact(0).normal);
            speed = lastVelocity.magnitude * direction ;

            rb.velocity = speed;
        }
    }
}
