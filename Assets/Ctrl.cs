using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour
{
    private const float GroundY = -8f;
    private const float LeftX = -12f;
    private const float RightX = 35f;
    private readonly float _middleX = (RightX + LeftX) / 2;
    private Rigidbody2D _rb;
    private GameObject _target;

    IEnumerator Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.Find("player");

        while (true)
        {
            yield return null;
            if (!Input.GetKey(KeyCode.R)) continue;

            StartCoroutine(Jump());

            yield return new WaitForSeconds(1f);
        }
    }

    public Vector2 p2;
    public Vector2 p3;
    public int side;

    private IEnumerator Jump()
    {
        TpToEdge(side);
        
        yield return new WaitForSeconds(0.5f);

        Vector2 tarPos = _target.transform.position;
        Vector2 p1 = transform.position;

        float timePass = 0f;
        float duration = 1f;
        while (timePass < duration)
        {
            transform.position = QuadraticBezierInterp(p1, p2, p3, timePass / duration);
            timePass += Time.deltaTime;
            yield return null;
        }
        transform.position = p3;

        yield return new WaitForSeconds(0.4f);
        
        Vector2 diff = p3 - tarPos;
        float rot = Mathf.Atan(diff.y / diff.x);
        if (side > 0 && p3.x > tarPos.x) rot += Mathf.PI;
        Debug.Log($"Rot is {rot * Mathf.Rad2Deg}");
        _rb.velocity = new Vector2(60f * Mathf.Cos(rot), 60f * Mathf.Sin(rot));

        yield return WaitByVelY();

        _rb.velocity = Vector2.zero;
    }

    

    private Vector2 QuadraticBezierInterp(Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        Vector2 a = LineLerp(p1, p2, t);
        Vector2 b = LineLerp(p2, p3, t);

        return LineLerp(a, b, t);
        
        Vector2 LineLerp(Vector2 p1, Vector2 p2, float t)
        {
            float x = Mathf.SmoothStep(p1.x, p2.x, t);
            float y = Mathf.SmoothStep(p1.y, p2.y, t);

            return new Vector2(x, y);
        }
    }
    
    private void TpToEdge(int side)
    {
        transform.position = side == -1 ? new Vector3(LeftX, GroundY) : new Vector3(RightX, GroundY);
    }

    private IEnumerator WaitByVelX()
    {
        yield return new WaitWhile(() => _rb.velocity.x != 0f);
    }
    
    private IEnumerator WaitByVelY()
    {
        yield return new WaitWhile(() => _rb.velocity.y != 0f);
    }

    void Update()
    {
        Vector2 pos = transform.position;
        
        if (_rb.velocity == Vector2.zero) return;
        
        if (pos.x < LeftX)
        {
            transform.position = new Vector2(LeftX, pos.y);
            _rb.velocity *= Vector2.up;
        }

        if (pos.x > RightX)
        {
            transform.position = new Vector2(RightX, pos.y);
            _rb.velocity *= Vector2.up;
        }

        if (pos.y < GroundY)
        {
            transform.position = new Vector3(pos.x, GroundY);
            _rb.velocity = Vector2.right;
        }
    }
}
