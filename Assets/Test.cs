using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 force = new Vector2(-1f, 0f);
    private Vector3 startPos;
    private bool isready;
    private float time; 
    public float type = 1f;
    public float delay = 0f;
    
    // Start is called before the first frame updat

    IEnumerator Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        GetComponent<Animator>().speed = 2f;
        
        while (true)
        {
            yield return new WaitWhile((() => !Input.GetKey(KeyCode.R)));
            yield return new WaitForSeconds(delay);
            isready = true;
            _rb.velocity = new Vector2(50, 0f);
            transform.position = startPos;
            time = 0f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x < startPos.x && transform.position.y.Within(startPos.y, 1f))
        {
            isready = false;
            _rb.velocity = Vector2.zero;
            transform.position = startPos;
        }

        if (isready)
        {
            transform.position = new Vector3(transform.position.x, startPos.y + type * Mathf.Sin(time * 8f) * 2f);
            _rb.AddForce(force, ForceMode2D.Impulse);
            time += Time.fixedDeltaTime;
        }
    }
}
