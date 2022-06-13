using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory(ActionCategory.Physics)]
  [Tooltip("Detect additional collisions between the Owner of this FSM and other object with additional raycasting.")]
  public class CheckCollisionSide : FsmStateAction
  {
    [UIHint(UIHint.Variable)]
    public FsmBool topHit;
    [UIHint(UIHint.Variable)]
    public FsmBool rightHit;
    [UIHint(UIHint.Variable)]
    public FsmBool bottomHit;
    [UIHint(UIHint.Variable)]
    public FsmBool leftHit;
    public FsmEvent topHitEvent;
    public FsmEvent rightHitEvent;
    public FsmEvent bottomHitEvent;
    public FsmEvent leftHitEvent;
    public bool otherLayer;
    public int otherLayerNumber;
    public FsmBool ignoreTriggers;
    private PlayMakerUnity2DProxy _proxy;
    private Collider2D col2d;
    private const float RAYCAST_LENGTH = 0.08f;
    private List<Vector2> topRays;
    private List<Vector2> rightRays;
    private List<Vector2> bottomRays;
    private List<Vector2> leftRays;
    private bool checkUp;
    private bool checkDown;
    private bool checkLeft;
    private bool checkRight;

    public override void Reset()
    {
      checkUp = false;
      checkDown = false;
      checkLeft = false;
      checkRight = false;
    }

    public override void OnEnter()
    {
      col2d = Fsm.GameObject.GetComponent<Collider2D>();
      topRays = new List<Vector2>(3);
      rightRays = new List<Vector2>(3);
      bottomRays = new List<Vector2>(3);
      leftRays = new List<Vector2>(3);
      _proxy = Owner.GetComponent<PlayMakerUnity2DProxy>();
      if (_proxy == null)
        _proxy = Owner.AddComponent<PlayMakerUnity2DProxy>();
      _proxy.AddOnCollisionStay2dDelegate(DoCollisionStay2D);
      checkUp = !topHit.IsNone || topHitEvent != null;
      checkRight = !rightHit.IsNone || rightHitEvent != null;
      checkDown = !bottomHit.IsNone || bottomHitEvent != null;
      if (!leftHit.IsNone || leftHitEvent != null)
        checkLeft = true;
      else
        checkLeft = false;
    }

    public override void OnExit()
    {
      _proxy.RemoveOnCollisionStay2dDelegate(DoCollisionStay2D);
    }

    public override void OnUpdate()
    {
      if (!topHit.Value && !bottomHit.Value && !rightHit.Value && !leftHit.Value)
        return;
      if (!otherLayer)
        CheckTouching(8);
      else
        CheckTouching(otherLayerNumber);
    }

    public new void DoCollisionStay2D(Collision2D collision)
    {
      Log("Collision");
      if (!otherLayer)
      {
        if (collision.gameObject.layer != 8)
          return;
        CheckTouching(8);
      }
      else
        CheckTouching(otherLayerNumber);
    }

    public new void DoCollisionExit2D(Collision2D collision)
    {
      topHit.Value = false;
      rightHit.Value = false;
      bottomHit.Value = false;
      leftHit.Value = false;
    }

    private void CheckTouching(LayerMask layer)
    {
      if (checkUp)
      {
        topRays.Clear();
        topRays.Add(new Vector2(col2d.bounds.min.x, col2d.bounds.max.y));
        topRays.Add(new Vector2(col2d.bounds.center.x, col2d.bounds.max.y));
        topRays.Add(col2d.bounds.max);
        topHit.Value = false;
        for (int index = 0; index < 3; ++index)
        {
          RaycastHit2D raycastHit2D = Physics2D.Raycast(topRays[index], Vector2.up, RAYCAST_LENGTH, 1 << layer);
          if (raycastHit2D.collider != null && (!ignoreTriggers.Value || !raycastHit2D.collider.isTrigger))
          {
            topHit.Value = true;
            Fsm.Event(topHitEvent);
            break;
          }
        }
      }
      if (checkRight)
      {
        rightRays.Clear();
        rightRays.Add(col2d.bounds.max);
        rightRays.Add(new Vector2(col2d.bounds.max.x, col2d.bounds.center.y));
        rightRays.Add(new Vector2(col2d.bounds.max.x, col2d.bounds.min.y));
        rightHit.Value = false;
        for (int index = 0; index < 3; ++index)
        {
          RaycastHit2D raycastHit2D = Physics2D.Raycast(rightRays[index], Vector2.right, RAYCAST_LENGTH, 1 << layer);
          if (raycastHit2D.collider != null && (!ignoreTriggers.Value || !raycastHit2D.collider.isTrigger))
          {
            rightHit.Value = true;
            Fsm.Event(rightHitEvent);
            break;
          }
        }
      }
      if (checkDown)
      {
        bottomRays.Clear();
        bottomRays.Add(new Vector2(col2d.bounds.max.x, col2d.bounds.min.y));
        bottomRays.Add(new Vector2(col2d.bounds.center.x, col2d.bounds.min.y));
        bottomRays.Add(col2d.bounds.min);
        bottomHit.Value = false;
        for (int index = 0; index < 3; ++index)
        {
          RaycastHit2D raycastHit2D = Physics2D.Raycast(bottomRays[index], Vector2.down, RAYCAST_LENGTH, 1 << layer);
          if (raycastHit2D.collider != null && (!ignoreTriggers.Value || !raycastHit2D.collider.isTrigger))
          {
            bottomHit.Value = true;
            Fsm.Event(bottomHitEvent);
            break;
          }
        }
      }
      if (!checkLeft)
        return;
      leftRays.Clear();
      leftRays.Add(col2d.bounds.min);
      leftRays.Add(new Vector2(col2d.bounds.min.x, col2d.bounds.center.y));
      leftRays.Add(new Vector2(col2d.bounds.min.x, col2d.bounds.max.y));
      leftHit.Value = false;
      for (int index = 0; index < 3; ++index)
      {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(leftRays[index], Vector2.left, RAYCAST_LENGTH, 1 << layer);
        if (raycastHit2D.collider != null && (!ignoreTriggers.Value || !raycastHit2D.collider.isTrigger))
        {
          leftHit.Value = true;
          Fsm.Event(leftHitEvent);
          break;
        }
      }
    }

    public enum CollisionSide
    {
      top,
      left,
      right,
      bottom,
      other,
    }
  }
}