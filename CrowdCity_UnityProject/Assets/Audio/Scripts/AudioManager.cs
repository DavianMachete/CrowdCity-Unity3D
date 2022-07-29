using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] AudioData buttonSound;


    bool clipsLoaded = false;
    private void Awake()
    {
        instance = this;
        LoadClips();
    }

    void LoadClips()
    {
        buttonSound.Init();
        clipsLoaded = true;
    }

    public bool isAudioEnabled = false;

    public void CheckAudioStatus()
    {
        //isAudioEnabled = RemoteConfigLoader.AudioConfig.Audio;
    }

    public void ToggleAudio(bool enable)
    {
        isAudioEnabled = enable;

        if (!clipsLoaded) return;
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }
}

[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;

    public float volume =1;
    public float pitch=1;

    public bool loop;

    AudioSource audioSource = null;
    AudioManager m_audioManager;

    public void Init()
    {
        m_audioManager = AudioManager.instance;
        audioSource = m_audioManager.gameObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
    }

    public void Play(float fadeInDur = 0)
    {
        if (audioSource == null) return;

        if (!m_audioManager.isAudioEnabled) return;

        audioSource.Play();

        if (fadeInDur != 0)
        {
            DoFadeIn(fadeInDur,volume);
        }
    }
    public void SetVol(float _v, float fadeInDur = 0)
    {
        float newVol = volume * _v;
        if (fadeInDur != 0)
        {
            DoFadeIn(fadeInDur, newVol);
        }
        else
        {
            audioSource.volume = newVol;
        }
    }

    public void SetPitch(float _p)
    {
        audioSource.pitch = pitch * _p;
    }

    void DoFadeIn(float fadeInDur, float targetVol)
    {
        if (fadeInRoutineC != null) m_audioManager.StopCoroutine(fadeInRoutineC);
        fadeInRoutineC = m_audioManager.StartCoroutine(FadeInRoutine(fadeInDur,targetVol));
    }

    Coroutine fadeInRoutineC;
    IEnumerator FadeInRoutine(float dur, float targetVol)
    {
        float startVol = 0;
        float t = audioSource.volume;
        float startT = Time.time;

        while(t<1)
        {
            t = (Time.time - startT) / dur;

            audioSource.volume = Mathf.Lerp(startVol,targetVol,t);

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void Stop()
    {
        //if (!m_audioManager.isAudioEnabled) return;
        if(audioSource != null)
            audioSource.Stop();
    }
}

