using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory(ActionCategory.Audio)]
  [Tooltip("Instantiate an Audio Player object and play a oneshot sound via its Audio Source.")]
  public class AudioPlayerOneShotSingle : FsmStateAction
  {
    public FsmFloat volume = 1f;
    [RequiredField]
    [Tooltip("The object to spawn. Select Audio Player prefab.")]
    public FsmGameObject audioPlayer;
    [RequiredField]
    [Tooltip("Object to use as the spawn point of Audio Player")]
    public FsmGameObject spawnPoint;
    [ObjectType(typeof (AudioClip))]
    public FsmObject audioClip;
    public FsmFloat pitchMin;
    public FsmFloat pitchMax;
    public FsmFloat delay;
    public FsmGameObject storePlayer;
    private AudioSource audio;
    private float timer;

    public override void Reset()
    {
      spawnPoint = null;
      pitchMin = 1f;
      pitchMax = 1f;
      volume = 1f;
    }

    public override void OnEnter()
    {
      timer = 0.0f;
      if (delay.Value != 0.0)
        return;
      DoPlayRandomClip();
      Finish();
    }

    public override void OnUpdate()
    {
      if (delay.Value <= 0.0)
        return;
      if (timer < delay.Value)
      {
        timer += Time.deltaTime;
      }
      else
      {
        DoPlayRandomClip();
        Finish();
      }
    }

    private void DoPlayRandomClip()
    {
      if (audioPlayer.IsNone || spawnPoint.IsNone || !(spawnPoint.Value != null))
        return;
      GameObject gameObject1 = audioPlayer.Value;
      Vector3 position = spawnPoint.Value.transform.position;
      Vector3 up = Vector3.up;
      if (audioPlayer.Value != null)
      {
        GameObject gameObject2 = audioPlayer.Value.Spawn(position, Quaternion.Euler(up));
        audio = gameObject2.GetComponent<AudioSource>();
        storePlayer.Value = gameObject2;
        AudioClip clip = audioClip.Value as AudioClip;
        audio.pitch = Random.Range(pitchMin.Value, pitchMax.Value);
        audio.volume = volume.Value;
        if (!(clip != null))
          return;
        audio.PlayOneShot(clip);
      }
      else
        Debug.LogError("AudioPlayer object not set!");
    }
  }
}
