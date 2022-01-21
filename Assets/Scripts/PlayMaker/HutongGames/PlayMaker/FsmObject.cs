using Object = UnityEngine.Object;
using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmObject : NamedVariable
	{
		[SerializeField]
		private string typeName;
		[SerializeField]
		private Object value;
	}
}
