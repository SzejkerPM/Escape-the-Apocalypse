using System;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;

    public AudioSource musicSource, sfxSource, characterSfxSource, zombieSfxSource;

    public UnityEvent onAudioManagerReady;

    [SerializeField]
    private GameStatesSO gameStates;

    private PlayerData playerData;

    public static event Action<bool> OnMusicChanged;
    public static event Action<bool> OnSFXChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        onAudioManagerReady.Invoke();
    }

    private void Start()
    {
        PlayMusic("Menu");

        playerData = SaveSystem.LoadPlayerData();

        SetSoundFromSave();
    }

    private void Update()
    {
        if (gameStates.isGameOver)
        {
            characterSfxSource.Stop();
            zombieSfxSource.Stop();
        }
    }

    public void PlayMusic(string name)
    {
        Sound music = Array.Find(musicSounds, s => s.name == name);

        if (music == null)
        {
            Debug.Log("Music not found!");
        }
        else
        {
            musicSource.clip = music.clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sfx = Array.Find(sfxSounds, s => s.name == name);

        if (sfx == null)
        {
            Debug.Log("SFX not found!");
        }
        else
        {
            sfxSource.PlayOneShot(sfx.clip);
        }
    }

    public void PlayCharacterSFX(string name)
    {

        Sound sfx = Array.Find(sfxSounds, s => s.name == name);

        if (sfx == null)
        {
            Debug.Log("Character SFX not found!");
        }
        else
        {
            if (characterSfxSource.isPlaying)
            {
                characterSfxSource.Stop();
            }

            characterSfxSource.clip = sfx.clip;
            characterSfxSource.loop = true;
            characterSfxSource.Play();
        }
    }

    public void PlayZombieSFX(string name)
    {

        Sound sfx = Array.Find(sfxSounds, s => s.name == name);

        if (sfx == null)
        {
            Debug.Log("Zombie SFX not found!");
        }
        else
        {
            zombieSfxSource.clip = sfx.clip;
            zombieSfxSource.Play();
        }
    }

    public void MuteMusic()
    {
        playerData = SaveSystem.LoadPlayerData();

        if (playerData.isMusicMuted)
        {
            musicSource.volume = 0.8f;
            playerData.isMusicMuted = false;
        }
        else
        {
            musicSource.volume = 0f;
            playerData.isMusicMuted = true;
        }

        SaveSystem.SavePlayerData(playerData);

        OnMusicChanged?.Invoke(!playerData.isMusicMuted);
    }

    public void MuteSFX()
    {
        playerData = SaveSystem.LoadPlayerData();

        if (playerData.isSFXMuted)
        {
            sfxSource.volume = 1f;
            characterSfxSource.volume = 0.9f;
            zombieSfxSource.volume = 0.9f;
            playerData.isSFXMuted = false;
        }
        else
        {
            sfxSource.volume = 0f;
            characterSfxSource.volume = 0f;
            zombieSfxSource.volume = 0f;
            playerData.isSFXMuted = true;
        }

        SaveSystem.SavePlayerData(playerData);

        OnSFXChanged?.Invoke(!playerData.isSFXMuted);
    }

    private void SetSoundFromSave()
    {
        if (!playerData.isMusicMuted)
        {
            musicSource.volume = 0.8f;
        }
        else
        {
            musicSource.volume = 0f;
        }

        if (!playerData.isSFXMuted)
        {
            sfxSource.volume = 1f;
            characterSfxSource.volume = 0.9f;
            zombieSfxSource.volume = 0.9f;
        }
        else
        {
            sfxSource.volume = 0f;
            characterSfxSource.volume = 0f;
            zombieSfxSource.volume = 0f;
        }
    }


}
