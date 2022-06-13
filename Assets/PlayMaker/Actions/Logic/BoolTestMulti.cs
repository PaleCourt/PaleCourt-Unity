namespace HutongGames.PlayMaker.Actions
{
    [Tooltip("Tests if all the given Bool Variables are are equal to thier Bool States.")]
    [ActionCategory(ActionCategory.Logic)]
    public class BoolTestMulti : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [RequiredField]
        [Tooltip("This must be the same number used for Bool States.")]
        public FsmBool[] boolVariables;
        [RequiredField]
        [Tooltip("This must be the same number used for Bool Variables.")]
        public FsmBool[] boolStates;
        public FsmEvent trueEvent;
        public FsmEvent falseEvent;
        [UIHint(UIHint.Variable)]
        public FsmBool storeResult;
        public bool everyFrame;

        public override void Reset()
        {
            boolVariables = null;
            boolStates = null;
            trueEvent = null;
            falseEvent = null;
            storeResult = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoAllTrue();
            if (everyFrame)
                return;
            Finish();
        }

        public override void OnUpdate()
        {
            DoAllTrue();
        }

        private void DoAllTrue()
        {
            if (boolVariables.Length == 0 || boolStates.Length == 0 || boolVariables.Length != boolStates.Length)
                return;
            bool flag = true;
            for (int index = 0; index < boolVariables.Length; ++index)
            {
                if (boolVariables[index].Value != boolStates[index].Value)
                {
                    flag = false;
                    break;
                }
            }
            storeResult.Value = flag;
            if (flag)
                Fsm.Event(trueEvent);
            else
                Fsm.Event(falseEvent);
        }
    }
}