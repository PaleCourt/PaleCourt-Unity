using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Physics 2d")]
    [Tooltip("Accelerates objects velocity, and clamps top speed")]
    public class AccelerateVelocity : RigidBody2dActionBase
    {
        [CheckForComponent(typeof (Rigidbody2D))]
        [RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmFloat xAccel;
        public FsmFloat yAccel;
        public FsmFloat xMaxSpeed;
        public FsmFloat yMaxSpeed;

        public override void Reset()
        {
            gameObject = null;
            FsmFloat fsmFloat1 = new FsmFloat();
            fsmFloat1.UseVariable = true;
            xAccel = fsmFloat1;
            FsmFloat fsmFloat2 = new FsmFloat();
            fsmFloat2.UseVariable = true;
            yAccel = fsmFloat2;
            FsmFloat fsmFloat3 = new FsmFloat();
            fsmFloat3.UseVariable = true;
            xMaxSpeed = fsmFloat3;
            FsmFloat fsmFloat4 = new FsmFloat();
            fsmFloat4.UseVariable = true;
            yMaxSpeed = fsmFloat4;
        }

        public override void Awake()
        {
            Fsm.HandleFixedUpdate = true;
        }

        public override void OnEnter()
        {
            CacheRigidBody2d(Fsm.GetOwnerDefaultTarget(gameObject));
        }

        public override void OnPreprocess()
        {
            Fsm.HandleFixedUpdate = true;
        }

        public override void OnFixedUpdate()
        {
            DoSetVelocity();
        }

        private void DoSetVelocity()
        {
            if (rb2d == null)
                return;
            Vector2 vector2 = rb2d.velocity;
            if (!xAccel.IsNone)
                vector2 = new Vector2(Mathf.Clamp(vector2.x + xAccel.Value, -xMaxSpeed.Value, xMaxSpeed.Value), vector2.y);
            if (!yAccel.IsNone)
            {
                float y = Mathf.Clamp(vector2.y + yAccel.Value, -yMaxSpeed.Value, yMaxSpeed.Value);
                vector2 = new Vector2(vector2.x, y);
            }
            rb2d.velocity = vector2;
        }
    }
}