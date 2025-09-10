using UnityEngine;

public class GameManager : MonoBehaviour {
  public Transform player;
  public Weapon playerWeapon;
  public static GameManager Instance;

  private void Awake() {
    if (Instance is null) Instance = this;
    else Destroy(this);
  }

  void Start() {
    playerWeapon = player.GetChild(1).GetChild(0).GetComponent<Weapon>();
  }
}
