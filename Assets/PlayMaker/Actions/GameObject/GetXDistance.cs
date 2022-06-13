using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [Tooltip("Measures the Distance betweens 2 Game Objects and stores the result in a Float Variable. X axis only.")]
    [ActionCategory(ActionCategory.GameObject)]
    public class GetXDistance : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmGameObject target;
        [UIHint(UIHint.Variable)]
        [RequiredField]
        public FsmFloat storeResult;
        public bool everyFrame;

        public override void Reset()
        {
            gameObject = null;
            target = null;
            storeResult = null;
            everyFrame = true;
        }

        public override void OnEnter()
        {
            DoGetDistance();
            if (everyFrame)
                return;
            Finish();
        }

        public override void OnUpdate()
        {
            DoGetDistance();
        }

        private void DoGetDistance()
        {
            GameObject gameObject = this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner ? this.gameObject.GameObject.Value : Owner;
            if (gameObject == null || target.Value == null || storeResult == null)
                return;
            float num = gameObject.transform.position.x - target.Value.transform.position.x;
            if (num < 0.0)
                num *= -1f;
            storeResult.Value = num;
        }
    }
}