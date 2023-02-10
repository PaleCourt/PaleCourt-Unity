using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;

public class TestRing : MonoBehaviour
{
    public float spd = 2f;

    private bool isPress = false;
    private bool isPress2 = false;

    private IEnumerator RunLoop()
    {
        Random rnd = new Random();
        int[] randSlashes = new [] {0, 1, 2, 3, 4}.OrderBy(x => rnd.Next()).ToArray();  
        
        foreach (var i in randSlashes)
        {
            GameObject ring = transform.Find($"SlashRing{i}").gameObject;
            Animator arc1 = ring.transform.Find("1").GetComponent<Animator>();
            Animator arc2 = ring.transform.Find("2").GetComponent<Animator>();
            StartCoroutine(ActivateRing(ring, arc1, arc2));
            yield return new WaitForSeconds(rnd.Next(15, 30) * 0.01f); //0.15f
        }
        
        yield return new WaitForSeconds(2f);
            
            
        foreach (Transform child in transform)
        {
            foreach (PostProcessVolume ppv in child.GetComponentsInChildren<PostProcessVolume>(true))
            {
                StartCoroutine(LerpBloom(ppv, 0.2f, 40f));
            }
        }
            
        yield return new WaitForSeconds(0.5f);
            
        yield return LerpScale2(transform, 0.2f);
            
        Destroy(gameObject);
        
        IEnumerator ActivateRing(GameObject ring, Animator a1, Animator a2)
        {
            ring.SetActive(true);
        
            a1.enabled = true;
            a1.gameObject.SetActive(true);
            a1.Play("NewSlash3Antic", -1, 0f);
            a1.speed = spd;
            a2.enabled = false;
        
            yield return new WaitWhile(() => GetCurrentFrame(a1) < 4);

            StartCoroutine(LoopAnimation(a1, "NewSlash3Loop"));
            a2.enabled = true;
            a2.speed = spd;
            a2.gameObject.SetActive(true);
            a2.Play("NewSlash3Antic", -1, 0f);

            yield return new WaitWhile(() => GetCurrentFrame(a2) < 4);

            StartCoroutine(LoopAnimation(a2, "NewSlash3Loop"));

            IEnumerator LoopAnimation(Animator anim, string name)
            {
                while (anim != null)
                {
                    anim.Play(name, -1, 0f);
                    anim.speed = spd;
                    yield return null;
                    yield return new WaitWhile(() => GetCurrentFrame(anim) < 5);
                }
            }
        }
    }
    
    IEnumerator LerpBloom(PostProcessVolume ppv, float lerpDuration, float endValue)
    {
        float startValue = 0f;
        float timeElapsed = 0;
        Bloom bloom = ppv.profile.GetSetting<Bloom>();
        while (timeElapsed < lerpDuration)
        {
            bloom.intensity.value = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        bloom.intensity.value = endValue;
    }
    
    IEnumerator LerpScale2(Transform trans, float lerpDuration)
    {
        Vector2 startValue = trans.localScale;
        Vector2 endValue = trans.localScale * 0.1f;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            trans.localScale = Vector2.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        trans.localScale = endValue;
    }
    
    /*private void ActivateSpiral(GameObject spiral, float spd)
    {
        spiral.SetActive(true);
        var animOrig = spiral.GetComponent<Animator>();
        animOrig.speed = spd;
        animOrig.Rebind();
        animOrig.Update(0f);
        foreach (var anim in spiral.GetComponentsInChildren<Animator>(true))
        {
            anim.Rebind();
            anim.Update(0f);
            anim.speed = spd;
        }
    }
    IEnumerator RunNoLoop()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject spiral = transform.Find($"SlashRing{i}").gameObject;
            ActivateSpiral(spiral, spd);
        }
        
        // Wait for first set to do non-hitbox part of animation
        Animator oldAnim = transform.Find("SlashRing0").Find("1").gameObject.GetComponent<Animator>();
        yield return new WaitWhile(() => GetCurrentFrame(oldAnim) < 7);
        
        for (int i = 3; i < 8; i++)
        {
            GameObject spiral = transform.Find($"SlashRing{i}").gameObject;
            ActivateSpiral(spiral, spd * 2);
        }

        yield return new WaitForSeconds(3f);
        foreach (Transform t in transform)
        {
            GameObject go = t.gameObject;
            go.GetComponent<Animator>().Rebind();
            go.GetComponent<Animator>().Update(0f);
            foreach (var anim in go.GetComponentsInChildren<Animator>(true))
            {
                anim.Rebind();
                anim.Update(0f);
            }
            t.gameObject.SetActive(false);
        }
        isPress = false;
    }
    */
    
    public int GetCurrentFrame(Animator anim)
    {
        AnimatorClipInfo att = anim.GetCurrentAnimatorClipInfo(0)[0];
        int currentFrame = (int)(anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f * (att.clip.length * att.clip.frameRate));
        return currentFrame;
    }
    
    public static IEnumerator PlayToEnd(Animator self)
    {
        yield return null;
        while (self.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            yield return null;
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && !isPress)
        {
            Debug.Log("TEST");
            isPress = true;
            StartCoroutine(RunLoop());
        }
    }
}
