using UnityEngine;

[CreateAssetMenu(fileName = "Simple audio event", menuName = "Framework/Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip clip;

    [MinMaxRange(0, 2)]
    public FloatRange volume;

    [MinMaxRange(0, 1)]
    public FloatRange pitch;

    public override void Play(AudioSource source)
    {
        if (!clip)
            return;

        source.volume = volume.GetRandom();
        source.pitch = pitch.GetRandom();

        source.PlayOneShot(clip);
    }
}
