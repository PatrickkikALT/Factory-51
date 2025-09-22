using UnityEngine;

public abstract class Upgrade : MonoBehaviour {
  public UpgradeType type;
  public GameObject bulletPrefab;
  public float damageMultiplier;

  protected Transform Player;
  protected Weapon WeaponS;
  protected Transform Weapon;

  private void Start() {
    Player = GameManager.Instance.player;
    WeaponS = Player.FindWeapon();
    Weapon = WeaponS.transform;
    
  }

  public abstract void Shoot();
}