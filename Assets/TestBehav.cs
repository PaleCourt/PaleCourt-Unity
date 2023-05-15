using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cringe
{
    public static void SetRotation2D(this Transform t, float rotation)
    {
        Vector3 eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, rotation);
        t.eulerAngles = eulerAngles;
    }
}

public class TestBehav : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _target;
    private Animator _anim;
    IEnumerator Start()
    {
        _anim = GetComponent<Animator>();
        _target = GameObject.Find("player");

        while (true)
        {
            yield return new WaitWhile(() => !Input.GetKey(KeyCode.R));
            yield return Throw();
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private int FaceHero(bool onlyCalc = false, bool opposite = false, float? tarX = null)
    {
        tarX ??= _target.transform.position.x;
        int sign = (int) Mathf.Sign(gameObject.transform.position.x - tarX.Value);
        sign = opposite ? -sign : sign;
        if (onlyCalc)
            return sign;

        Vector3 pScale = gameObject.transform.localScale;

        gameObject.transform.localScale = new Vector3(Mathf.Abs(pScale.x) * sign, pScale.y, 1f);

        return sign;
    }

    private float ThrowDelay = 0.2f;
    
    private float GetAngleTo(Vector2 from, Vector2 to)
    {
        float num = Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg;
        while ((double) num < 0.0) num += 360f;
        return num;
    }

    private void Log(object o)
    {
        Debug.Log(o);
    }

    IEnumerator Throw()
    {
        Vector2 hero = _target.transform.position;
        Vector2 zem = gameObject.transform.position;

        float dir = FaceHero();
        float rot;
        _anim.enabled = true;
        _anim.Play("ZThrow1", -1, 0f);
        yield return _anim.WaitToFrame(2);
        _anim.enabled = false;
        yield return new WaitForSeconds(ThrowDelay / 2f);
        hero = _target.transform.position;
        zem = gameObject.transform.position;
        dir = FaceHero();
        yield return new WaitForSeconds(ThrowDelay / 2f);
        _anim.enabled = true;
        yield return _anim.WaitToFrame(3);

        rot = GetAngleTo(zem, hero) * Mathf.Deg2Rad;
        //rot = Mathf.Atan((hero.y - zem.y) / (hero.x - zem.x));
        //float rotVel = dir > 0 ? rot + 180 * Mathf.Deg2Rad : rot;
        //rot += Mathf.PI / 4f;
        float rotVel = rot; //+ (dir > 0 ? Mathf.PI : 0f);
        Log($"Rot is {rot * Mathf.Rad2Deg} and rot vel is {rotVel * Mathf.Rad2Deg}");

        GameObject arm = transform.Find("NailHand").gameObject;
        GameObject nailPar = Instantiate(transform.Find("ZNailB").gameObject);
        Rigidbody2D parRB = nailPar.GetComponent<Rigidbody2D>();
        arm.transform.SetRotation2D(rot * Mathf.Rad2Deg + (dir > 0 ? 180f : 0f));
        nailPar.transform.SetRotation2D(rot * Mathf.Rad2Deg + (dir > 0 ? 180f : 0f));
        nailPar.transform.position = transform.Find("ZNailB").position;
        nailPar.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x) * 1.5f, 1.5f, 1.5f);
        yield return new WaitWhile(() => _anim.GetCurrentFrame() < 4);
        yield return new WaitForSeconds(0.07f);
        nailPar.SetActive(true);
        // TODO might want to readjust speed
        float velmag = 70f;
        parRB.velocity = new Vector2(Mathf.Cos(rotVel) * velmag, Mathf.Sin(rotVel) * velmag);
        yield return new WaitForSeconds(0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
