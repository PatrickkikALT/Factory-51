using UnityEngine;

public abstract class Upgrade : MonoBehaviour {
  public UpgradeType type;
  public GameObject bulletPrefab;
  public float damageMultiplier;
  public abstract void Shoot();
  
  protected Transform Player;
  protected Transform Weapon;
  private void Start() {
    Player = GameManager.Instance.player;
    Weapon = Player.FindWeapon().transform;
  }
}
