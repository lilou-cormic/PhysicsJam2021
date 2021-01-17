using PurpleCable;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : Singleton<MusicManager>
{
    private AudioSource AudioSource = null;

    [SerializeField] AudioClip MenuMusic = null;

    [SerializeField] AudioClip WinJingle = null;

    [SerializeField] AudioClip LoseJingle = null;

    protected override void Awake()
    {
        MusicPlayer.VolumeChanged += SetVolume;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = MenuMusic;

        base.Awake();

        SetVolume();
    }

    private void Start()
    {
        TryPlayMenuMusic(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        MusicPlayer.VolumeChanged -= SetVolume;
        SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    private void SetVolume()
    {
        AudioSource.volume = MusicPlayer.Volume;
    }

    public static void PlayWinJingle()
    {
        Instance.PlayJingle(Instance.WinJingle);
    }

    public static void PlayLoseJingle()
    {
        Instance.PlayJingle(Instance.LoseJingle);
    }

    private void PlayJingle(AudioClip jingle)
    {
        AudioSource.PlayOneShot(jingle);
    }

    private static void TryPlayMenuMusic(string sceneName)
    {
        switch (sceneName)
        {
            case "Menu":
            case "Settings":
            case "Credits":
            case "Controls":
                if (!Instance.AudioSource.isPlaying)
                    Instance.AudioSource.Play();
                return;

            case "Main":
                Instance.AudioSource.Stop();
                return;
        }
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        if (arg0 != null && (arg0.name == "Win" || arg0.name == "GameOver"))
            Instance.AudioSource.Stop();
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        TryPlayMenuMusic(arg1.name);
    }
}