using HutongGames.PlayMaker;

namespace FiveKnights
{
    [ActionCategory(ActionCategory.Time)]
    [Tooltip("Delays a State from finishing by the specified time multiplied by the specified multiplier. NOTE: Other actions continue, but FINISHED can't happen before Time.")]
    public class Wait : FsmStateAction
    {
        [RequiredField]
        public FsmFloat time;
        [RequiredField]
        public FsmFloat multiplier;
        public FsmEvent finishEvent;
        public bool realTime;

        private float startTime;
        private float timer;
        private float multipliedTime;

        public override void Reset()
        {
            time = 1f;
            finishEvent = null;
            realTime = false;
        }

        public override void OnEnter()
        {
            if (time.Value <= 0)
            {
                Fsm.Event(finishEvent);
                Finish();
                return;
            }

            startTime = FsmTime.RealtimeSinceStartup;
            timer = 0f;
            multipliedTime = time.Value * multiplier.Value;
        }

        public override void OnUpdate()
        {
            // update time

            if (realTime)
            {
                timer = FsmTime.RealtimeSinceStartup - startTime;
            }
            else
            {
                timer += UnityEngine.Time.deltaTime;
            }

            if (timer >= time.Value)
            {
                Finish();
                if (finishEvent != null)
                {
                    Fsm.Event(finishEvent);
                }
            }
        }



#if UNITY_EDITOR

        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, time);
        }

        public override float GetProgress()
        {
            return UnityEngine.Mathf.Min(timer / time.Value, 1f);
        }

#endif
    }
}
