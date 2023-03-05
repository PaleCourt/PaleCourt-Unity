using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using UnityEngine.Rendering;

public class TestRing : MonoBehaviour
{
    public float spd = 2f;

    private bool isPress = false;
    private static readonly string[] outer = { "SlashRing0", "SlashRing1", "SlashRing2", "SlashRing3"};

    private IEnumerator RunLoop()
    {
        var slash = gameObject;
        slash.SetActive(true);
        float spd = 2f; // 2f
        StartCoroutine(LerpScale(slash.transform, 2.5f));

        for (int i = 0; i < 3; i++)
        {
            GameObject spiral = slash.transform.Find($"SlashRing{i}").gameObject;
            ActivateSpiral(spiral, spd);
        }

        // Wait for first set to do non-hitbox part of animation
        Animator oldAnim = slash.transform.Find("SlashRing0").Find("1").gameObject.GetComponent<Animator>();
        yield return new WaitWhile(() => oldAnim.GetCurrentFrame() < 7);


        for (int i = 3; i < 5; i++)
        {
            GameObject spiral = slash.transform.Find($"SlashRing{i}").gameObject;
            ActivateSpiral(spiral, spd * 1.8f);
        }

        System.Random rnd = new System.Random();
        int[] randSlashes = new [] {5, 6, 7}.OrderBy(x => rnd.Next()).ToArray();
        GameObject lastSpiral = null;
        
        foreach (int i in randSlashes)
        {
            GameObject spiral = slash.transform.Find($"SlashRing{i}").gameObject;
            ActivateSpiral(spiral, spd * 2f);
            lastSpiral = spiral;
            yield return new WaitForSeconds(rnd.Next(5, 10) * 0.01f);
        }

        oldAnim = lastSpiral.transform.Find("1").gameObject.GetComponent<Animator>();
        yield return new WaitWhile(() => oldAnim.GetCurrentFrame() < 8);
        

        yield return new WaitWhile(() => oldAnim.GetCurrentFrame() < 10);
        yield return LerpScale2(slash.transform);
        StartCoroutine(LerpOpacity(slash.transform));

        // Terrible way to check if animation is over
        foreach (Transform t in slash.transform)
        {
            foreach (var anim in t.GetComponentsInChildren<Animator>(true))
            {
                yield return anim.PlayToEnd();
            }
        }


        isPress = false;
        Destroy(slash);

        IEnumerator LerpScale(Transform trans, float scale)
        {
            float lerpDuration = (5f / 12f) / 1.8f;
            Vector2 startValue = trans.localScale;
            Vector2 endValue = trans.localScale * scale;
            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                trans.localScale = Vector2.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            trans.localScale = endValue;
        }
        
        IEnumerator LerpScale2(Transform trans)
        {
            float lerpDuration = 0.1f;
            Vector2 startValue = trans.localScale;
            Vector2 endValue = trans.localScale * 0.7f;
            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                trans.localScale = Vector2.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            trans.localScale = endValue;
        }
        
        IEnumerator LerpOpacity(Transform trans)
        {
            float lerpDuration = 0.1f;
            float startValue = 1f;
            float endValue = 0f;
            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                float a = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                foreach (Transform t in trans)
                {
                    foreach (var sr in t.GetComponentsInChildren<SpriteRenderer>(true))
                    {
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
                    }
                }
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            foreach (SpriteRenderer sr in trans.GetComponentsInChildren<SpriteRenderer>(true))
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            }
        }
        
        void ActivateSpiral(GameObject spiral, float spd)
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
    }

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
