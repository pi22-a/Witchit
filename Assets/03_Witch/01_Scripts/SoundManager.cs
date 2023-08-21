using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    private AudioSource audioSource = null;

    public enum BGM
    {
        Lobby,
        Ready,
        Game,
        End
    }

    public enum Effect
    {
        Button_Click
    }

    public AudioClip[] bgms;
    public AudioClip[] effects;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM(BGM bgm)
    {
        audioSource.clip = bgms[(int)bgm];
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PlayEffect(Effect effect)
    {
        audioSource.PlayOneShot(effects[(int)effect]);
    }
}
