using UnityEngine;

public class GameManager : MonoBehaviour {
  public Transform player;

  public static GameManager Instance;

  private void Awake() {
    if (Instance is null) Instance = this;
    else Destroy(this);
  }
}
