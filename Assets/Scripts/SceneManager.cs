using UnityEngine;

public class SceneManager : MonoBehaviour {
  public void SwitchScene(int sceneIndex) {
    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
  }

  public void QuitApplication() {
    Application.Quit();
  }
}
