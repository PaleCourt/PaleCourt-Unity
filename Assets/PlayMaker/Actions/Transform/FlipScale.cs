using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Transform)]
    [Tooltip("Sets the Scale of a Game Object. To leave any axis unchanged, set variable to 'None'.")]
    public class FlipScale : FsmStateAction
    {
        [Tooltip("The GameObject to scale.")]
        [RequiredField]
        public FsmOwnerDefault gameObject;
        public bool flipHorizontally;
        public bool flipVertically;
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
        public bool lateUpdate;

        public override void Reset()
        {
            flipHorizontally = false;
            flipVertically = false;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoFlipScale();
            if (everyFrame)
                return;
            Finish();
        }

        public override void OnUpdate()
        {
            if (lateUpdate)
                return;
            DoFlipScale();
        }

        public override void OnLateUpdate()
        {
            if (lateUpdate)
                DoFlipScale();
            if (everyFrame)
                return;
            Finish();
        }

        private void DoFlipScale()
        {
            GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
            if (ownerDefaultTarget == null)
                return;
            Vector3 localScale = ownerDefaultTarget.transform.localScale;
            if (flipHorizontally)
                localScale.x = -localScale.x;
            if (flipVertically)
                localScale.y = -localScale.y;
            ownerDefaultTarget.transform.localScale = localScale;
        }
    }
}