using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class AudioManager : MonoBehaviour
{
    private AudioSourcePool audioSourcePool = new();
    public AudioClip[] Clips;
    public AudioSource footprint;
    public AudioSource breathe;
    private void Start()
    {
        SetAudio();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SetAudio()
    {
        audioSourcePool.SetAudoiPool();
        footprint = audioSourcePool.GetAudioSource();
        breathe = audioSourcePool.GetAudioSource();
        footprint.clip = Clips[(int)SoundEffect.PlayerWalk];
        breathe.clip = Clips[((int)SoundEffect.PlayerBreath)];
        footprint.loop = true;
        breathe.loop = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "inGame")
        {
            SetAudio();
        }
    }

    public void PlaySFX(SoundEffect sound)
    {
        AudioSource source = audioSourcePool.GetAudioSource();
        source.clip = Clips[(int)sound];
        source.volume = 0.5f;
        source.Play();
        StartCoroutine(ReturnSourceWhenFinished(source));
    }
    public void PlayBGM(bool control)
    {
        if (control) GetComponent<AudioSource>().Play();
        else GetComponent<AudioSource>().Stop();
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
