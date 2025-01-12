using HutongGames.PlayMaker;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	public class TranslateV2 : FsmStateAction
	{
		public FsmOwnerDefault gameObject;
		public FsmVector3 vector;
		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;
		public Space space;
		public bool perSecond;
		public bool everyFrame;
		public bool lateUpdate;
		public bool fixedUpdate;
		public bool alwaysOnStart;
		public FsmFloat yMin;
		public FsmFloat yMax;
	}
}
