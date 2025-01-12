using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	public class FlingObjectsFromGlobalPool : RigidBody2dActionBase
	{
		public FsmGameObject gameObject;
		public FsmGameObject spawnPoint;
		public FsmVector3 position;
		public FsmInt spawnMin;
		public FsmInt spawnMax;
		public FsmFloat speedMin;
		public FsmFloat speedMax;
		public FsmFloat angleMin;
		public FsmFloat angleMax;
		public FsmFloat originVariationX;
		public FsmFloat originVariationY;
		public FsmString FSM;
		public FsmString FSMEvent;
	}
}
