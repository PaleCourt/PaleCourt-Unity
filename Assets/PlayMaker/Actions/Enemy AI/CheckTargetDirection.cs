using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory("Enemy AI")]
  [Tooltip("Check whether target is left/right/up/down relative to object")]
  public class CheckTargetDirection : FsmStateAction
  {
    [RequiredField]
    public FsmOwnerDefault gameObject;
    [RequiredField]
    public FsmGameObject target;
    public FsmEvent aboveEvent;
    public FsmEvent belowEvent;
    public FsmEvent rightEvent;
    public FsmEvent leftEvent;
    [UIHint(UIHint.Variable)]
    public FsmBool aboveBool;
    [UIHint(UIHint.Variable)]
    public FsmBool belowBool;
    [UIHint(UIHint.Variable)]
    public FsmBool rightBool;
    [UIHint(UIHint.Variable)]
    public FsmBool leftBool;
    private FsmGameObject self;
    private FsmFloat x;
    private FsmFloat y;
    public bool everyFrame;

    public override void Reset()
    {
      gameObject = null;
      target = null;
      everyFrame = false;
    }

    public override void OnEnter()
    {
      self = Fsm.GetOwnerDefaultTarget(gameObject);
      DoCheckDirection();
      if (everyFrame)
        return;
      Finish();
    }

    public override void OnUpdate()
    {
      DoCheckDirection();
      if (everyFrame)
        return;
      Finish();
    }

    private void DoCheckDirection()
    {
      try
      {
        if (gameObject == null || gameObject.GameObject == null || gameObject.GameObject.Value == null)
        {
          if (gameObject == null)
          {
            gameObject = new FsmOwnerDefault();
            gameObject.OwnerOption = OwnerDefaultOption.UseOwner;
          }
          gameObject.GameObject = new FsmGameObject(Fsm.GameObject);
        }
        if (gameObject == null || gameObject.GameObject == null || gameObject.GameObject.Value == null || (target == null || target.Value == null))
          Finish();
        else
          orig_DoCheckDirection();
      }
      catch (Exception ex)
      {
        Debug.Log("Error.");
        Finish();
      }
    }

    private void orig_DoCheckDirection()
    {
      float x1 = self.Value.transform.position.x;
      float y1 = self.Value.transform.position.y;
      float x2 = target.Value.transform.position.x;
      float y2 = target.Value.transform.position.y;
      if (x1 < x2)
      {
        Fsm.Event(rightEvent);
        rightBool.Value = true;
      }
      else
        rightBool.Value = false;
      if (x1 > x2)
      {
        Fsm.Event(leftEvent);
        leftBool.Value = true;
      }
      else
        leftBool.Value = false;
      if (y1 < y2)
      {
        Fsm.Event(aboveEvent);
        aboveBool.Value = true;
      }
      else
        aboveBool.Value = false;
      if (y1 > y2)
      {
        Fsm.Event(belowEvent);
        belowBool.Value = true;
      }
      else
        belowBool.Value = false;
    }
  }
}
