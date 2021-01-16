using System.Collections;
using UnityEngine;

namespace PurpleCable
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicWithIntro : MonoBehaviour
    {
        private AudioSource AudioSource = null;

        [SerializeField] AudioClip Intro = null;

        [SerializeField] AudioClip Loop = null;

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = Loop;

            SetVolume();

            MusicPlayer.VolumeChanged += SetVolume;
        }

        private void OnDestroy()
        {
            MusicPlayer.VolumeChanged -= SetVolume;
        }

        public void Play()
        {
            StartCoroutine(DoPlay());
        }

        private IEnumerator DoPlay()
        {
            AudioSource.PlayOneShot(Intro);

            yield return new WaitForSecondsRealtime(Intro.length);

            AudioSource.Play();
        }

        private void SetVolume()
        {
            AudioSource.volume = MusicPlayer.Volume;
        }

        public void Stop()
        {
            AudioSource.Stop();
        }
    }
}
