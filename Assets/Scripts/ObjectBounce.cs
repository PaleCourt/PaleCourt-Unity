using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class ObjectBounce : MonoBehaviour
{
	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000810 RID: 2064 RVA: 0x0004E9A4 File Offset: 0x0004CBA4
	// (remove) Token: 0x06000811 RID: 2065 RVA: 0x0004E9D8 File Offset: 0x0004CBD8
	public event ObjectBounce.BounceEvent OnBounce;

	// Token: 0x06000812 RID: 2066 RVA: 0x0004EA0C File Offset: 0x0004CC0C
	private void Start()
	{
		rb = base.GetComponent<Rigidbody2D>();
		audio = base.GetComponent<AudioSource>();
		animator = base.GetComponent<tk2dSpriteAnimator>();
		if (sendFSMEvent)
		{
			fsm = base.GetComponent<PlayMakerFSM>();
		}
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x0004EA48 File Offset: 0x0004CC48
	private void FixedUpdate()
	{
		if (bouncing)
		{
			if (stepCounter >= 3)
			{
				Vector2 a = new Vector2(base.transform.position.x, base.transform.position.y);
				velocity = a - lastPos;
				lastPos = a;
				speed = ((!rb) ? 0f : rb.velocity.magnitude);
				stepCounter = 0;
			}
			else
			{
				stepCounter++;
			}
		}
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x0004EAF8 File Offset: 0x0004CCF8
	private void Update()
	{
		if (animTimer > 0f)
		{
			animTimer -= Time.deltaTime;
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0004EB1C File Offset: 0x0004CD1C
	private void OnCollisionEnter2D(Collision2D col)
	{
		if (!rb || rb.isKinematic)
		{
			return;
		}
		if (bouncing && speed > speedThreshold)
		{
			Vector3 inNormal = col.GetSafeContact().Normal;
			Vector3 inDirection = velocity.normalized;
			Vector3 normalized = Vector3.Reflect(inDirection, inNormal).normalized;
			rb.velocity = new Vector2(normalized.x, normalized.y) * (speed * (bounceFactor * UnityEngine.Random.Range(0.8f, 1.2f)));
			if (playSound)
			{
				chooser = UnityEngine.Random.Range(1, 100);
				int num = UnityEngine.Random.Range(0, clips.Length - 1);
				AudioClip clip = clips[num];
				if (chooser <= chanceToPlay)
				{
					float pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
					audio.pitch = pitch;
					audio.PlayOneShot(clip);
				}
			}
			if (playAnimationOnBounce && animTimer <= 0f)
			{
				animator.Play(animationName);
				animator.PlayFromFrame(0);
				animTimer = animPause;
			}
			if (sendFSMEvent && fsm)
			{
				fsm.SendEvent("BOUNCE");
			}
			if (OnBounce != null)
			{
				OnBounce();
			}
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0004ECBC File Offset: 0x0004CEBC
	public void StopBounce()
	{
		bouncing = false;
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x0004ECC8 File Offset: 0x0004CEC8
	public void StartBounce()
	{
		bouncing = true;
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0004ECD4 File Offset: 0x0004CED4
	public void SetBounceFactor(float value)
	{
		bounceFactor = value;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0004ECE0 File Offset: 0x0004CEE0
	public void SetBounceAnimation(bool set)
	{
		playAnimationOnBounce = set;
	}

	// Token: 0x0400063A RID: 1594
	public float bounceFactor;

	// Token: 0x0400063B RID: 1595
	public float speedThreshold = 1f;

	// Token: 0x0400063C RID: 1596
	public bool playSound;

	// Token: 0x0400063D RID: 1597
	public AudioClip[] clips;

	// Token: 0x0400063E RID: 1598
	public int chanceToPlay = 100;

	// Token: 0x0400063F RID: 1599
	public float pitchMin = 1f;

	// Token: 0x04000640 RID: 1600
	public float pitchMax = 1f;

	// Token: 0x04000641 RID: 1601
	public bool playAnimationOnBounce;

	// Token: 0x04000642 RID: 1602
	public string animationName;

	// Token: 0x04000643 RID: 1603
	public float animPause = 0.5f;

	// Token: 0x04000644 RID: 1604
	public bool sendFSMEvent;

	// Token: 0x04000645 RID: 1605
	private float speed;

	// Token: 0x04000646 RID: 1606
	private float animTimer;

	// Token: 0x04000647 RID: 1607
	private tk2dSpriteAnimator animator;

	// Token: 0x04000648 RID: 1608
	private PlayMakerFSM fsm;

	// Token: 0x04000649 RID: 1609
	private Vector2 velocity;

	// Token: 0x0400064A RID: 1610
	private Vector2 lastPos;

	// Token: 0x0400064B RID: 1611
	private Rigidbody2D rb;

	// Token: 0x0400064C RID: 1612
	private AudioSource audio;

	// Token: 0x0400064D RID: 1613
	private int chooser;

	// Token: 0x0400064E RID: 1614
	private bool bouncing = true;

	// Token: 0x0400064F RID: 1615
	private int stepCounter;

	// Token: 0x020001D3 RID: 467
	// (Invoke) Token: 0x0600081B RID: 2075
	public delegate void BounceEvent();
}
