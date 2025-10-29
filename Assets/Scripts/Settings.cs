using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour {
  [SerializeField] private AudioMixer mixer;

  public void SetFPS(string value) {
    //input is limited to just integers so should be fine to just parse
    Application.targetFrameRate = int.Parse(value);
  }

  public void SetMusicVolume(float value) {
    var volume = Mathf.Log10(value) * 20;
    mixer.SetFloat("Music", volume);
  }

  public void SetSoundVolume(float value) {
    var volume = Mathf.Log10(value) * 20;
    mixer.SetFloat("SFX", volume);
  }

  public void SetMasterVolume(float value) {
    var volume = Mathf.Log10(value) * 20;
    mixer.SetFloat("Master", volume);
  }
}
