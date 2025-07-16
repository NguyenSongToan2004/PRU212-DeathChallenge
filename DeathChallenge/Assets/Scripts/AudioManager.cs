using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        // Cập nhật sounds array nếu scene hiện tại có AudioManager khác
        AudioManager currentSceneAudioManager = FindObjectOfType<AudioManager>();
        if (currentSceneAudioManager != null && currentSceneAudioManager != this)
        {
            // Copy sounds từ AudioManager của scene hiện tại
            sounds = currentSceneAudioManager.sounds;
            
            // Khởi tạo lại AudioSource cho sounds mới
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
            
            // Hủy AudioManager của scene hiện tại
            Destroy(currentSceneAudioManager.gameObject);
        }
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{sound}' not found!");
        }
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.Stop();
    }

    public void PlayBackground(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.Play();
    }

    public void StopBackground(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null) return;

        s.source.Stop();
    }

    public void ReloadSounds(Sound[] newSounds)
    {
        // Dừng tất cả âm thanh đang phát
        foreach (Sound s in sounds)
        {
            if (s.source != null)
            {
                s.source.Stop();
                Destroy(s.source);
            }
        }
        
        // Cập nhật sounds array
        sounds = newSounds;
        
        // Khởi tạo lại AudioSource
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
}
