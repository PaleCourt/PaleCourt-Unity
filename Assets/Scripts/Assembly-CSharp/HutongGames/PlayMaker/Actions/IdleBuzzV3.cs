using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	public class IdleBuzzV3 : RigidBody2dActionBase
	{
		public FsmOwnerDefault gameObject;
		public FsmFloat waitMin;
		public FsmFloat waitMax;
		public FsmFloat speedMax;
		public FsmFloat accelerationMin;
		public FsmFloat accelerationMax;
		public FsmFloat roamingRangeX;
		public FsmFloat roamingRangeY;
		public FsmVector3 manualStartPos;
	}
}
