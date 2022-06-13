using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogVelocity : MonoBehaviour
{
    Vector2 lastvelocity = Vector2.zero;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != lastvelocity)
        {
            lastvelocity = rb.velocity;
            Debug.Log("New velocity   x: " + rb.velocity.x + "  y: " + rb.velocity.y);
        }
    }
}
