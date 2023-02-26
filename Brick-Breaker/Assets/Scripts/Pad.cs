using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float boundX = 50f;

    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxis("Horizontal");

        RestrictToBound();
    }
    private void FixedUpdate()
    {
        if(GameManager.instance.hasGameStarted)
            Move();
    }
    private void Move()
    {
        var velocityX = moveSpeed * direction * Time.deltaTime;
        rb.velocity = new Vector2(velocityX, 0f);
    }
    private void RestrictToBound()
    {
        var pos = transform.position;

        if(pos.x > boundX)
        {
            pos.x = boundX;
        }
        else if(pos.x < -boundX)
        {
            pos.x = -boundX;
        }
        transform.position = pos;
    }
}
