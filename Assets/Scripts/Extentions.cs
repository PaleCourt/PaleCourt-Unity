using System;
using UnityEngine;

// Token: 0x02000B51 RID: 2897
public static class Collision2DUtils
{
	// Token: 0x0600380F RID: 14351 RVA: 0x001315E4 File Offset: 0x0012F7E4
	public static Collision2DUtils.Collision2DSafeContact GetSafeContact(this Collision2D collision)
	{
		int contacts = collision.GetContacts(Collision2DUtils.contactsBuffer);
		if (contacts >= 1)
		{
			ContactPoint2D contactPoint2D = Collision2DUtils.contactsBuffer[0];
			return new Collision2DUtils.Collision2DSafeContact
			{
				Point = contactPoint2D.point,
				Normal = contactPoint2D.normal,
				IsLegitimate = true
			};
		}
		Vector2 b = collision.collider.transform.TransformPoint(collision.collider.offset);
		Vector2 a = collision.otherCollider.transform.TransformPoint(collision.otherCollider.offset);
		return new Collision2DUtils.Collision2DSafeContact
		{
			Point = (a + b) * 0.5f,
			Normal = (a - b).normalized,
			IsLegitimate = false
		};
	}

	// Token: 0x0400417E RID: 16766
	private static ContactPoint2D[] contactsBuffer = new ContactPoint2D[1];

	// Token: 0x02000B52 RID: 2898
	public struct Collision2DSafeContact
	{
		// Token: 0x0400417F RID: 16767
		public Vector2 Point;

		// Token: 0x04004180 RID: 16768
		public Vector2 Normal;

		// Token: 0x04004181 RID: 16769
		public bool IsLegitimate;
	}
}