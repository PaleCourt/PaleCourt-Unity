using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	public class ListenForRight : FsmStateAction
	{
		public FsmEventTarget eventTarget;
		public FsmEvent wasPressed;
		public FsmEvent wasReleased;
		public FsmEvent isPressed;
		public FsmEvent isNotPressed;
	}
}
