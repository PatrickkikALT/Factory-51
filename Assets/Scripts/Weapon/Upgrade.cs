using UnityEngine;

public abstract class Upgrade : MonoBehaviour {
  public UpgradeType type;
  public GameObject bulletPrefab;
  public float damageMultiplier;

  protected Transform Player;
  protected Transform Weapon;

  private void Start() {
    Player = GameManager.Instance.player;
    Weapon = Player.FindWeapon().transform;
  }

  public abstract void Shoot();
}