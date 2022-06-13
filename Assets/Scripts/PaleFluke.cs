using System;
using System.Collections;
using UnityEngine;

namespace FiveKnights
{
	public class PaleFluke : MonoBehaviour
	{
		private void Awake()
		{
			animator = base.GetComponent<tk2dSpriteAnimator>();
			meshRenderer = base.GetComponent<MeshRenderer>();
			body = base.GetComponent<Rigidbody2D>();
			spriteFlash = base.GetComponent<SpriteFlash>();
			objectBounce = base.GetComponent<ObjectBounce>();
			audioPlayer = GetComponent<AudioSource>();
		}

		private void Start()
		{
			/*this.damager.OnTriggerEntered += delegate (Collider2D collider, GameObject sender)
			{
				this.DoDamage(collider.gameObject, 2, true);
			};*/
			if (objectBounce)
			{
				objectBounce.OnBounce += delegate ()
				{
					if (hasBursted)
					{
						return;
					}
					hasBounced = true;
					if (body)
					{
						Vector2 velocity = body.velocity;
						velocity.x = UnityEngine.Random.Range(-5f, 5f);
						velocity.y = Mathf.Clamp(velocity.y, UnityEngine.Random.Range(7.3f, 15f), UnityEngine.Random.Range(20f, 25f));
						body.velocity = velocity;
					}
					if (animator)
					{
						animator.Play(Anim);
					}
					var angles = transform.rotation.eulerAngles;
					transform.rotation = Quaternion.Euler(angles.x, angles.y, 0);
				};
			}
			if (body)
            {
				Vector3 forward1 = transform.InverseTransformVector(Vector3.forward);
				float angle = Vector3.Angle(transform.position, new Vector3(forward1.x, UnityEngine.Random.Range(-0.5f, 1.6f)) + transform.position);
				Debug.Log(angle);
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
				Debug.Log(transform.rotation.eulerAngles.z);
				Vector3 forward2 = transform.InverseTransformVector(Vector3.forward * 2);
				body.velocity = forward2;
			}
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
			/*if (collision.GetComponent<HealthManager>())
				DoDamage(collision.gameObject);*/
        }

        private void DoDamage(GameObject obj, int upwardRecursionAmount = 2, bool burst = true)
		{
			/*HealthManager component = obj.GetComponent<HealthManager>();
			if (component)
			{
				if (component.IsInvincible && obj.tag != "Spell Vulnerable")
				{
					return;
				}
				if (!component.isDead)
				{
					component.hp -= this.damage;
					if (component.hp <= 0)
					{
						component.Die(new float?(0f), AttackTypes.Generic, false);
					}
				}
			}*/
			SpriteFlash component2 = obj.GetComponent<SpriteFlash>();
			if (component2)
			{
				component2.FlashShadowRecharge();
			}
			FSMUtility.SendEventToGameObject(obj.gameObject, "TOOK DAMAGE", false);
			upwardRecursionAmount--;
			if (upwardRecursionAmount > 0 && obj.transform.parent)
			{
				DoDamage(obj.transform.parent.gameObject, upwardRecursionAmount, false);
			}
			if (burst)
			{
				Burst();
			}
		}

		private void OnEnable()
		{
			if (animator)
			{
				animator.Play(Anim);
			}
			lifeEndTime = Time.time + UnityEngine.Random.Range(averageLifeTime - 0.5f, averageLifeTime + 0.5f);
			if (meshRenderer)
			{
				meshRenderer.enabled = true;
			}
			if (body)
			{
				body.isKinematic = false;
				Vector3 forward1 = transform.InverseTransformVector(Vector3.forward);
				float angle = Vector3.Angle(transform.position, new Vector3(forward1.x, UnityEngine.Random.Range(-0.5f, 1.6f)) + transform.position);
				transform.rotation = Quaternion.Euler(0, 0, angle);
				Vector3 forward2 = transform.InverseTransformVector(Vector3.forward * 2);
				body.velocity = forward2;
			}
			/*if (GameManager.instance.playerData.GetBool("equippedCharm_19"))
			{
				float num = UnityEngine.Random.Range(0.9f, 1.2f);
				base.transform.localScale = new Vector3(num, num, 0f);
				this.damage = 5;
			}
			else
			{
				float num2 = UnityEngine.Random.Range(0.7f, 0.9f);
				base.transform.localScale = new Vector3(num2, num2, 0f);
				this.damage = 4;
			}*/
			float num2 = UnityEngine.Random.Range(0.7f, 0.9f);
			base.transform.localScale = new Vector3(num2, num2, 0f);
			baseDamage = 4;
			if (spriteFlash)
			{
				spriteFlash.flashArmoured();
			}
			hasBounced = false;
			hasBursted = false;
		}

		private void Update()
		{
			if (hasBursted)
			{
				return;
			}
			if (!hasBounced)
			{
				Vector2 velocity = body.velocity;
				float z = Mathf.Atan2(velocity.y, velocity.x) * 57.2957764f;
				base.transform.localEulerAngles = new Vector3(0f, 0f, z);
			}
			if (Time.time >= lifeEndTime)
			{
				Burst();
			}
		}

		private void Burst()
		{
			if (!hasBursted)
			{
				base.StartCoroutine(BurstSequence());
			}
			hasBursted = true;
		}

		private IEnumerator BurstSequence()
		{
			if (meshRenderer)
			{
				meshRenderer.enabled = false;
			}
			if (body)
			{
				body.velocity = Vector2.zero;
				body.angularVelocity = 0f;
				body.isKinematic = true;
			}
			if (splatEffect)
			{
				splatEffect.SetActive(true);
				if (splatEffect.GetComponent<FlukeParticleDamageEnemies>())
					splatEffect.GetComponent<FlukeParticleDamageEnemies>().damage = baseDamage;
			}
			if (audioPlayer)
			{
				audioPlayer.PlayOneShot(splatSounds[UnityEngine.Random.Range(0, splatSounds.Length)]);
			}
			yield return new WaitForSeconds(0.8f);
			if (splatEffect && splatEffect.transform.parent == transform)
            {
				splatEffect.transform.parent = null;
			}
			yield return new WaitForSeconds(0.2f);
			gameObject.Recycle();
			yield break;
		}

		//public string airAnim = "Air";

		//public string flopAnim = "Flop";

		public string Anim = "Fluke";

		public float averageLifeTime = 2.5f;

		public int baseDamage = 4;

		//public TriggerEnterEvent damager;

		public GameObject splatEffect;

		public AudioClip[] splatSounds;

		private float lifeEndTime;

		private bool hasBounced;

		private bool hasBursted;

		private tk2dSpriteAnimator animator;

		private MeshRenderer meshRenderer;

		private Rigidbody2D body;

		private SpriteFlash spriteFlash;

		private ObjectBounce objectBounce;

		private AudioSource audioPlayer;
	}
}