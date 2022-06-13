using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory(ActionCategory.StateMachine)]
  [Tooltip("Sends a Random Event picked from an array of Events. Optionally set the relative weight of each event. Use ints to keep events from being fired x times in a row.")]
  public class SendRandomEventV3 : FsmStateAction
  {
    [CompoundArray("Events", "Event", "Weight")]
    public FsmEvent[] events;
    [HasFloatSlider(0.0f, 1f)]
    public FsmFloat[] weights;
    [UIHint(UIHint.Variable)]
    public FsmInt[] trackingInts;
    public FsmInt[] eventMax;
    [UIHint(UIHint.Variable)]
    public FsmInt[] trackingIntsMissed;
    public FsmInt[] missedMax;
    private int loops;
    private DelayedEvent delayedEvent;

    public override void Reset()
    {
      events = new FsmEvent[3];
      weights = new FsmFloat[3]
      {
        1f,
        1f,
        1f
      };
    }

    public override void OnEnter()
    {
      bool flag1 = false;
      bool flag2 = false;
      int index1 = 0;
      while (!flag1)
      {
        int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(weights);
        if (randomWeightedIndex != -1)
        {
          for (int index2 = 0; index2 < trackingIntsMissed.Length; ++index2)
          {
            if (trackingIntsMissed[index2].Value >= missedMax[index2].Value)
            {
              flag2 = true;
              index1 = index2;
            }
          }
          if (flag2)
          {
            flag1 = true;
            for (int index2 = 0; index2 < trackingInts.Length; ++index2)
            {
              trackingInts[index2].Value = 0;
              ++trackingIntsMissed[index2].Value;
            }
            trackingIntsMissed[index1].Value = 0;
            trackingInts[index1].Value = 1;
            Fsm.Event(events[index1]);
          }
          else if (trackingInts[randomWeightedIndex].Value < eventMax[randomWeightedIndex].Value)
          {
            int num = ++trackingInts[randomWeightedIndex].Value;
            for (int index2 = 0; index2 < trackingInts.Length; ++index2)
            {
              trackingInts[index2].Value = 0;
              ++trackingIntsMissed[index2].Value;
            }
            trackingInts[randomWeightedIndex].Value = num;
            trackingIntsMissed[randomWeightedIndex].Value = 0;
            flag1 = true;
            Fsm.Event(events[randomWeightedIndex]);
          }
        }
        ++loops;
        if (loops > 100)
        {
          Fsm.Event(events[0]);
          flag1 = true;
          Finish();
        }
      }
      Finish();
    }
  }
}
