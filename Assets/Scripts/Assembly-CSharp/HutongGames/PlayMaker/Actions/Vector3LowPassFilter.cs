using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	public class Vector3LowPassFilter : FsmStateAction
	{
		public FsmVector3 vector3Variable;
		public FsmFloat filteringFactor;
	}
}
