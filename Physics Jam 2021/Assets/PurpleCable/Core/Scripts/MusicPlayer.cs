using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PurpleCable
{
    public class MusicPlayer : MonoBehaviour
    {
        public static MusicPlayer Instance { get; private set; }

        [SerializeField]
        private AudioSource MenuMusicAudioSource = null;

        [SerializeField]
        private AudioSource GameMusicAudioSource = null;

        public static float Volume
        {
            get => PlayerPrefs.GetFloat("MusicVolume", 0.2f);

            set
            {
                PlayerPrefs.SetFloat("MusicVolume", value);

                if (Instance?.MenuMusicAudioSource != null)
                {
                    Instance.MenuMusicAudioSource.volume = value;
                    Instance.GameMusicAudioSource.volume = value;
                }

                VolumeChanged?.Invoke();
            }
        }

        public static event Action VolumeChanged;

        private void Start()
        {
            MenuMusicAudioSource.volume = Volume;
            GameMusicAudioSource.volume = Volume;

            if (Instance != null)
            {
                Instance.PlayMusic();

                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Instance.PlayMusic();
        }

        public void PlayMusic()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Menu":
                case "Credits":
                case "About":
                case "Controls":
                case "Options":
                case "Settings":
                case "Tutorial":
                    GameMusicAudioSource.Stop();

                    if (!MenuMusicAudioSource.isPlaying)
                        MenuMusicAudioSource.Play();
                    break;

                case "Main":
                case "GameOver":
                case "Win":
                default:
                    MenuMusicAudioSource.Stop();

                    if (!GameMusicAudioSource.isPlaying)
                        GameMusicAudioSource.Play();
                    break;
            }
        }
    }
}
