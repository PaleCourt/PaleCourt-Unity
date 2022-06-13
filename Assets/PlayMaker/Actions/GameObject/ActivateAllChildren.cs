using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GameObject)]
    [Tooltip("Activate or deactivate all children on a GameObject.")]
    public class ActivateAllChildren : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [RequiredField]
        public FsmGameObject gameObject;
        public bool activate;

        public override void Reset()
        {
            gameObject = null;
            activate = true;
        }

        public override void OnEnter()
        {
            GameObject gameObject = this.gameObject.Value;
            if (gameObject != null)
            {
                IEnumerator enumerator = gameObject.transform.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                        ((Component) enumerator.Current).gameObject.SetActive(activate);
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                        disposable.Dispose();
                }
            }
            Finish();
        }
    }
}