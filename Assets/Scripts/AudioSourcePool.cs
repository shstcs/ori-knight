// 2025-01-11 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public int poolSize = 10; // 풀의 크기

    private Queue<AudioSource> pool;

    public void SetAudoiPool()
    {
        pool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("AudioSource"));
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            obj.SetActive(false);
            pool.Enqueue(audioSource);
        }
    }

    public AudioSource GetAudioSource()
    {
        if (pool.Count > 0)
        {
            AudioSource audioSource = pool.Dequeue();
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }
        else
        {
            // 풀에 사용 가능한 오디오 소스가 없으면 새로 생성
            GameObject obj = Instantiate(Resources.Load<GameObject>("AudioSource"));
            return obj.GetComponent<AudioSource>();
        }
    }

    public void ReturnAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        pool.Enqueue(audioSource);
    }
}