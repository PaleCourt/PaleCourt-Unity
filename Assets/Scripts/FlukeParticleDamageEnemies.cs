using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiveKnights
{
    public class FlukeParticleDamageEnemies : MonoBehaviour
    {
		public int damage = 4;
		public float LifeSpan = 1f;
        /*private void OnParticleCollision(GameObject other)
		{
			HealthManager component = other.GetComponent<HealthManager>();
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
			}
		}*/
        private void OnTriggerEnter2D(Collider2D collision)
		{
			/*HealthManager component =collision.GetComponent<HealthManager>();
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
		}
        private void Update()
        {
			LifeSpan -= Time.deltaTime;
			if (LifeSpan <= 0)
				Destroy(gameObject);
        }
    }
}
