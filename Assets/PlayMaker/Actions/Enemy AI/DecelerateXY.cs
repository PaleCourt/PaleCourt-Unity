using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [Tooltip("Decelerate X and Y separately. Uses multiplication.")]
  [ActionCategory("Enemy AI")]
  public class DecelerateXY : RigidBody2dActionBase
  {
    [RequiredField]
    [CheckForComponent(typeof (Rigidbody2D))]
    public FsmOwnerDefault gameObject;
    public FsmFloat decelerationX;
    public FsmFloat decelerationY;

    public override void Reset()
    {
      gameObject = null;
      decelerationX = null;
      decelerationY = null;
    }

    public override void Awake()
    {
      Fsm.HandleFixedUpdate = true;
    }

    public override void OnPreprocess()
    {
      Fsm.HandleFixedUpdate = true;
    }

    public override void OnEnter()
    {
      CacheRigidBody2d(Fsm.GetOwnerDefaultTarget(gameObject));
      DecelerateSelf();
    }

    public override void OnFixedUpdate()
    {
      DecelerateSelf();
    }

    private void DecelerateSelf()
    {
      if (rb2d == null)
        return;
      Vector2 velocity = rb2d.velocity;
      if (!decelerationX.IsNone)
      {
        if (velocity.x < 0.0)
        {
          velocity.x *= decelerationX.Value;
          if (velocity.x > 0.0)
            velocity.x = 0.0f;
        }
        else if (velocity.x > 0.0)
        {
          velocity.x *= decelerationX.Value;
          if (velocity.x < 0.0)
            velocity.x = 0.0f;
        }
        if (velocity.x < 1.0 / 1000.0 && velocity.x > -1.0 / 1000.0)
          velocity.x = 0.0f;
      }
      if (!decelerationY.IsNone)
      {
        if (velocity.y < 0.0)
        {
          velocity.y *= decelerationY.Value;
          if (velocity.y > 0.0)
            velocity.y = 0.0f;
        }
        else if (velocity.y > 0.0)
        {
          velocity.y *= decelerationY.Value;
          if (velocity.y < 0.0)
            velocity.y = 0.0f;
        }
        if (velocity.y < 1.0 / 1000.0 && velocity.y > -1.0 / 1000.0)
          velocity.y = 0.0f;
      }
      rb2d.velocity = velocity;
    }
  }
}
