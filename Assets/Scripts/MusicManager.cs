using UnityEngine;

public class MusicManager : MonoBehaviour {
  public AudioClip menuMusic;
  public AudioClip backgroundMusic;
  public AudioClip bossMusic;
  public AudioSource audioSource;
  
  public void PlayMenuMusic() {
    if (audioSource.isPlaying) {
      audioSource.Stop();
    }
    audioSource.clip = menuMusic;
    audioSource.Play();
  }

  public void PlayBackgroundMusic() {
    if (audioSource.isPlaying) {
      audioSource.Stop();
    }
    audioSource.clip = backgroundMusic;
    audioSource.Play();
  }

  public void PlayBossMusic() {
    if (audioSource.isPlaying) {
      audioSource.Stop();
    }
    audioSource.clip = bossMusic;
    audioSource.Play();
  }
}
