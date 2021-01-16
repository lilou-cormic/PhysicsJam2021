using PurpleCable;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] MusicWithIntro MainMusic = null;

    [SerializeField] AudioSource MenuMusicAudioSource = null;

    [SerializeField] AudioClip WinJingle = null;

    [SerializeField] AudioClip LoseJingle = null;

    protected override void Awake()
    {
        MusicPlayer.VolumeChanged += SetVolume;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        base.Awake();
    }

    private void OnEnable()
    {   
        SetVolume();

        TryPlayMenuMusic();
    }

    private void OnDestroy()
    {
        MusicPlayer.VolumeChanged -= SetVolume;
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    private void SetVolume()
    {
        MenuMusicAudioSource.volume = MusicPlayer.Volume;
    }

    public static void PlayMainMusic()
    {
        Instance.MenuMusicAudioSource.Stop();

        Instance.MainMusic.Play();
    }

    private void PlayMenuMusic()
    {
        if (this != Instance)
            return;

        MainMusic.Stop();

        if (!MenuMusicAudioSource.isPlaying)
            MenuMusicAudioSource.Play();
    }

    public static void PlayWinJingle()
    {
        Instance.StartCoroutine(Instance.DoPlayJingle(Instance.WinJingle));
    }

    public static void PlayLoseJingle()
    {
        Instance.StartCoroutine(Instance.DoPlayJingle(Instance.LoseJingle));
    }

    private IEnumerator DoPlayJingle(AudioClip jingle)
    {
        MainMusic.Stop();

        SoundPlayer.Play(jingle);

        yield return new WaitForSeconds(jingle.length);

        PlayMenuMusic();
    }

    private static void TryPlayMenuMusic()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Menu":
            case "Settings":
            case "Credits":
            case "Controls":
                Instance.PlayMenuMusic();
                return;
        }
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        TryPlayMenuMusic();
    }
}