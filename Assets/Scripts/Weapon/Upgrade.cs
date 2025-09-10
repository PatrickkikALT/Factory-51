using UnityEngine;

public abstract class Upgrade : MonoBehaviour {
  public UpgradeType type;
  public GameObject bulletPrefab;
  public float damageMultiplier;
  public abstract void Shoot();
  
  protected Transform Player;
  private void Start() {
    Player = GameManager.Instance.player;
  }
}
