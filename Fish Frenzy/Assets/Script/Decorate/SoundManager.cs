using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SoundEffect
{
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1;
    public bool oneTime = false;
    /// <summary>
    /// 
    /// </summary>
    protected int timeCount = 0;
    public int TimeCount
    {
        get { return timeCount; }
        set { timeCount = value; }
    }
    public void ResetCouter() { timeCount = 0; }
    /// <summary>
    /// 
    /// </summary>
    protected AudioSource audioSource;
    public AudioSource AudioSource
    {
        get { return audioSource; }
        set { audioSource = value; }
    }
}

[Serializable]
public class SoundSettings
{
    public bool MusicOn = true;
    public bool SfxOn = true;
}

/// <summary>
/// This persistent singleton handles sound playing
/// </summary>
[AddComponentMenu("Corgi Engine/Managers/Sound Manager")]
public class SoundManager : PersistentSingleton<SoundManager>
{
    [Header("Settings")]
    public SoundSettings Settings;

    [Header("Music")]
    /// the music volume
    [Range(0, 1)]
    public float MusicVolume = 0.3f;

    [Header("Sound Effects")]
    /// the sound fx volume
    [Range(0, 1)]
    public float SfxVolume = 1f;

    [Header("Pause")]
    public bool MuteSfxOnPause = true;

    protected const string _saveFolderName = "CorgiEngine/";
    protected const string _saveFileName = "sound.settings";
    protected AudioSource _backgroundMusic;
    protected List<AudioSource> _loopingSounds;

    protected AudioSource _2DOneShotSource;

    /// <summary>
    /// Plays a background music.
    /// Only one background music can be active at a time.
    /// </summary>
    /// <param name="Clip">Your audio clip.</param>
    public virtual void PlayBackgroundMusic(AudioSource Music)
    {
        // if the music's been turned off, we do nothing and exit
        if (!Settings.MusicOn)
            return;
        // if we already had a background music playing, we stop it
        if (_backgroundMusic != null)
            _backgroundMusic.Stop();
        // we set the background music clip
        _backgroundMusic = Music;
        // we set the music's volume
        _backgroundMusic.volume = MusicVolume;
        // we set the loop setting to true, the music will loop forever
        _backgroundMusic.loop = true;
        // we start playing the background music
        _backgroundMusic.Play();
    }

    /// <summary>
    /// Plays a sound
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="sfx">The sound clip you want to play.</param>
    /// <param name="location">The location of the sound.</param>
    /// <param name="loop">If set to true, the sound will loop.</param>
    /// <param name="volume">If not set (value = 2), the sound will use default volume.</param>
    protected virtual AudioSource PlaySound(AudioClip sfx, Vector3 location, bool loop = false, float spartialBlend = 0f, float volume = 2f)
    {
        if (!Settings.SfxOn)
            return null;
        // we create a temporary game object to host our audio source
        GameObject temporaryAudioHost = new GameObject("TempAudio");
        // we set the temp audio's position
        temporaryAudioHost.transform.position = location;
        // we add an audio source to that host
        AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
        // make the sound 3D
        audioSource.spatialBlend = spartialBlend;
        // we set that audio source clip to the one in paramaters
        audioSource.clip = sfx;
        // we set the audio source volume to the one in parameters
        if(volume == 2)
        {
            audioSource.volume = SfxVolume;
        }
        else
        {
            audioSource.volume = volume;
        }
        // we set our loop setting
        audioSource.loop = loop;
        // we start playing the sound
        audioSource.Play();

        if (!loop)
        {
            // we destroy the host after the clip has played
            Destroy(temporaryAudioHost, sfx.length);
        }
        else
        {
            _loopingSounds.Add(audioSource);
        }

        // we return the audiosource reference
        return audioSource;
    }

    /// <summary>
    /// Plays a sound
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="sfx">The sound clip you want to play.</param>
    /// <param name="location">The location of the sound.</param>
    /// <param name="loop">If set to true, the sound will loop.</param>
    public virtual AudioSource PlaySound(SoundEffect sfx, Vector3 location, bool loop = false, float spartialBlend = 0f)
    {
        if (!Settings.SfxOn || sfx == null || sfx.clip == null)
        {
            return null;
        }
        if (sfx.oneTime == true && sfx.TimeCount > 0)
        {
            return null;
        }
        if (sfx.AudioSource && sfx.AudioSource.isPlaying)
        {
            return null;
        }
        sfx.TimeCount += 1;

        sfx.AudioSource = PlaySound(sfx.clip, location, loop, spartialBlend, sfx.volume);
        return sfx.AudioSource;
    }

    public void StopSound(SoundEffect sfx)
    {
        if (sfx.AudioSource && sfx.AudioSource.isPlaying)
        {
            sfx.AudioSource.Stop();
        }
    }

    public virtual void Play2DSoundOneShot(AudioClip sfx)
    {
        if (sfx == null)
        {
            return;
        }
        if (_2DOneShotSource == null)
        {
            GameObject audioHost = new GameObject("2DOneShotAudioHost");
            // we set the temp audio's position
            audioHost.transform.position = Vector3.zero;
            // we add an audio source to that host
            _2DOneShotSource = audioHost.AddComponent<AudioSource>() as AudioSource;
            // make the sound 2D
            _2DOneShotSource.spatialBlend = 0f;
            // we set that audio source clip to the one in paramaters
            _2DOneShotSource.clip = sfx;
            // we set the audio source volume to the one in parameters
            _2DOneShotSource.volume = SfxVolume;
        }

        _2DOneShotSource.PlayOneShot(sfx);
    }

    /// <summary>
    /// Stops the looping sounds if there are any
    /// </summary>
    /// <param name="source">Source.</param>
    public virtual void StopLoopingSound(AudioSource source)
    {
        if (source != null)
        {
            _loopingSounds.Remove(source);
            Destroy(source.gameObject);
        }
    }

    /// <summary>
    /// Sets the music on/off setting based on the value in parameters
    /// This value will be saved, and any music played after that setting change will comply
    /// </summary>
    /// <param name="status"></param>
	protected virtual void SetMusic(bool status)
    {
        Settings.MusicOn = status;
        if (status)
        {
            UnmuteBackgroundMusic();
        }
        else
        {
            MuteBackgroundMusic();
        }
    }

    /// <summary>
    /// Sets the SFX on/off setting based on the value in parameters
    /// This value will be saved, and any SFX played after that setting change will comply
    /// </summary>
    /// <param name="status"></param>
	protected virtual void SetSfx(bool status)
    {
        Settings.SfxOn = status;
    }

    /// <summary>
    /// Sets the music setting to On
    /// </summary>
	public virtual void MusicOn() { SetMusic(true); }

    /// <summary>
    /// Sets the Music setting to Off
    /// </summary>
	public virtual void MusicOff() { SetMusic(false); }

    /// <summary>
    /// Sets the SFX setting to On
    /// </summary>
	public virtual void SfxOn() { SetSfx(true); }

    /// <summary>
    /// Sets the SFX setting to Off
    /// </summary>
	public virtual void SfxOff() { SetSfx(false); }

    /// <summary>
    /// Resets the sound settings by destroying the save file
    /// </summary>
	protected virtual void ResetSoundSettings()
    {
        PlayerPrefs.DeleteKey(_saveFileName);
        //SaveLoadManager.DeleteSave(_saveFileName, _saveFolderName);
    }

    /// <summary>
    /// Mutes all sfx currently playing
    /// </summary>
    protected virtual void MuteAllSfx()
    {
        foreach (AudioSource source in _loopingSounds)
        {
            if (source != null)
            {
                source.mute = true;
            }
        }
    }

    /// <summary>
    /// Unmutes all sfx currently playing
    /// </summary>
	protected virtual void UnmuteAllSfx()
    {
        foreach (AudioSource source in _loopingSounds)
        {
            if (source != null)
            {
                source.mute = false;
            }
        }
    }

    /// <summary>
    /// Unmutes the background music
    /// </summary>
    public virtual void UnmuteBackgroundMusic()
    {
        if (_backgroundMusic != null)
        {
            _backgroundMusic.mute = false;
        }
    }

    /// <summary>
    /// Mutes the background music
    /// </summary>
    public virtual void MuteBackgroundMusic()
    {
        if (_backgroundMusic != null)
        {
            _backgroundMusic.mute = true;
        }
    }

    /// <summary>
    /// On enable we start listening for events
    /// </summary>
    protected virtual void OnEnable()
    {
        _loopingSounds = new List<AudioSource>();
    }

    /// <summary>
    /// On disable we stop listening for events
    /// </summary>
	protected virtual void OnDisable()
    {
        if (_enabled)
        {
           
        }
    }

    
}
