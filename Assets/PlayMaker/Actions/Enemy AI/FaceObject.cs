using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [Tooltip("Object A will flip to face Object B horizontally.")]
  [ActionCategory("Enemy AI")]
  public class FaceObject : FsmStateAction
  {
    public bool resetFrame = true;
    [RequiredField]
    public FsmGameObject objectA;
    [UIHint(UIHint.Variable)]
    [RequiredField]
    public FsmGameObject objectB;
    [Tooltip("Does object A's sprite face right?")]
    public FsmBool spriteFacesRight;
    public bool playNewAnimation;
    public FsmString newAnimationClip;
    [Tooltip("Repeat every frame.")]
    public bool everyFrame;
    private float xScale;
    private FsmVector3 vector;
    private tk2dSpriteAnimator _sprite;

    public override void Reset()
    {
      objectA = null;
      objectB = null;
      newAnimationClip = null;
      spriteFacesRight = false;
      everyFrame = false;
      resetFrame = false;
      playNewAnimation = false;
    }

    public override void OnEnter()
    {
      _sprite = objectA.Value.GetComponent<tk2dSpriteAnimator>();
      if (_sprite == null)
        Finish();
      xScale = objectA.Value.transform.localScale.x;
      if (xScale < 0.0)
        xScale *= -1f;
      DoFace();
      if (everyFrame)
        return;
      Finish();
    }

    public override void OnUpdate()
    {
      DoFace();
    }

    private void DoFace()
    {
      try
      {
        if (objectA == null || objectA.Value == null)
          objectA = new FsmGameObject(Fsm.GameObject);
        if (objectA == null || objectA.Value == null || (objectB == null || objectA.Value == null))
          Finish();
        else
          orig_DoFace();
      }
      catch
      {
        Finish();
      }
    }

    private void orig_DoFace()
    {
      Vector3 localScale = objectA.Value.transform.localScale;
      if (objectB.Value == null || objectB.IsNone)
        Finish();
      if (objectA.Value.transform.position.x < objectB.Value.transform.position.x)
      {
        if (spriteFacesRight.Value)
        {
          if (localScale.x != xScale)
          {
            localScale.x = xScale;
            if (resetFrame)
              _sprite.PlayFromFrame(0);
            if (playNewAnimation)
              _sprite.Play(newAnimationClip.Value);
          }
        }
        else if (localScale.x != -xScale)
        {
          localScale.x = -xScale;
          if (resetFrame)
            _sprite.PlayFromFrame(0);
          if (playNewAnimation)
            _sprite.Play(newAnimationClip.Value);
        }
      }
      else if (spriteFacesRight.Value)
      {
        if (localScale.x != -xScale)
        {
          localScale.x = -xScale;
          if (resetFrame)
            _sprite.PlayFromFrame(0);
          if (playNewAnimation)
            _sprite.Play(newAnimationClip.Value);
        }
      }
      else if (localScale.x != xScale)
      {
        localScale.x = xScale;
        if (resetFrame)
          _sprite.PlayFromFrame(0);
        if (playNewAnimation)
          _sprite.Play(newAnimationClip.Value);
      }
      objectA.Value.transform.localScale = new Vector3(localScale.x, objectA.Value.transform.localScale.y, objectA.Value.transform.localScale.z);
    }
  }
}
