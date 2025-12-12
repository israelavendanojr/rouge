using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio Event")]
public class SimpleAudioEvent: AudioEvent
{
    public AudioClip clip;
    public AudioMixerGroup output;
    [Space(10)]
    [Range(0,2)] public float volume = 1f, pitch = 1f;
    public bool loop;

    public override void Play(AudioSource source)
    {
        if (clip == null)
            return;

        source.clip = clip;
        source.outputAudioMixerGroup = output;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.Play();
    }
    public void Play()
    {
        GameObject audioObject = new GameObject("Audio Event: " + name);
        AudioSource source = audioObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.outputAudioMixerGroup = output;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.Play();

        audioObject.AddComponent<DestroyAudioSourceOnComplete>();
    }
}