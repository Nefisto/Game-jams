using UnityEngine;

[CreateAssetMenu(fileName = "Simple audio event", menuName = "Framework/Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip clip;

    public FloatRange volume;

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
