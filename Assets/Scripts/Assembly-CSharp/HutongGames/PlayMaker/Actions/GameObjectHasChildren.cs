using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	public class GameObjectHasChildren : FsmStateAction
	{
		public FsmOwnerDefault gameObject;
		public FsmEvent trueEvent;
		public FsmEvent falseEvent;
		public FsmBool storeResult;
		public bool everyFrame;
	}
}
