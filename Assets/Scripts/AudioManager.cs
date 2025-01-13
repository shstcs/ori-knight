using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSourcePool audioSourcePool = new();
    public AudioClip[] Clips;
    public AudioSource footprint;
    public AudioSource breathe;
    private void Start()
    {
        audioSourcePool.setAudoiPool();
        footprint = audioSourcePool.GetAudioSource();
        breathe = audioSourcePool.GetAudioSource();
        footprint.clip = Clips[(int)SoundEffect.PlayerWalk];
        breathe.clip = Clips[((int)SoundEffect.PlayerBreath)];
        footprint.loop = true;
        breathe.loop = true;
    }

    public void PlaySFX(SoundEffect sound)
    {
        AudioSource source = audioSourcePool.GetAudioSource();
        source.clip = Clips[(int)sound];
        source.volume = 0.5f;
        source.Play();
        StartCoroutine(ReturnSourceWhenFinished(source));
    }
    public void PlayFootprintLoop(bool control)
    {
        if (!footprint.isPlaying && control) footprint.Play();
        else if (footprint.isPlaying && !control) footprint.Pause();
    }

    public void PlayBreathLoop(bool control)
    {
        if (!breathe.isPlaying && control) breathe.Play();
        else if (breathe.isPlaying && !control) breathe.Pause();
    }
    private IEnumerator ReturnSourceWhenFinished(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        audioSourcePool.ReturnAudioSource(source);
    }
}
