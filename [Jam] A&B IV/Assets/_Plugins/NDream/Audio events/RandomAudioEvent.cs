using UnityEngine;

[CreateAssetMenu(fileName = "RandomAudioEvent", menuName = "Framework/Audio Events/Random")]
public class RandomAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    public FloatRange volume;

    [MinMaxRange(0, 2)]
    public FloatRange pitch;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0) 
            return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = volume.GetRandom();
        source.pitch = pitch.GetRandom();
        
        source.Play();
    }
}